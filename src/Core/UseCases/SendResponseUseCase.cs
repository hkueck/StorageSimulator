using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.UseCases
{
    public class SendResponseUseCase : ISendResponseUseCase
    {
        private readonly IStorageSimulatorConfig _configuration;
        private readonly IEventAggregator _eventAggregator;

        public SendResponseUseCase(IStorageSimulatorConfig configuration, IEventAggregator eventAggregator)
        {
            _configuration = configuration;
            _eventAggregator = eventAggregator;
        }

        public void Execute(MovementResponse movementResponse)
        {
            var responseFile = $"{_configuration.CommunicationPath}/MovementResponse_V.xml";
            var tempFile = $"{_configuration.CommunicationPath}/MovementResponse_V.tmp";
            if (!Directory.Exists(_configuration.CommunicationPath))
            {
                Directory.CreateDirectory(_configuration.CommunicationPath);
            }

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("","");
            var serializer = new XmlSerializer(typeof(MovementResponse));
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            using (var stream = new StreamWriter(tempFile))
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, movementResponse, ns);
                    writer.Flush();
                    writer.Close();
                }
            }
            File.Move(tempFile, responseFile);
            var responseEvent = _eventAggregator.GetEvent<PubSubEvent<MovementResponseEvent>>();
            responseEvent.Publish(new MovementResponseEvent {Response = movementResponse});
        }
    }
}