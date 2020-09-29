using FluentAssertions;
using Moq;
using Prism.Events;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using Xunit;

namespace StorageSimulatorTests.Model
{
    public class StorageSystemTest
    {
        [Fact]
        public void ConstructorShouldCallRequestUseCase()
        {
            var useCase = new Mock<IWatchRequestUseCase>();

            var storageSystem = new StorageSystem(useCase.Object, new EventAggregator());

            storageSystem.Should().NotBeNull();
            useCase.Verify(uc => uc.Execute());
        }
    }
}
