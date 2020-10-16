using FluentAssertions;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Model;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class MovementResponseListViewModelTest
    {
        [Fact]
        public void ReceivingResponseEventShouldAddResponse()
        {
            var eventAggregator = new EventAggregator();
            var response = new MovementResponse();
            var responseEvent = eventAggregator.GetEvent<PubSubEvent<MovementResponseEvent>>();
            var viewModel = new MovementResponseListViewModel(eventAggregator);
            
            responseEvent.Publish(new MovementResponseEvent{Response = response});

            viewModel.Responses.Should().HaveCount(1);
        }
    }
}