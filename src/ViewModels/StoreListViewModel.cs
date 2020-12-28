using System.Collections.ObjectModel;
using System.Threading;
using Prism.Events;
using Prism.Mvvm;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.ViewModels
{
    public class StoreListViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        public ObservableCollection<StoreViewModel> Stores { get; } = new ObservableCollection<StoreViewModel>();

        public StoreListViewModel()
        {
        }

        public StoreListViewModel(IStorageSystem storageSystem, IEventAggregator eventAggregator): this()
        {
            _eventAggregator = eventAggregator;
            foreach (var store in storageSystem.Stores)
            {
                Stores.Add(new StoreViewModel(store, _eventAggregator));
            }

            var pubSubEvent = eventAggregator.GetEvent<PubSubEvent<AddStoreEvent>>();
            if (Thread.CurrentThread.IsBackground)
                pubSubEvent.Subscribe(OnAddStore);
            else
                pubSubEvent.Subscribe(OnAddStore, ThreadOption.UIThread);
        }

        private void OnAddStore(AddStoreEvent addStoreEvent)
        {
            Stores.Add(new StoreViewModel(addStoreEvent.Store, _eventAggregator));
        }
    }
}