using System;
using System.IO;
using System.Xml.Serialization;
using FluentAssertions;
using Moq;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;
using StorageSimulator.Core.UseCases;
using Xunit;

namespace StorageSimulatorTests.UseCases
{
    public class SendResponseUseCaseTest
    {
        [Fact]
        public void ExecuteShouldWriteResponseFile()
        {
            var responsePath = $"./responsePath";
            var responseFile = $"{responsePath}/MovementResponse_V.xml";
            var expectedTicket = Guid.NewGuid();
            var expectedTimestamp = DateTime.UtcNow;
            PrepareResponseDirectory(responsePath);
            var expected = new MovementResponse()
            {
                Info = "info", Quantity = 2, Source = "source", Target = "target", Status = AutomationStatus.InsertionSucceeded,
                Ticket = expectedTicket, Timestamp = expectedTimestamp, SourceCompartment = "2", TargetCompartment = "3",
            };
            expected.Data.Add(new MovementData{Barcode = "barcode", Index = "2"});
            PrepareResponseDirectory(responsePath);
            var config = new Mock<IStorageSimulatorConfig>();
            config.Setup(c => c.CommunicationPath).Returns(responsePath);
            ISendResponseUseCase useCase = new SendResponseUseCase(config.Object);

            useCase.Execute(expected);

            File.Exists(responseFile).Should().BeTrue();
            var xmlSerializer = new XmlSerializer(typeof(MovementResponse));
            using var reader = new FileStream(responseFile, FileMode.Open);
            var response = (MovementResponse) xmlSerializer.Deserialize(reader);

            response.Info.Should().Be("info");
            response.Quantity.Should().Be(2);
            response.Source.Should().Be("source");
            response.Target.Should().Be("target");
            response.Status.Should().Be(AutomationStatus.InsertionSucceeded);
            response.Ticket.Should().Be(expectedTicket);
            response.Timestamp.Should().Be(expectedTimestamp);
            response.SourceCompartment.Should().Be("2");
            response.TargetCompartment.Should().Be("3");
        }

        private static void PrepareResponseDirectory(string responsePath)
        {
            if (Directory.Exists(responsePath))
            {
                var files = Directory.GetFiles(responsePath);
                foreach (var file in files)
                {
                    File.Delete(file);
                }
                Directory.Delete(responsePath);
            }
            else
            {
                Directory.CreateDirectory(responsePath);
            }
        }
    }
}