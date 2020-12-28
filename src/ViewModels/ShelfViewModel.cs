using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Prism.Events;
using Prism.Mvvm;
using StorageSimulator.Core.Events;
using StorageSimulator.Core.Model;

namespace StorageSimulator.ViewModels
{
    public class ShelfViewModel : BindableBase
    {
        private readonly Shelf _shelf;
        private PubSubEvent<AddPartEvent> _addPartEvent;
        private PubSubEvent<RemovePartFromShelfEvent> _removePartEvent;

        public string Number
        {
            get { return _shelf.Number; }
            set
            {
                _shelf.Number = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<PartViewModel> Parts { get; } = new ObservableCollection<PartViewModel>();

        public ShelfViewModel()
        {
        }

        public ShelfViewModel(Shelf shelf, IEventAggregator eventAggregator):this()
        {
            _shelf = shelf;
            foreach (var part in shelf.Parts)
            {
                Parts.Add(new PartViewModel(part));
            }

            _addPartEvent = eventAggregator.GetEvent<PubSubEvent<AddPartEvent>>();
            _removePartEvent = eventAggregator.GetEvent<PubSubEvent<RemovePartFromShelfEvent>>();
            if (Thread.CurrentThread.IsBackground)
            {
                _addPartEvent.Subscribe(OnPartAdded);
                _removePartEvent.Subscribe(OnPartRemoved);
            }
            else
            {
                _addPartEvent.Subscribe(OnPartAdded, ThreadOption.UIThread);
                _removePartEvent.Subscribe(OnPartRemoved, ThreadOption.UIThread);
            }
        }

        private void OnPartRemoved(RemovePartFromShelfEvent removePartFromShelfEvent)
        {
            if (removePartFromShelfEvent.Shelf != _shelf) return;
            var partViewModel = Parts.FirstOrDefault(p => p.Position == removePartFromShelfEvent.Part.Position);
            if (partViewModel != null) Parts.Remove(partViewModel);
        }

        private void OnPartAdded(AddPartEvent addPartEvent)
        {
            if (addPartEvent.Shelf != _shelf) return;
            Parts.Insert(0, new PartViewModel(addPartEvent.Part));
        }
    }
}