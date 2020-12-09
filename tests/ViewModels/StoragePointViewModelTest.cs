using FluentAssertions;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Model;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class StoragePointViewModelTest
    {
        private IEventAggregator _eventAggregator;

        public StoragePointViewModelTest()
        {
            _eventAggregator = new EventAggregator();
        }

        [Fact]
        public void ConstructorShouldInitializeMember()
        {
            var storagePoint = new StoragePoint{Name = "expected"};
            for (int i = 0; i < 3; i++)
            {
                storagePoint.Parts.Add(new Part{Barcode = $"{i}", Position = i});
            }

            var viewModel = new StoragePointViewModel(storagePoint, _eventAggregator);

            viewModel.Name.Should().Be("expected");
            viewModel.Parts.Count.Should().Be(3);
            for (int i = 0; i < 3; i++)
            {
                viewModel.Parts[i].Barcode.Should().Be(i.ToString());
                viewModel.Parts[i].Position.Should().Be(i);
            }
        }

        [Fact]
        public void ReceivingInsertPartShouldAddPart()
        {
            var insertEvent = _eventAggregator.GetEvent<PubSubEvent<InsertPartEvent>>();
            var part = new Part{Barcode = "expected"};
            var viewModel = new StoragePointViewModel(new StoragePoint{Name = "storagePoint"}, _eventAggregator);
            
            insertEvent.Publish(new InsertPartEvent{StoragePoint = "storagePoint", Part = part});

            viewModel.Parts.Count.Should().Be(1);
            viewModel.Parts[0].Barcode.Should().Be("expected");
        }

        [Fact]
        public void ReceivingInsertPartToDeliveryShouldAddPart()
        {
            var insertEvent = _eventAggregator.GetEvent<PubSubEvent<InsertPartToDeliveryEvent>>();
            var part = new Part{Barcode = "expected"};
            var deliveryPoint = new StoragePoint{Name = "deliveryPoint"};
            var viewModel = new StoragePointViewModel(deliveryPoint, _eventAggregator);
            
            insertEvent.Publish(new InsertPartToDeliveryEvent{DeliveryPoint = deliveryPoint, Part = part});

            viewModel.Parts.Count.Should().Be(1);
            viewModel.Parts[0].Barcode.Should().Be("expected");
        }

        [Fact]
        public void ReceivingRemovePartShouldRemovePart()
        {
            var removeEvent = _eventAggregator.GetEvent<PubSubEvent<RemovePartFromStoragePointEvent>>();
            var part = new Part{Barcode = "expected"};
            var storagePoint = new StoragePoint{Name = "storagePoint"};
            storagePoint.Parts.Add(part);
            var viewModel = new StoragePointViewModel(storagePoint, _eventAggregator);
            
            removeEvent.Publish(new RemovePartFromStoragePointEvent{Part = part, StoragePoint = storagePoint});

            viewModel.Parts.Count.Should().Be(0);
        }
    }
}