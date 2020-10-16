using System.Collections.ObjectModel;
using System.Threading;
using Prism.Events;
using Prism.Mvvm;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Model;

namespace StorageSimulator.ViewModels
{
    public class StoreViewModel: BindableBase
    {
        private readonly Store _store;
        private readonly IEventAggregator _eventAggregator;
        private PubSubEvent<AddShelfEvent> _addShelfEvent;

        public ObservableCollection<ShelfViewModel> Shelves { get; } = new ObservableCollection<ShelfViewModel>();

        public string Name
        {
            get => _store.Name;
            set
            {
                _store.Name = value; 
                RaisePropertyChanged();
            }
        }

        public StoreViewModel(Store store, IEventAggregator eventAggregator)
        {
            _store = store;
            _eventAggregator = eventAggregator;
            _addShelfEvent = _eventAggregator.GetEvent<PubSubEvent<AddShelfEvent>>();
            if (Thread.CurrentThread.IsBackground)
            {
                _addShelfEvent.Subscribe(OnAddShelf);
            }
            else
            {
                _addShelfEvent.Subscribe(OnAddShelf, ThreadOption.UIThread);
            }
            foreach (var shelf in store.Shelves)
            {
                Shelves.Add(new ShelfViewModel(shelf));
            }
        }

        private void OnAddShelf(AddShelfEvent addShelfEvent)
        {
            if (_store.Name == addShelfEvent.Store.Name)
            {
                var shelfViewModel = new ShelfViewModel(addShelfEvent.Shelf);
                Shelves.Add(shelfViewModel);
            }
        }
    }
}