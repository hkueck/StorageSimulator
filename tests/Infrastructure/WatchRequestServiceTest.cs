using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using FluentAssertions;
using Moq;
using Prism.Events;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Infrastructure;
using Xunit;
using MovementRequest = StorageSimulator.Core.Model.MovementRequest;

namespace StorageSimulatorTests.Infrastructure
{
    public class WatchRequestServiceTest
    {
        private string _watchpath = "./watchPath";

        [Fact]
        public void RunShouldStartCreatedWatching()
        {
            var requestEvent = new EventMock();
            var requestFile = $"{_watchpath}1/MovementRequest_V.XML";
            DeleteMovementDirectory($"{_watchpath}1");
            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(e => e.GetEvent<PubSubEvent<StorageSimulator.Core.Events.MovementRequestEvent>>()).Returns(requestEvent);
            var config = new Mock<IStorageSimulatorConfig>();
            config.Setup(c => c.CommunicationPath).Returns($"{_watchpath}1");
            var service = new WatchRequestService(eventAggregator.Object, config.Object);
            
            service.Run();
            CreateMovementRequest(requestFile);

            Task.Delay(45).Wait();
            eventAggregator.Verify(e => e.GetEvent<PubSubEvent<StorageSimulator.Core.Events.MovementRequestEvent>>());
            requestEvent.PublishCalled.Should().BeTrue();
        }

        [Fact]
        public void RunShouldStartRenameWatching()
        {
            var requestEvent = new EventMock();
            var tempFile = $"{_watchpath}/tempFile";
            var requestFile = $"{_watchpath}/MovementRequest_V.XML";
            DeleteMovementDirectory(_watchpath);
            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(e => e.GetEvent<PubSubEvent<StorageSimulator.Core.Events.MovementRequestEvent>>()).Returns(requestEvent);
            var config = new Mock<IStorageSimulatorConfig>();
            config.Setup(c => c.CommunicationPath).Returns(_watchpath);
            var service = new WatchRequestService(eventAggregator.Object, config.Object);
            
            service.Run();
            CreateMovementRequest(tempFile);
            File.Move(tempFile, requestFile);

            Task.Delay(25).Wait();
            eventAggregator.Verify(e => e.GetEvent<PubSubEvent<StorageSimulator.Core.Events.MovementRequestEvent>>());
            requestEvent.PublishCalled.Should().BeTrue();
        }

        class EventMock : PubSubEvent<StorageSimulator.Core.Events.MovementRequestEvent>
        {
            public override void Publish(StorageSimulator.Core.Events.MovementRequestEvent payload)
            {
                PublishCalled = true;
            }

            public bool PublishCalled { get; set; }
        }
        
        private void CreateMovementRequest(string requestFile)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("","");
            var serializer = new XmlSerializer(typeof(MovementRequest));
            var movement = new MovementRequest();
            movement.Data.Add(new MovementData{Barcode = "12345"});
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            using (var stream = new StreamWriter(requestFile))
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, movement, ns);
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        private static void DeleteMovementDirectory(string watchPath)
        {
            if (Directory.Exists(watchPath))
            {
                var files = Directory.GetFiles(watchPath);
                foreach (var file in files)
                {
                    File.Delete(file);
                }
                Directory.Delete(watchPath);
            }
        }
    }
}