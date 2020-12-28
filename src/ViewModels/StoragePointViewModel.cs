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
        private PubSubEvent<InsertPartToDeliveryEvent> _insertPartToDeliveryEvent;

        public ObservableCollection<PartViewModel> Parts { get; } = new ObservableCollection<PartViewModel>();

        public StoragePointViewModel()
        {
        }

        public StoragePointViewModel(StoragePoint storagePoint, IEventAggregator eventAggregator): this()
        {
            _eventAggregator = eventAggregator;
            Name = storagePoint.Name;
            _storagePoint = storagePoint;
            _insertPartEvent = _eventAggregator.GetEvent<PubSubEvent<InsertPartEvent>>();
            _insertPartToDeliveryEvent = _eventAggregator.GetEvent<PubSubEvent<InsertPartToDeliveryEvent>>();
            _removePartEvent = _eventAggregator.GetEvent<PubSubEvent<RemovePartFromStoragePointEvent>>();
            if (Thread.CurrentThread.IsBackground)
            {
                _insertPartEvent.Subscribe(OnInsertPart);
                _removePartEvent.Subscribe(OnRemovePart);
                _insertPartToDeliveryEvent.Subscribe(OnInsertPartToDelivery);
            }
            else
            {
                _insertPartEvent.Subscribe(OnInsertPart, ThreadOption.UIThread);
                _removePartEvent.Subscribe(OnRemovePart, ThreadOption.UIThread);
                _insertPartToDeliveryEvent.Subscribe(OnInsertPartToDelivery, ThreadOption.UIThread);
            }

            foreach (var part in storagePoint.Parts)
            {
                Parts.Add(new PartViewModel(part));
            }
        }

        private void OnInsertPartToDelivery(InsertPartToDeliveryEvent partToDeliveryEvent)
        {
            if (partToDeliveryEvent.DeliveryPoint.Name == _name)
                Parts.Add(new PartViewModel(partToDeliveryEvent.Part));
        }

        private void OnRemovePart(RemovePartFromStoragePointEvent removePartEvent)
        {
            if (removePartEvent.StoragePoint != _storagePoint) return;
            var partViewModel = Parts.FindFirst(vm => vm.Barcode == removePartEvent.Part.Barcode);
            if (partViewModel != null) Parts.Remove(partViewModel);
        }

        private void OnInsertPart(InsertPartEvent insertPartEvent)
        {
            if (insertPartEvent.StoragePoint == _name)
                Parts.Add(new PartViewModel(insertPartEvent.Part));
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