using System.Collections.Generic;
using System.IO;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.Core.Model
{
    public class StorageSystem : IStorageSystem
    {
        private readonly IWatchRequestUseCase _watchRequestUseCase;
        private readonly ISendResponseUseCase _sendUseCase;
        private readonly IAnalyseRequestUseCase _analyseRequestUseCase;
        private readonly IEventAggregator _eventAggregator;
        private PubSubEvent<AddStoreEvent> _addStoreEvent;
        private PubSubEvent<AddShelfEvent> _addShelfEvent;

        public IList<Store> Stores { get; } = new List<Store>();
        public IList<StoragePoint> StoragePoints { get; } = new List<StoragePoint>();

        public StorageSystem(IWatchRequestUseCase watchRequestUseCase, ISendResponseUseCase sendUseCase, IAnalyseRequestUseCase analyseRequestUseCase,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            var requestEvent = _eventAggregator.GetEvent<PubSubEvent<MovementRequestEvent>>();
            requestEvent.Subscribe(OnMovementRequest);
            _addStoreEvent = _eventAggregator.GetEvent<PubSubEvent<AddStoreEvent>>();
            _addShelfEvent = _eventAggregator.GetEvent<PubSubEvent<AddShelfEvent>>();
            _watchRequestUseCase = watchRequestUseCase;
            _sendUseCase = sendUseCase;
            _analyseRequestUseCase = analyseRequestUseCase;
            _analyseRequestUseCase.StorageSystem = this;
            _watchRequestUseCase.Execute();
        }

        private void OnMovementRequest(MovementRequestEvent movementRequestEvent)
        {
            var movement = movementRequestEvent.MovementRequest;
            var response = _analyseRequestUseCase.Execute(movement);
            try
            {
                _sendUseCase.Execute(response);
                var responseEvent = _eventAggregator.GetEvent<PubSubEvent<MovementResponseEvent>>();
                responseEvent.Publish(new MovementResponseEvent {Response = response});
            }
            catch (IOException exception)
            {
                var exceptionEvent = _eventAggregator.GetEvent<PubSubEvent<ExceptionEvent>>();
                exceptionEvent.Publish(new ExceptionEvent {Exception = exception});
            }
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