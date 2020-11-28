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
    public class SendInsertSucceededUseCaseTest
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
                Info = "info", Quantity = 2, Source = "", Target = "target", Task = AutomationTasks.Insert,
                Ticket = expectedTicket, Timestamp = expectedTimestamp, SourceCompartment = "", TargetCompartment = "3",
            };
            expected.Data.Add(new MovementData{Barcode = "barcode", Index = "2"});
            var useCase = new SendInsertSucceededUseCase(sendResponseUseCase.Object);

            useCase.Execute(expected);

            response.Should().NotBeNull();
            response.Info.Should().Be("Insert: info");
            response.Quantity.Should().Be(2);
            response.Source.Should().Be("");
            response.Target.Should().Be("target");
            response.Status.Should().Be(AutomationStatus.InsertionSucceeded);
            response.Ticket.Should().Be(expectedTicket);
            response.TimestampString.Should().Be(expectedTimestamp.ToString("dd.MM.yyyy hh:mm:ss"));
            response.SourceCompartment.Should().Be("");
            response.TargetCompartment.Should().Be("3");
        }
    }
}
