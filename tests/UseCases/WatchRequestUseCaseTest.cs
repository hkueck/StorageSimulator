using Moq;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.UseCases;
using Xunit;

namespace StorageSimulatorTests.UseCases
{
    public class WatchRequestUseCaseTest
    {
        [Fact]
        public void ExecuteShouldStartWatchService()
        {
            var service = new Mock<IWatchRequestService>();
            var useCase = new WatchRequestUseCase(service.Object);
            
            useCase.Execute();
            
            service.Verify(s => s.Run());
        }
    }
}