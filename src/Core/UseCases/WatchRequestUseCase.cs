using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.Core.UseCases
{
    public class WatchRequestUseCase: IWatchRequestUseCase
    {
        private readonly IWatchRequestService _service;

        public WatchRequestUseCase(IWatchRequestService service)
        {
            _service = service;
        }

        public void Execute()
        {
            _service.Run();
        }
    }
}