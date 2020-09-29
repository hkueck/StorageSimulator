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
        private readonly IEventAggregator _eventAggregator;

        public IList<Store> Stores { get; set; }
        public IList<StoragePoint> StoragePoints { get; set; }
        
        public StorageSystem(IWatchRequestUseCase watchRequestUseCase, ISendResponseUseCase sendUseCase, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            var requestEvent = _eventAggregator.GetEvent<PubSubEvent<Events.MovementRequest>>();
            requestEvent.Subscribe(OnMovementRequest);
            _watchRequestUseCase = watchRequestUseCase;
            _sendUseCase = sendUseCase;
            _watchRequestUseCase.Execute();
        }

        private void OnMovementRequest(Events.MovementRequest request)
        {
            var movement = request.Request;
            var response = new MovementResponse()
            {
                Info = movement.Info, Quantity = movement.Quantity, Source = movement.Source, Status = AutomationStatus.InsertionSucceeded,
                Target = movement.Target, Ticket = movement.Ticket, Timestamp = DateTime.UtcNow,
                SourceCompartment = movement.SourceCompartment, TargetCompartment = movement.TargetCompartment
            };
            _sendUseCase.Execute(response);
        }
    }
}
