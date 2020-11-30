using System.Collections.ObjectModel;
using System.Threading;
using ImTools;
using Prism.Events;
using Prism.Mvvm;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Model;

namespace StorageSimulator.ViewModels
{
    public class StoragePointViewModel: BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private string _name;
        private PubSubEvent<InsertPartEvent> _insertPartEvent;
        private PubSubEvent<RemovePartFromStoragePointEvent> _removePartEvent;
        private StoragePoint _storagePoint;

        public ObservableCollection<PartViewModel> Parts { get; } = new ObservableCollection<PartViewModel>();

        public StoragePointViewModel(StoragePoint storagePoint, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Name = storagePoint.Name;
            _storagePoint = storagePoint;
            _insertPartEvent = _eventAggregator.GetEvent<PubSubEvent<InsertPartEvent>>();
            _removePartEvent = _eventAggregator.GetEvent<PubSubEvent<RemovePartFromStoragePointEvent>>();
            if (Thread.CurrentThread.IsBackground)
            {
                _insertPartEvent.Subscribe(OnInsertPart);
                _removePartEvent.Subscribe(OnRemovePart);
            }
            else
            {
                _insertPartEvent.Subscribe(OnInsertPart, ThreadOption.UIThread);
                _removePartEvent.Subscribe(OnRemovePart, ThreadOption.UIThread);
            }

            foreach (var part in storagePoint.Parts)
            {
                Parts.Add(new PartViewModel(part));
            }
        }

        private void OnRemovePart(RemovePartFromStoragePointEvent obj)
        {
            if (obj.StoragePoint != _storagePoint) return;
            var partViewModel = Parts.FindFirst(vm => vm.Barcode == obj.Part.Barcode);
            if (partViewModel != null) Parts.Remove(partViewModel);
        }

        private void OnInsertPart(InsertPartEvent insertPartEvent)
        {
            if (insertPartEvent.StoragePoint == _name)
            {
                Parts.Add(new PartViewModel(insertPartEvent.Part));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
    }
}