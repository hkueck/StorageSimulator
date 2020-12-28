using System.Collections.ObjectModel;
using System.Threading;
using Prism.Events;
using Prism.Mvvm;
using StorageSimulator.Core.Events;

namespace StorageSimulator.ViewModels
{
    public class MovementResponseListViewModel: BindableBase
    {
        public ObservableCollection<MovementResponseViewModel> Responses { get; } = new ObservableCollection<MovementResponseViewModel>();

        public MovementResponseListViewModel()
        {
        }

        public MovementResponseListViewModel(IEventAggregator eventAggregator): this()
        {
            var responseEvent = eventAggregator.GetEvent<PubSubEvent<MovementResponseEvent>>();
            if (Thread.CurrentThread.IsBackground)
                responseEvent.Subscribe(OnReceiveResponse);
            else
                responseEvent.Subscribe(OnReceiveResponse, ThreadOption.UIThread);
        }

        private void OnReceiveResponse(MovementResponseEvent response)
        {
            var viewModel = new MovementResponseViewModel(response.Response);
            Responses.Add(viewModel);
        }
    }
}