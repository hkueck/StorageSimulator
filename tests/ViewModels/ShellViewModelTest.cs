using FluentAssertions;
using Moq;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class ShellViewModelTest
    {
        [Fact]
        public void ConstructorShouldStartWatchStorageUseCase()
        {
            var storageSystem = new Mock<IStorageSystem>();

            var viewModel = new ShellViewModel(storageSystem.Object);

            viewModel.Should().NotBeNull();
        }
    }
}