using System.Linq;
using FluentAssertions;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Model;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class ShelfViewModelTest
    {
        private IEventAggregator _eventAggregator;

        public ShelfViewModelTest()
        {
            _eventAggregator = new EventAggregator();
        }

        [Fact]
        public void ConstructorShouldInitializeMember()
        {
            var shelf = new Shelf {Number = "1"};
            shelf.Parts.Add(new Part {Position = 1, Barcode = "barcode"});

            var viewModel = new ShelfViewModel(shelf, _eventAggregator);

            viewModel.Number.Should().Be("1");
            viewModel.Parts.Count.Should().Be(1);
        }

        [Fact]
        public void SetNumberShouldSetNumberToModel()
        {
            var shelf = new Shelf();
            var viewModel = new ShelfViewModel(shelf, _eventAggregator);

            viewModel.Number = "4";

            shelf.Number.Should().Be("4");
        }

        [Fact]
        public void ReceivingAddPartEventShouldAddPart()
        {
            var part = new Part {Barcode = "expected", Position = 1};
            var shelf = new Shelf();
            var viewModel = new ShelfViewModel(shelf, _eventAggregator);

            var addPartEvent = _eventAggregator.GetEvent<PubSubEvent<AddPartEvent>>();
            addPartEvent.Publish(new AddPartEvent {Shelf = shelf, Part = part});

            viewModel.Parts.Count.Should().Be(1);
            var partViewModel = viewModel.Parts.First();
            partViewModel.Barcode.Should().Be("expected");
            partViewModel.Position.Should().Be(1);
        }

        [Fact]
        public void ReceivingRemovePartEventShouldRemovePart()
        {
            var part = new Part {Barcode = "expected", Position = 1};
            var shelf = new Shelf();
            shelf.Parts.Add(part);
            var viewModel = new ShelfViewModel(shelf, _eventAggregator);

            var removePartEvent = _eventAggregator.GetEvent<PubSubEvent<RemovePartFromShelfEvent>>();
            removePartEvent.Publish(new RemovePartFromShelfEvent {Shelf = shelf, Part = part});

            viewModel.Parts.Count.Should().Be(0);
        }
        
        [Fact]
        public void ReceivingAddPartEventWithWrongShelfShouldNotAddPart()
        {
            var part = new Part {Barcode = "expected", Position = 1};
            var shelf = new Shelf();
            var viewModel = new ShelfViewModel(shelf, _eventAggregator);

            var addPartEvent = _eventAggregator.GetEvent<PubSubEvent<AddPartEvent>>();
            addPartEvent.Publish(new AddPartEvent {Shelf = new Shelf(), Part = part});

            viewModel.Parts.Count.Should().Be(0);
        }

        [Fact]
        public void ReceivingRemovePartEventWithWrongShelfShouldNotRemovePart()
        {
            var part = new Part {Barcode = "expected", Position = 1};
            var shelf = new Shelf();
            shelf.Parts.Add(part);
            var viewModel = new ShelfViewModel(shelf, _eventAggregator);

            var removePartEvent = _eventAggregator.GetEvent<PubSubEvent<RemovePartFromShelfEvent>>();
            removePartEvent.Publish(new RemovePartFromShelfEvent {Shelf = new Shelf(), Part = part});

            viewModel.Parts.Count.Should().Be(1);
        }
    }
}