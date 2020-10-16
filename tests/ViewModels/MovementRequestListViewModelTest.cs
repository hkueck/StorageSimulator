using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Model;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class MovementRequestListViewModelTest
    {
        [Fact]
        public void ReceivingRequestMessageShouldAddRequest()
        {
            var eventAggregator = new EventAggregator();
            var viewModel = new MovementRequestListViewModel(eventAggregator);
            var requestEvent = eventAggregator.GetEvent<PubSubEvent<MovementRequestEvent>>();
            var request = new MovementRequest {Ticket = Guid.NewGuid()};
            var movementRequestEvent = new MovementRequestEvent {MovementRequest = request};

            requestEvent.Publish(movementRequestEvent);

            viewModel.Requests.Count.Should().Be(1);
            var requestViewModel = viewModel.Requests.First();
            requestViewModel.Ticket.Should().Be(request.Ticket);
        }
    }
}