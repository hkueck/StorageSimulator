using System.Collections.ObjectModel;
using Prism.Events;
using Prism.Mvvm;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.ViewModels
{
    public class LogListViewModel: BindableBase, ILogListViewModel
    {
        public LogListViewModel(IEventAggregator eventAggregator, bool forTestOnly = false)
        {
            var exceptionEvent = eventAggregator.GetEvent<PubSubEvent<ExceptionEvent>>();
            if (forTestOnly)
            {
                exceptionEvent.Subscribe(OnExceptionReceived);
            }
            else
            {
                exceptionEvent.Subscribe(OnExceptionReceived, ThreadOption.UIThread);
            }
        }

        private void OnExceptionReceived(ExceptionEvent request)
        {
            var viewModel = new LogViewModel(request.Exception);
            Logs.Add(viewModel);
        }

        public ObservableCollection<LogViewModel> Logs { get; } = new ObservableCollection<LogViewModel>();
    }
}