using System.Collections.ObjectModel;
using System.Threading;
using Prism.Events;
using Prism.Mvvm;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.ViewModels
{
    public class StoragePointListViewModel: BindableBase
    {
        private readonly IStorageSystem _storageSystemObject;
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<StoragePointViewModel> StoragePoints { get; } = new ObservableCollection<StoragePointViewModel>();

        public StoragePointListViewModel(IStorageSystem storageSystemObject, IEventAggregator eventAggregator)
        {
            _storageSystemObject = storageSystemObject;
            _eventAggregator = eventAggregator;
            var addEvent = _eventAggregator.GetEvent<PubSubEvent<AddStoragePointEvent>>();
            if (Thread.CurrentThread.IsBackground)
                addEvent.Subscribe(OnAddStoragePoint);
            else
                addEvent.Subscribe(OnAddStoragePoint, ThreadOption.UIThread);

            foreach (var storagePoint in _storageSystemObject.StoragePoints)
            {
                var storagePointViewModel = new StoragePointViewModel(storagePoint, _eventAggregator);
                StoragePoints.Add(storagePointViewModel);
            }
        }

        private void OnAddStoragePoint(AddStoragePointEvent storagePointEvent)
        {
            StoragePoints.Add(new StoragePointViewModel(storagePointEvent.StoragePoint, _eventAggregator));
        }
    }
}