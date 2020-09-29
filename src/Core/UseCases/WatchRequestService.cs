using System;
using System.IO;
using System.Xml.Serialization;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.UseCases
{
    public class WatchRequestService: IWatchRequestService
    {
        private const string MovementRequestFile = "MovementRequest_V.XML";
        private readonly IEventAggregator _eventAggregator;
        private readonly IStorageSimulatorConfig _configuration;
        private FileSystemWatcher _watcher;

        private string RequestFile => Path.Combine(_configuration.CommunicationPath, MovementRequestFile);

        public WatchRequestService(IEventAggregator eventAggregator, IStorageSimulatorConfig configuration)
        {
            _eventAggregator = eventAggregator;
            _configuration = configuration;
            CreateWatchPath(configuration.CommunicationPath);
        }

        private void CreateWatchPath(string watchPath)
        {
            if (!Directory.Exists(watchPath))
            {
                Directory.CreateDirectory(watchPath);
            }
        }

        public void Run()
        {
            _watcher = new FileSystemWatcher(_configuration.CommunicationPath);
            _watcher.Filter = "*";
            _watcher.Created += RequestOnCreated;
            _watcher.Renamed += RequestOnCreated;
            _watcher.Changed += RequestOnCreated;
            _watcher.EnableRaisingEvents = true;
        }

        private void RequestOnCreated(object sender, FileSystemEventArgs e)
        {
            var xmlSerializer = new XmlSerializer(typeof(Movement));
            if (File.Exists(RequestFile))
            {
                using var reader = new FileStream(RequestFile, FileMode.Open);
                var request = (Movement) xmlSerializer.Deserialize(reader);
                SendRequest(request);
                try
                {
                    File.Delete(RequestFile);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void SendRequest(Movement request)
        {
            var movementRequest = new MovementRequest{Request = request};
            var requestEvent = _eventAggregator.GetEvent<PubSubEvent<MovementRequest>>();
            requestEvent.Publish(movementRequest);
        }
    }
}