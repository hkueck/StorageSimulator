using System.Collections.ObjectModel;
using Prism.Events;
using Prism.Mvvm;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.ViewModels
{
    public class MovementRequestListViewModel: BindableBase, IMovementRequestListViewModel
    {
        public ObservableCollection<IMovementRequestViewModel> Requests { get; } = new ObservableCollection<IMovementRequestViewModel>();

        public MovementRequestListViewModel(IEventAggregator eventAggregator)
        {
            var requestEvent = eventAggregator.GetEvent<PubSubEvent<MovementRequestEvent>>();
            requestEvent.Subscribe(OnReceiveRequest);
        }

        private void OnReceiveRequest(MovementRequestEvent request)
        {
            var viewModel = new MovementRequestViewModel(request.MovementRequest);
            Requests.Add(viewModel);
        }
    }
}