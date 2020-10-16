using FluentAssertions;
using StorageSimulator.Core.Model;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class PartViewModelTest
    {
        [Fact]
        public void ConstructorShouldInitalizeMember()
        {
            var part = new Part {Position = 1, Barcode = "12345678"};

            var viewModel = new PartViewModel(part);

            viewModel.Position.Should().Be(1);
            viewModel.Barcode.Should().Be("12345678");
        }

        [Fact]
        public void SetPropertiesShouldSetModel()
        {
            var part = new Part();
            var viewModel = new PartViewModel(part);

            viewModel.Barcode = "12345678";
            viewModel.Position = 3;

            part.Barcode.Should().Be("12345678");
            part.Position.Should().Be(3);
        }

    }
}