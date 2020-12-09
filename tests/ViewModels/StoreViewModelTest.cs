using System.Threading.Tasks;
using FluentAssertions;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Model;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class StoreViewModelTest
    {
        private EventAggregator _eventAggregator;

        public StoreViewModelTest()
        {
            _eventAggregator = new EventAggregator();
        }

        [Fact]
        public void ConstructorShouldInitializeMember()
        {
            var store = new Store {Name = "store1"};
            store.Shelves.Add(new Shelf {Number = "1"});

            var viewModel = new StoreViewModel(store, _eventAggregator);

            viewModel.Name.Should().Be("store1");
            viewModel.Shelves.Count.Should().Be(1);
        }

        [Fact]
        public void SetPropertiesShouldSetModel()
        {
            var store = new Store();
            var viewModel = new StoreViewModel(store, _eventAggregator);

            viewModel.Name = "store";

            store.Name.Should().Be("store");
        }

        [Fact]
        public void ReceivingAddShelfEventShouldAddShelf()
        {
            var shelf = new Shelf{Number = "expected"};
            var store = new Store {Name = "store1"};
            store.Shelves.Add(new Shelf {Number = "1"});
            var viewModel = new StoreViewModel(store, _eventAggregator);

            var addShelfEvent = _eventAggregator.GetEvent<PubSubEvent<AddShelfEvent>>();
            addShelfEvent.Publish(new AddShelfEvent{Shelf = shelf, Store = store});

            Task.Delay(5).Wait();
            viewModel.Shelves.Count.Should().Be(2);
            viewModel.Shelves[1].Number.Should().Be("expected");
        }
    }
}
