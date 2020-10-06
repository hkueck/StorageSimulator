using System.IO;
using FluentAssertions;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class LogListViewModelTest
    {
        [Fact]
        public void ReceivingExceptionEventShouldAddErrorLog()
        {
            var eventAggregator = new EventAggregator();
            var exceptionEvent = eventAggregator.GetEvent<PubSubEvent<ExceptionEvent>>();
            var viewModel = new LogListViewModel(eventAggregator, true);
            var exception = new IOException("expected message");
            
            exceptionEvent.Publish(new ExceptionEvent{Exception = exception});

            var logs = viewModel.Logs;
            logs.Should().NotBeNullOrEmpty();
        }
    }
}