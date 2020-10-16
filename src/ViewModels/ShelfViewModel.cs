using System.Collections.ObjectModel;
using Prism.Mvvm;
using StorageSimulator.Core.Model;

namespace StorageSimulator.ViewModels
{
    public class ShelfViewModel : BindableBase
    {
        private readonly Shelf _shelf;

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

        public ShelfViewModel(Shelf shelf)
        {
            _shelf = shelf;
            foreach (var part in shelf.Parts)
            {
                Parts.Add(new PartViewModel(part));
            }
        }
    }
}