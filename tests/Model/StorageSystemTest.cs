using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;
using Xunit;

namespace StorageSimulatorTests.Model
{
    public class StorageSystemTest
    {
        private StorageSystem _storageSystem;
        private IEventAggregator _eventAggregator;
        private Mock<IWatchRequestUseCase> _watchUseCase;
        private Mock<ISendResponseUseCase> _sendUseCase;
        private Mock<IAnalyseRequestUseCase> _requestAnalyser;

        public StorageSystemTest()
        {
            _watchUseCase = new Mock<IWatchRequestUseCase>();
            _sendUseCase = new Mock<ISendResponseUseCase>();
            _requestAnalyser = new Mock<IAnalyseRequestUseCase>();
            _eventAggregator = new EventAggregator();
            
            _storageSystem = new StorageSystem(_watchUseCase.Object, _sendUseCase.Object, _requestAnalyser.Object, _eventAggregator);

            _requestAnalyser.Setup(a => a.Execute(It.IsAny<MovementRequest>())).Returns(new MovementResponse());
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
            var requestEvent = _eventAggregator.GetEvent<PubSubEvent<MovementRequestEvent>>();
            var movement = new MovementRequest
            {
                Info = "info", Quantity = 2, Source = "source", Target = "target", Task = AutomationTasks.Insert,
                Ticket = Guid.NewGuid(), Timestamp = DateTime.UtcNow, SourceCompartment = "4", TargetCompartment = "2"
            };
            var movementRequest = new MovementRequestEvent{MovementRequest = movement};
            
            requestEvent.Publish(movementRequest);

            sentResponse.Should().NotBeNull();
        }

        [Fact]
        public void ReceivingRequestShouldCallRequestAnalyser()
        {
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Info = "part in new storage point", Quantity = 1, Target = "TV01",
                TargetCompartment = "1"
            };
            request.Data.Add(new MovementData {Barcode = "12345"});
            var movementRequest = new MovementRequestEvent{MovementRequest = request};
            var requestEvent = _eventAggregator.GetEvent<PubSubEvent<MovementRequestEvent>>();
            
            requestEvent.Publish(movementRequest);

            _requestAnalyser.Verify(a => a.Execute(request));
        }

        [Fact]
        public void AddStoragePointShouldAddStoragePoint()
        {
            var expected = new StoragePoint();
            
            _storageSystem.AddStoragePoint(expected);

            _storageSystem.StoragePoints.Count.Should().Be(1);
            var storagePoint = _storageSystem.StoragePoints.First();
            storagePoint.Should().Be(expected);
        }
    }
}
