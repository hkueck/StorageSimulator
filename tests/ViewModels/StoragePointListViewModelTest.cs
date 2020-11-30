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
    public class StoragePointListViewModelTest
    {
        private EventAggregator _eventAggregator;
        private Mock<IStorageSystem> _storageSystem;

        public StoragePointListViewModelTest()
        {
            _eventAggregator = new EventAggregator();
            _storageSystem = new Mock<IStorageSystem>();
        }   
        
        [Fact]
        public void ConstructorShouldInitializeMember()
        {
            var storagePoints = new List<StoragePoint>();
            storagePoints.Add(new StoragePoint{Name = "storePoint"});
            _storageSystem.Setup(s => s.StoragePoints).Returns(storagePoints);

            var viewModel = new StoragePointListViewModel(_storageSystem.Object, _eventAggregator);

            viewModel.StoragePoints.Count.Should().Be(1);
            var storagePointViewModel = viewModel.StoragePoints.First();
            storagePointViewModel.Name.Should().Be("storePoint");
        }

        [Fact]
        public void ReceiveAddStoreEventShouldAddStore()
        {
            var storagePoint = new StoragePoint {Name = "expected"};
            var storeEvent = _eventAggregator.GetEvent<PubSubEvent<AddStoragePointEvent>>();
            _storageSystem.Setup(s => s.StoragePoints).Returns(new List<StoragePoint>());
            var viewModel = new StoragePointListViewModel(_storageSystem.Object, _eventAggregator);

            storeEvent.Publish(new AddStoragePointEvent {StoragePoint = storagePoint});

            Task.Delay(5).Wait();
            viewModel.StoragePoints.Count.Should().Be(1);
            var storeViewModel = viewModel.StoragePoints.First();
            storeViewModel.Name.Should().Be("expected");
        }
    }
}