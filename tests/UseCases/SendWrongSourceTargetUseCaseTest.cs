using System;
using FluentAssertions;
using Moq;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;
using StorageSimulator.Core.UseCases;
using Xunit;

namespace StorageSimulatorTests.UseCases
{
    public class SendWrongSourceTargetUseCaseTest
    {
        [Fact]
        public void ExecuteShouldWriteResponseFile()
        {
            MovementResponse response = null;
            var sendResponseUseCase = new Mock<ISendResponseUseCase>();
            sendResponseUseCase.Setup(s => s.Execute(It.IsAny<MovementResponse>()))
                .Callback<MovementResponse>(movementResponse => response = movementResponse);
            var expectedTicket = Guid.NewGuid();
            var expectedTimestamp = DateTime.UtcNow;
            var expected = new MovementRequest()
            {
                Info = "info", Quantity = 2, Source = "source", Target = "target", Task = AutomationTasks.Transport,
                Ticket = expectedTicket, Timestamp = expectedTimestamp, SourceCompartment = "2", TargetCompartment = "3",
            };
            expected.Data.Add(new MovementData{Barcode = "barcode", Index = "2"});
            ISendWrongSourceTargetUseCase useCase = new SendWrongSourceTargetUseCase(sendResponseUseCase.Object);

            useCase.Execute(expected);

            response.Should().NotBeNull();
            response.Info.Should().Be("Wrong source or target: info");
            response.Quantity.Should().Be(2);
            response.Source.Should().Be("source");
            response.Target.Should().Be("target");
            response.Status.Should().Be(AutomationStatus.InvalidOrderTargetSourceNotFound);
            response.Ticket.Should().Be(expectedTicket);
            response.TimestampString.Should().Be(expectedTimestamp.ToString("dd.MM.yyyy hh:mm:ss"));
            response.SourceCompartment.Should().Be("2");
            response.TargetCompartment.Should().Be("3");
        }
    }
}
