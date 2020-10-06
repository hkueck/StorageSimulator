using System.IO;
using FluentAssertions;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class LogViewModelTest
    {
        [Fact]
        public void ConstructorWithExceptionShouldCreateExceptionLog()
        {
            var exception = new IOException("expected message");

            var viewModel = new LogViewModel(exception);

            viewModel.Type.Should().Be("Exception");
            viewModel.Message.Should().Be("expected message");
        }

    }
}