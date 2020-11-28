using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
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
            _requestAnalyser = new Mock<IAnalyseRequestUseCase>();
            _eventAggregator = new EventAggregator();
            _sendUseCase = new Mock<ISendResponseUseCase>();
            _storageSystem = new StorageSystem(_watchUseCase.Object, _requestAnalyser.Object, _eventAggregator);
        }
        
        [Fact]
        public void ConstructorShouldCallRequestUseCase()
        {
            _storageSystem.Should().NotBeNull();
            _watchUseCase.Verify(uc => uc.Execute());
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

        [Fact]
        public void OnMovementRequestCantWriteResponseShouldSendExceptionMessage()
        {
            _sendUseCase.Setup(u => u.Execute(It.IsAny<MovementResponse>())).Throws<IOException>();
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Info = "part in new storage point", Quantity = 1, Target = "TV01",
                TargetCompartment = "1"
            };
            request.Data.Add(new MovementData {Barcode = "12345"});
            var movementRequest = new MovementRequestEvent{MovementRequest = request};
            var requestEvent = _eventAggregator.GetEvent<PubSubEvent<MovementRequestEvent>>();
            
            requestEvent.Publish(movementRequest);

            Task.Delay(25).Wait();
        }
    }
}
