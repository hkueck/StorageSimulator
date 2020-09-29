using System.IO;
using System.Xml;
using System.Xml.Serialization;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.UseCases
{
    public class SendResponseUseCase : ISendResponseUseCase
    {
        private readonly IStorageSimulatorConfig _configuration;

        public SendResponseUseCase(IStorageSimulatorConfig configuration)
        {
            _configuration = configuration;
        }

        public void Execute(Movement movement)
        {
            var responseFile = $"{_configuration.CommunicationPath}/MovementResponse_V.xml";
            var tempFile = $"{_configuration.CommunicationPath}/MovementResponse_V.tmp";
            if (!Directory.Exists(_configuration.CommunicationPath))
            {
                Directory.CreateDirectory(_configuration.CommunicationPath);
            }

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("","");
            var serializer = new XmlSerializer(typeof(Movement));
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            using (var stream = new StreamWriter(tempFile))
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, movement, ns);
                    writer.Flush();
                    writer.Close();
                }
            }
            File.Move(tempFile, responseFile);
        }
    }
}