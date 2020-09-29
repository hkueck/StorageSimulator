using System;
using System.Collections.Generic;
using Prism.Events;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Types;

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
            var movement = movementRequestEvent.Request;
            var response = _analyseRequestUseCase.Analyse(movement);
            _sendUseCase.Execute(response);
        }

        public void AddStoragePoint(StoragePoint storagePoint)
        {
            StoragePoints.Add(storagePoint);
        }
    }
}
