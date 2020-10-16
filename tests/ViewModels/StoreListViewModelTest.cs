using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class StoreListViewModelTest
    {
        private IEventAggregator _eventAggregator;
        private Mock<IStorageSystem> _storageSystem;

        public StoreListViewModelTest()
        {
            _eventAggregator = new EventAggregator();
            _storageSystem = new Mock<IStorageSystem>();
        }

        [Fact]
        public void ConstructorShouldInitializeMember()
        {
            var stores = new List<Store>();
            stores.Add(new Store{Name = "store"});
            _storageSystem.Setup(s => s.Stores).Returns(stores);

            var viewModel = new StoreListViewModel(_storageSystem.Object, _eventAggregator);

            viewModel.Stores.Count.Should().Be(1);
            var storeViewModel = viewModel.Stores.First();
            storeViewModel.Name.Should().Be("store");
        }

        [Fact]
        public void ReceiveAddStoreEventShouldAddStore()
        {
            var store = new Store {Name = "expected"};
            var storeEvent = _eventAggregator.GetEvent<PubSubEvent<AddStoreEvent>>();
            _storageSystem.Setup(s => s.Stores).Returns(new List<Store>());
            var viewModel = new StoreListViewModel(_storageSystem.Object, _eventAggregator);

            storeEvent.Publish(new AddStoreEvent {Store = store});

            Task.Delay(5).Wait();
            viewModel.Stores.Count.Should().Be(1);
            var storeViewModel = viewModel.Stores.First();
            storeViewModel.Name.Should().Be("expected");
        }
    }
}