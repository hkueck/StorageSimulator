using System.Collections.Generic;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.Core.Model
{
    public class StorageSystem : IStorageSystem
    {
        private readonly IWatchRequestUseCase _watchRequestUseCase;
        private readonly IAnalyseRequestUseCase _analyseRequestUseCase;
        private PubSubEvent<AddStoreEvent> _addStoreEvent;
        private PubSubEvent<AddShelfEvent> _addShelfEvent;
        private PubSubEvent<AddPartEvent> _addPartEvent;
        private PubSubEvent<AddStoragePointEvent> _addStoragePointEvent;
        private PubSubEvent<InsertPartEvent> _insertPartEvent;
        private PubSubEvent<RemovePartFromShelfEvent> _removePartFromShelfEvent;
        private PubSubEvent<RemovePartFromStoragePointEvent> _removePartFromStoragePointEvent;
        private PubSubEvent<AddDeliveryPointEvent> _addDeliveryPointEvent;
        private PubSubEvent<InsertPartToDeliveryEvent> _insertPartToDeliveryPointEvent;

        public IList<Store> Stores { get; } = new List<Store>();
        public IList<StoragePoint> DeliveryPoints { get; } = new List<StoragePoint>();
        public IList<StoragePoint> StoragePoints { get; } = new List<StoragePoint>();

        public StorageSystem(IWatchRequestUseCase watchRequestUseCase, IAnalyseRequestUseCase analyseRequestUseCase,
            IEventAggregator eventAggregator)
        {
            var requestEvent = eventAggregator.GetEvent<PubSubEvent<MovementRequestEvent>>();
            requestEvent.Subscribe(OnMovementRequest);
            _addStoreEvent = eventAggregator.GetEvent<PubSubEvent<AddStoreEvent>>();
            _addShelfEvent = eventAggregator.GetEvent<PubSubEvent<AddShelfEvent>>();
            _addPartEvent = eventAggregator.GetEvent<PubSubEvent<AddPartEvent>>();
            _addStoragePointEvent = eventAggregator.GetEvent<PubSubEvent<AddStoragePointEvent>>();
            _insertPartEvent = eventAggregator.GetEvent<PubSubEvent<InsertPartEvent>>();
            _removePartFromShelfEvent = eventAggregator.GetEvent<PubSubEvent<RemovePartFromShelfEvent>>();
            _removePartFromStoragePointEvent = eventAggregator.GetEvent<PubSubEvent<RemovePartFromStoragePointEvent>>();
             _addDeliveryPointEvent = eventAggregator.GetEvent<PubSubEvent<AddDeliveryPointEvent>>();
             _insertPartToDeliveryPointEvent = eventAggregator.GetEvent<PubSubEvent<InsertPartToDeliveryEvent>>();
             _watchRequestUseCase = watchRequestUseCase;
            _analyseRequestUseCase = analyseRequestUseCase;
            _analyseRequestUseCase.StorageSystem = this;
            _watchRequestUseCase.Execute();
        }

        private void OnMovementRequest(MovementRequestEvent movementRequestEvent)
        {
            _analyseRequestUseCase.Execute(movementRequestEvent.MovementRequest);
        }

        public void AddStoragePoint(StoragePoint storagePoint)
        {
            StoragePoints.Add(storagePoint);
            _addStoragePointEvent.Publish(new AddStoragePointEvent{StoragePoint = storagePoint});
        }

        public void AddStore(Store store)
        {
            Stores.Add(store);
            _addStoreEvent.Publish(new AddStoreEvent {Store = store});
        }

        public void AddPartToShelf(Shelf shelf, Part part)
        {
            shelf.Parts.Add(part);
            _addPartEvent.Publish(new AddPartEvent {Part = part, Shelf = shelf});
        }

        public void AddShelfToStore(Store store, Shelf shelf)
        {
            store.Shelves.Add(shelf);
            _addShelfEvent.Publish(new AddShelfEvent {Shelf = shelf, Store = store});
        }

        public void RemovePartFromShelf(Shelf shelf, Part part)
        {
            shelf.Parts.Remove(part);
            _removePartFromShelfEvent.Publish(new RemovePartFromShelfEvent{Part = part, Shelf = shelf});
        }

        public void AddPartToStoragePoint(StoragePoint storagePoint, Part part)
        {
            storagePoint.Parts.Add(part);
            _insertPartEvent.Publish(new InsertPartEvent {Part = part, StoragePoint = storagePoint.Name});
        }

        public void AddDeliveryPoint(StoragePoint deliveryPoint)
        {
            DeliveryPoints.Add(deliveryPoint);
            _addDeliveryPointEvent.Publish(new AddDeliveryPointEvent{DeliveryPoint = deliveryPoint});
        }

        public void AddPartToDeliveryPoint(StoragePoint deliveryPoint, Part part)
        {
            deliveryPoint.Parts.Add(part);
            _insertPartToDeliveryPointEvent.Publish(new InsertPartToDeliveryEvent{DeliveryPoint = deliveryPoint, Part = part});
        }

        public void RemovePartFromStoragePoint(StoragePoint storagePoint, Part part)
        {
            storagePoint.Parts.Remove(part);
            _removePartFromStoragePointEvent.Publish(new RemovePartFromStoragePointEvent{Part = part, StoragePoint = storagePoint});
        }
    }
}