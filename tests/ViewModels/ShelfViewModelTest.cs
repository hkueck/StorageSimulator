using FluentAssertions;
using StorageSimulator.Core.Model;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class ShelfViewModelTest
    {
        [Fact]
        public void ConstructorShouldInitializeMember()
        {
            var shelf = new Shelf {Number = "1"};
            shelf.Parts.Add(new Part {Position = 1, Barcode = "barcode"});

            var viewModel = new ShelfViewModel(shelf);

            viewModel.Number.Should().Be("1");
            viewModel.Parts.Count.Should().Be(1);
        }

        [Fact]
        public void SetNumberShouldSetNumberToModel()
        {
            var shelf = new Shelf();
            var viewModel = new ShelfViewModel(shelf);

            viewModel.Number = "4";

            shelf.Number.Should().Be("4");
        }
    }
}