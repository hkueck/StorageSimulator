using System;
using FluentAssertions;
using Moq;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;
using Xunit;
using MovementRequest = StorageSimulator.Core.Model.MovementRequest;

namespace StorageSimulatorTests.Model
{
    public class StorageSystemTest
    {
        private StorageSystem _storageSystem;
        private IEventAggregator _eventAggregator;
        private Mock<IWatchRequestUseCase> _watchUseCase;
        private Mock<ISendResponseUseCase> _sendUseCase;

        public StorageSystemTest()
        {
            _watchUseCase = new Mock<IWatchRequestUseCase>();
            _sendUseCase = new Mock<ISendResponseUseCase>();

            _eventAggregator = new EventAggregator();
            _storageSystem = new StorageSystem(_watchUseCase.Object, _sendUseCase.Object, _eventAggregator);
        }
        
        [Fact]
        public void ConstructorShouldCallRequestUseCase()
        {
            _storageSystem.Should().NotBeNull();
            _watchUseCase.Verify(uc => uc.Execute());
        }

        [Fact]
        public void ReceivingRequestShouldSendResponse()
        {
            MovementResponse sentResponse = null;
            _sendUseCase.Setup(u => u.Execute(It.IsAny<MovementResponse>())).Callback<MovementResponse>(response => sentResponse = response);
            var requestEvent = _eventAggregator.GetEvent<PubSubEvent<StorageSimulator.Core.Events.MovementRequest>>();
            var movement = new MovementRequest
            {
                Info = "info", Quantity = 2, Source = "source", Target = "target", Task = AutomationTasks.Insert,
                Ticket = Guid.NewGuid(), Timestamp = DateTime.UtcNow, SourceCompartment = "4", TargetCompartment = "2"
            };
            var movementRequest = new StorageSimulator.Core.Events.MovementRequest{Request = movement};
            
            requestEvent.Publish(movementRequest);

            sentResponse.Should().NotBeNull();
            sentResponse.Info.Should().Be("info");
            sentResponse.Quantity.Should().Be(2);
            sentResponse.Source.Should().Be("source");
            sentResponse.Target.Should().Be("target");
            sentResponse.Status.Should().Be(AutomationStatus.InsertionSucceeded);
            sentResponse.Ticket.Should().Be(movement.Ticket);
            sentResponse.Timestamp.Should().NotBeOnOrAfter(DateTime.UtcNow);
            sentResponse.SourceCompartment.Should().Be("4");
            sentResponse.TargetCompartment.Should().Be("2");
        }
    }
}
