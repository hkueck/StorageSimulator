using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using FluentAssertions;
using Moq;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.UseCases;
using Xunit;

namespace StorageSimulatorTests.UseCases
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
            eventAggregator.Setup(e => e.GetEvent<PubSubEvent<MovementRequest>>()).Returns(requestEvent);
            var config = new Mock<IStorageSimulatorConfig>();
            config.Setup(c => c.WatchPath).Returns($"{_watchpath}1");
            var service = new WatchRequestService(eventAggregator.Object, config.Object);
            
            service.Run();
            CreateMovementRequest(requestFile);

            Task.Delay(25).Wait();
            eventAggregator.Verify(e => e.GetEvent<PubSubEvent<MovementRequest>>());
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
            eventAggregator.Setup(e => e.GetEvent<PubSubEvent<MovementRequest>>()).Returns(requestEvent);
            var config = new Mock<IStorageSimulatorConfig>();
            config.Setup(c => c.WatchPath).Returns(_watchpath);
            var service = new WatchRequestService(eventAggregator.Object, config.Object);
            
            service.Run();
            CreateMovementRequest(tempFile);
            File.Move(tempFile, requestFile);

            Task.Delay(25).Wait();
            eventAggregator.Verify(e => e.GetEvent<PubSubEvent<MovementRequest>>());
            requestEvent.PublishCalled.Should().BeTrue();
        }

        class EventMock : PubSubEvent<MovementRequest>
        {
            public override void Publish(MovementRequest payload)
            {
                PublishCalled = true;
            }

            public bool PublishCalled { get; set; }
        }
        
        private void CreateMovementRequest(string requestFile)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("","");
            var serializer = new XmlSerializer(typeof(Movement));
            var movement = new Movement();
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