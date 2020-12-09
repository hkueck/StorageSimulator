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
            var movementRequest = new MovementRequestEvent {MovementRequest = request};
            var requestEvent = _eventAggregator.GetEvent<PubSubEvent<MovementRequestEvent>>();

            requestEvent.Publish(movementRequest);

            _requestAnalyser.Verify(a => a.Execute(request));
        }

        [Fact]
        public void AddStoragePointShouldAddStoragePoint()
        {
            StoragePoint receivedStoragePoint = null;
            var expected = new StoragePoint();
            var addStoragePointEvent = _eventAggregator.GetEvent<PubSubEvent<AddStoragePointEvent>>();
            addStoragePointEvent.Subscribe(receivedEvent => receivedStoragePoint = receivedEvent.StoragePoint);

            _storageSystem.AddStoragePoint(expected);

            Task.Delay(25).Wait();
            _storageSystem.StoragePoints.Count.Should().Be(1);
            var storagePoint = _storageSystem.StoragePoints.First();
            storagePoint.Should().Be(expected);
            receivedStoragePoint.Should().Be(expected);
        }

        [Fact]
        public void AddStoreShouldAddStore()
        {
            Store receivedStore = null;
            var expected = new Store();
            var addStoreEvent = _eventAggregator.GetEvent<PubSubEvent<AddStoreEvent>>();
            addStoreEvent.Subscribe(receivedEvent => receivedStore = receivedEvent.Store);

            _storageSystem.AddStore(expected);

            Task.Delay(25).Wait();
            _storageSystem.Stores.Count.Should().Be(1);
            var store = _storageSystem.Stores.First();
            store.Should().Be(expected);
            receivedStore.Should().Be(expected);
        }

        [Fact]
        public void AddShelfToStoreShouldAddShelf()
        {
            Shelf receivedShelf = null;
            var expected = new Shelf();
            var store = new Store();
            _storageSystem.Stores.Add(store);
            var addShelfEvent = _eventAggregator.GetEvent<PubSubEvent<AddShelfEvent>>();
            addShelfEvent.Subscribe(e => receivedShelf = e.Shelf);

            _storageSystem.AddShelfToStore(store, expected);

            Task.Delay(25).Wait();
            store.Shelves.Count.Should().Be(1);
            var shelf = store.Shelves.First();
            shelf.Should().Be(expected);
            receivedShelf.Should().Be(expected);
        }

        [Fact]
        public void AddPartToShelfShouldAddPart()
        {
            Part receivedPart = null;
            var expected = new Part();
            var store = new Store();
            var shelf = new Shelf();
            store.Shelves.Add(shelf);
            _storageSystem.Stores.Add(store);
            var addPartEvent = _eventAggregator.GetEvent<PubSubEvent<AddPartEvent>>();
            addPartEvent.Subscribe(e => receivedPart = e.Part);

            _storageSystem.AddPartToShelf(store.Shelves.First(), expected);

            Task.Delay(25).Wait();
            shelf.Parts.Count.Should().Be(1);
            shelf.Parts.First().Should().Be(expected);
            receivedPart.Should().Be(expected);
        }

        [Fact]
        public void AddPartToStoragePointShouldAddPart()
        {
            Part receivedPart = null;
            var expected = new Part();
            var storagePoint = new StoragePoint();
            _storageSystem.StoragePoints.Add(storagePoint);
            var insertPartEvent = _eventAggregator.GetEvent<PubSubEvent<InsertPartEvent>>();
            insertPartEvent.Subscribe(e => receivedPart = e.Part);

            _storageSystem.AddPartToStoragePoint(storagePoint, expected);

            storagePoint.Parts.Count.Should().Be(1);
            storagePoint.Parts.First().Should().Be(expected);
            receivedPart.Should().Be(expected);
        }

        [Fact]
        public void RemovePartFromShelfShouldRemovePart()
        {
            Part receivedPart = null;
            Shelf receivedShelf = null;
            var store = new Store();
            var expectedShelf = new Shelf {Number = "1"};
            var expected = new Part {Barcode = "expected"};
            expectedShelf.Parts.Add(expected);
            store.Shelves.Add(expectedShelf);
            _storageSystem.Stores.Add(store);
            var removePartEvent = _eventAggregator.GetEvent<PubSubEvent<RemovePartFromShelfEvent>>();
            removePartEvent.Subscribe(e =>
            {
                receivedPart = e.Part;
                receivedShelf = e.Shelf;
            });

            _storageSystem.RemovePartFromShelf(expectedShelf, expected);

            Task.Delay(25).Wait();
            expectedShelf.Parts.Count.Should().Be(0);
            receivedPart.Should().Be(expected);
            receivedShelf.Should().Be(expectedShelf);
        }

        [Fact]
        public void RemovePartFromStoragePointShouldRemovePart()
        {
            Part receivedPart = null;
            StoragePoint reveivedStoragePoint = null;
            var expectedStoragePoint = new StoragePoint();
            var expected = new Part();
            expectedStoragePoint.Parts.Add(expected);
            _storageSystem.StoragePoints.Add(expectedStoragePoint);
            var removePartEvent = _eventAggregator.GetEvent<PubSubEvent<RemovePartFromStoragePointEvent>>();
            removePartEvent.Subscribe(e =>
            {
                receivedPart = e.Part;
                reveivedStoragePoint = e.StoragePoint;
            });

            _storageSystem.RemovePartFromStoragePoint(expectedStoragePoint, expected);

            Task.Delay(25).Wait();
            expectedStoragePoint.Parts.Count.Should().Be(0);
            receivedPart.Should().Be(expected);
            reveivedStoragePoint.Should().Be(expectedStoragePoint);
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
            var movementRequest = new MovementRequestEvent {MovementRequest = request};
            var requestEvent = _eventAggregator.GetEvent<PubSubEvent<MovementRequestEvent>>();

            requestEvent.Publish(movementRequest);

            Task.Delay(25).Wait();
        }

        [Fact]
        public void AddDeliveryPointShouldAddDeliveryPoint()
        {
            var storagePoint = new StoragePoint {Name = "AV01"};

            _storageSystem.AddDeliveryPoint(storagePoint);

            _storageSystem.DeliveryPoints.Count.Should().Be(1);
            var deliveryPoint = _storageSystem.DeliveryPoints.First();
            deliveryPoint.Name.Should().Be("AV01");
        }

        [Fact]
        public void AddDeliveryPointShouldSendAddDeliveryPointEvent()
        {
            StoragePoint receivedDeliveryPoint = null;
            var storagePoint = new StoragePoint {Name = "AV01"};

            var addEvent = _eventAggregator.GetEvent<PubSubEvent<AddDeliveryPointEvent>>();
            addEvent.Subscribe(a => receivedDeliveryPoint = a.DeliveryPoint);
            _storageSystem.AddDeliveryPoint(storagePoint);

            Task.Delay(25).Wait();
            receivedDeliveryPoint.Should().Be(storagePoint);
        }

        [Fact]
        public void AddPartToDeliveryPointShouldAddPartToDeliveryPoint()
        {
            StoragePoint receivedDeliveryPoint = null;
            Part receivedPart = null;
            var expected = new Part();
            var storagePoint = new StoragePoint();
            _storageSystem.DeliveryPoints.Add(storagePoint);
            var insertPartEvent = _eventAggregator.GetEvent<PubSubEvent<InsertPartToDeliveryEvent>>();
            insertPartEvent.Subscribe(e =>
            {
                receivedPart = e.Part;
                receivedDeliveryPoint = e.DeliveryPoint;
            });

            _storageSystem.AddPartToDeliveryPoint(storagePoint, expected);

            storagePoint.Parts.Count.Should().Be(1);
            storagePoint.Parts.First().Should().Be(expected);
            receivedPart.Should().Be(expected);
            receivedDeliveryPoint.Should().Be(storagePoint);
        }
    }
}
