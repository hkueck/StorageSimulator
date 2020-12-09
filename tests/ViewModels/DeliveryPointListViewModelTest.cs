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
    public class DeliveryPointListViewModelTest
    {
        private EventAggregator _eventAggregator;
        private Mock<IStorageSystem> _storageSystem;

        public DeliveryPointListViewModelTest()
        {
            _eventAggregator = new EventAggregator();
            _storageSystem = new Mock<IStorageSystem>();
        }
        
        [Fact]
        public void ConstructorShouldInitializeMember()
        {
            var storagePoints = new List<StoragePoint>();
            storagePoints.Add(new StoragePoint{Name = "deliveryPoint"});
            _storageSystem.Setup(s => s.DeliveryPoints).Returns(storagePoints);

            var viewModel = new DeliveryPointListViewModel(_storageSystem.Object, _eventAggregator);

            viewModel.DeliveryPoints.Count.Should().Be(1);
            var storagePointViewModel = viewModel.DeliveryPoints.First();
            storagePointViewModel.Name.Should().Be("deliveryPoint");
        }

        [Fact]
        public void ReceiveAddSDeliveryEventShouldAddStore()
        {
            var storagePoint = new StoragePoint {Name = "expected"};
            var deliveryPointEvent = _eventAggregator.GetEvent<PubSubEvent<AddDeliveryPointEvent>>();
            _storageSystem.Setup(s => s.DeliveryPoints).Returns(new List<StoragePoint>());
            var viewModel = new DeliveryPointListViewModel(_storageSystem.Object, _eventAggregator);

            deliveryPointEvent.Publish(new AddDeliveryPointEvent {DeliveryPoint = storagePoint});

            Task.Delay(5).Wait();
            viewModel.DeliveryPoints.Count.Should().Be(1);
            var storeViewModel = viewModel.DeliveryPoints.First();
            storeViewModel.Name.Should().Be("expected");
        }
    }
}
