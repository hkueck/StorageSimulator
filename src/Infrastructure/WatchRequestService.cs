using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;
using MovementRequest = StorageSimulator.Core.Model.MovementRequest;

namespace StorageSimulator.Infrastructure
{
    public class WatchRequestService : IWatchRequestService
    {
        private const string MovementRequestFile = "MovementRequest_V.XML";
        private readonly IEventAggregator _eventAggregator;
        private readonly IStorageSimulatorConfig _configuration;
        private FileSystemWatcher _watcher;
        private IList<Guid> _receivedTickets = new List<Guid>();

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
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(MovementRequest));
                if (File.Exists(RequestFile))
                {
                    using var reader = new FileStream(RequestFile, FileMode.Open);
                    var request = (MovementRequest) xmlSerializer.Deserialize(reader);
                    reader.Close();
                    if (_receivedTickets.All(t => t != request.Ticket))
                    {
                        _receivedTickets.Add(request.Ticket);
                        SendRequest(request);
                    }

                    File.Delete(RequestFile);
                }
            }
            catch (Exception exception)
            {
                var exceptionEvent = _eventAggregator.GetEvent<PubSubEvent<ExceptionEvent>>();
                exceptionEvent.Publish(new ExceptionEvent {Exception = exception});
            }
        }

        private void SendRequest(MovementRequest request)
        {
            var movementRequest = new Core.Events.MovementRequestEvent {MovementRequest = request};
            var requestEvent = _eventAggregator.GetEvent<PubSubEvent<Core.Events.MovementRequestEvent>>();
            requestEvent.Publish(movementRequest);
        }
    }
}