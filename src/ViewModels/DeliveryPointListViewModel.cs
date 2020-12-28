using System.Collections.ObjectModel;
using System.Threading;
using Prism.Events;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.ViewModels
{
    public class DeliveryPointListViewModel
    {
        private readonly IStorageSystem _storageSystem;
        private readonly IEventAggregator _eventAggregator;
        public ObservableCollection<StoragePointViewModel> DeliveryPoints { get; } = new ObservableCollection<StoragePointViewModel>();

        public DeliveryPointListViewModel()
        {
        }

        public DeliveryPointListViewModel(IStorageSystem storageSystem, IEventAggregator eventAggregator): this()
        {
            _storageSystem = storageSystem;
            _eventAggregator = eventAggregator;
            var addEvent = _eventAggregator.GetEvent<PubSubEvent<AddDeliveryPointEvent>>();
            if (Thread.CurrentThread.IsBackground)
                addEvent.Subscribe(OnAddDeliveryPoint);
            else
                addEvent.Subscribe(OnAddDeliveryPoint, ThreadOption.UIThread);

            foreach (var storagePoint in _storageSystem.DeliveryPoints)
            {
                var storagePointViewModel = new StoragePointViewModel(storagePoint, _eventAggregator);
                DeliveryPoints.Add(storagePointViewModel);
            }
        }

        private void OnAddDeliveryPoint(AddDeliveryPointEvent addDeliveryPointEvent)
        {
            DeliveryPoints.Insert(0, new StoragePointViewModel(addDeliveryPointEvent.DeliveryPoint, _eventAggregator));
        }
    }
}