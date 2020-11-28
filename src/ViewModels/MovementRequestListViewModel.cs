using System.Collections.ObjectModel;
using System.Threading;
using Prism.Events;
using Prism.Mvvm;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.ViewModels
{
    public class MovementRequestListViewModel : BindableBase, IMovementRequestListViewModel
    {
        private IMovementRequestViewModel _selectedItem;
        public ObservableCollection<IMovementRequestViewModel> Requests { get; } = new ObservableCollection<IMovementRequestViewModel>();

        public IMovementRequestViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                RaisePropertyChanged();
            }
        }

        public MovementRequestListViewModel(IEventAggregator eventAggregator)
        {
            var requestEvent = eventAggregator.GetEvent<PubSubEvent<MovementRequestEvent>>();
            if (!Thread.CurrentThread.IsBackground)
                requestEvent.Subscribe(OnReceiveRequest, ThreadOption.UIThread);
            else // for test only
                requestEvent.Subscribe(OnReceiveRequest);
        }

        private void OnReceiveRequest(MovementRequestEvent request)
        {
            var viewModel = new MovementRequestViewModel(request.MovementRequest);
            Requests.Insert(0, viewModel);
            SelectedItem = viewModel;
        }
    }
}