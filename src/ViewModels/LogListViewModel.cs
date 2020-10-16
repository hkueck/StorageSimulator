using System.Collections.ObjectModel;
using System.Threading;
using Prism.Events;
using Prism.Mvvm;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.ViewModels
{
    public class LogListViewModel: BindableBase, ILogListViewModel
    {
        public LogListViewModel(IEventAggregator eventAggregator)
        {
            var exceptionEvent = eventAggregator.GetEvent<PubSubEvent<ExceptionEvent>>();
            if (Thread.CurrentThread.IsBackground)
                exceptionEvent.Subscribe(OnExceptionReceived);
            else
                exceptionEvent.Subscribe(OnExceptionReceived, ThreadOption.UIThread);
        }

        private void OnExceptionReceived(ExceptionEvent request)
        {
            var viewModel = new LogViewModel(request.Exception);
            Logs.Add(viewModel);
        }

        public ObservableCollection<LogViewModel> Logs { get; } = new ObservableCollection<LogViewModel>();
    }
}