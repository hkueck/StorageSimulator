using System;
using System.Collections.Generic;
using System.IO;
using Prism.Events;
using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.Core.Model
{
    public class StorageSystem : IStorageSystem
    {
        private readonly IWatchRequestUseCase _watchRequestUseCase;
        private readonly ISendResponseUseCase _sendUseCase;
        private readonly IAnalyseRequestUseCase _analyseRequestUseCase;
        private readonly IEventAggregator _eventAggregator;

        public IList<Store> Stores { get; set; }
        public IList<StoragePoint> StoragePoints { get; set; } = new List<StoragePoint>();
        
        public StorageSystem(IWatchRequestUseCase watchRequestUseCase, ISendResponseUseCase sendUseCase, IAnalyseRequestUseCase analyseRequestUseCase,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            var requestEvent = _eventAggregator.GetEvent<PubSubEvent<Events.MovementRequestEvent>>();
            requestEvent.Subscribe(OnMovementRequest);
            _watchRequestUseCase = watchRequestUseCase;
            _sendUseCase = sendUseCase;
            _analyseRequestUseCase = analyseRequestUseCase;
            _watchRequestUseCase.Execute();
        }

        private void OnMovementRequest(Events.MovementRequestEvent movementRequestEvent)
        {
            var movement = movementRequestEvent.MovementRequest;
            var response = _analyseRequestUseCase.Execute(movement);
            try
            {
                _sendUseCase.Execute(response);
            }
            catch (IOException exception)
            {
                //todo Fehlerbehandlung
                Console.WriteLine(exception);
            }
        }

        public void AddStoragePoint(StoragePoint storagePoint)
        {
            StoragePoints.Add(storagePoint);
        }

        public void AddStore(Store store)
        {
            Stores.Add(store);
        }

        public void AddPartToShelf(Shelf shelf, Part part)
        {
            throw new System.NotImplementedException();
        }

        public void AddShelfToStore(Store store, Shelf shelf)
        {
            throw new System.NotImplementedException();
        }

        public void RemovePartFromShelf(Shelf shelf, Part part)
        {
            throw new System.NotImplementedException();
        }

        public void AddPartToStoragePoint(StoragePoint storagePoint, Part part)
        {
            throw new System.NotImplementedException();
        }
    }
}
