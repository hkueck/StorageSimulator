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

        public IList<Store> Stores { get; } = new List<Store>();
        public IList<StoragePoint> StoragePoints { get; } = new List<StoragePoint>();

        public StorageSystem(IWatchRequestUseCase watchRequestUseCase, IAnalyseRequestUseCase analyseRequestUseCase,
            IEventAggregator eventAggregator)
        {
            var requestEvent = eventAggregator.GetEvent<PubSubEvent<MovementRequestEvent>>();
            requestEvent.Subscribe(OnMovementRequest);
            _addStoreEvent = eventAggregator.GetEvent<PubSubEvent<AddStoreEvent>>();
            _addShelfEvent = eventAggregator.GetEvent<PubSubEvent<AddShelfEvent>>();
            _addPartEvent = eventAggregator.GetEvent<PubSubEvent<AddPartEvent>>();
            _watchRequestUseCase = watchRequestUseCase;
            _analyseRequestUseCase = analyseRequestUseCase;
            _analyseRequestUseCase.StorageSystem = this;
            _watchRequestUseCase.Execute();
        }

        private void OnMovementRequest(MovementRequestEvent movementRequestEvent)
        {
            var movement = movementRequestEvent.MovementRequest;
            _analyseRequestUseCase.Execute(movement);
        }

        public void AddStoragePoint(StoragePoint storagePoint)
        {
            StoragePoints.Add(storagePoint);
        }

        public void AddStore(Store store)
        {
            Stores.Add(store);
            _addStoreEvent.Publish(new AddStoreEvent {Store = store});
        }

        public void AddPartToShelf(Shelf shelf, Part part)
        {
            shelf.Parts.Add(part);
            _addPartEvent.Publish(new AddPartEvent{Part = part, Shelf = shelf});
        }

        public void AddShelfToStore(Store store, Shelf shelf)
        {
            store.Shelves.Add(shelf);
            _addShelfEvent.Publish(new AddShelfEvent {Shelf = shelf, Store = store});
        }

        public void RemovePartFromShelf(Shelf shelf, Part part)
        {
            shelf.Parts.Remove(part);
        }

        public void AddPartToStoragePoint(StoragePoint storagePoint, Part part)
        {
            storagePoint.Parts.Add(part);
        }
    }
}