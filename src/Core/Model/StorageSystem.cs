using System;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Types;

namespace StorageSimulator.Core.Model
{
    public class StorageSystem : IStorageSystem
    {
        private readonly IWatchRequestUseCase _watchRequestUseCase;
        private readonly ISendResponseUseCase _sendUseCase;
        private readonly IEventAggregator _eventAggregator;

        public StorageSystem(IWatchRequestUseCase watchRequestUseCase, ISendResponseUseCase sendUseCase, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            var requestEvent = _eventAggregator.GetEvent<PubSubEvent<MovementRequest>>();
            requestEvent.Subscribe(OnMovementRequest);
            _watchRequestUseCase = watchRequestUseCase;
            _sendUseCase = sendUseCase;
            _watchRequestUseCase.Execute();
        }

        private void OnMovementRequest(MovementRequest request)
        {
            var movement = request.Request;
            var response = new Movement
            {
                Info = movement.Info, Quantity = movement.Quantity, Source = movement.Source, Status = AutomationStatus.InsertionSucceeded,
                Target = movement.Target, Task = AutomationTasks.Insert, Ticket = Guid.NewGuid(), Timestamp = DateTime.UtcNow,
                SourceCompartment = movement.SourceCompartment, TargetCompartment = movement.TargetCompartment
            };
            _sendUseCase.Execute(response);
        }
    }
}