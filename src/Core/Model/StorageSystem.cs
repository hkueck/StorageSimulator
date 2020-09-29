using Prism.Events;
using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.Core.Model
{
    public class StorageSystem : IStorageSystem
    {
        private readonly IWatchRequestUseCase _watchRequestUseCase;
        private readonly IEventAggregator _eventAggregator;

        public StorageSystem(IWatchRequestUseCase watchRequestUseCase, IEventAggregator eventAggregator)
        {
            _watchRequestUseCase = watchRequestUseCase;
            _eventAggregator = eventAggregator;
            _watchRequestUseCase.Execute();
        }
    }
}