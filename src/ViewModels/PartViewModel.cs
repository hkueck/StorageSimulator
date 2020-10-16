using Prism.Mvvm;
using StorageSimulator.Core.Model;

namespace StorageSimulator.ViewModels
{
    public class PartViewModel : BindableBase
    {
        private readonly Part _part;

        public PartViewModel(Part part)
        {
            _part = part;
        }

        public int Position
        {
            get { return _part.Position; }
            set
            {
                _part.Position = value;
                RaisePropertyChanged();
            }
        }

        public string Barcode
        {
            get { return _part.Barcode; }
            set
            {
                _part.Barcode = value;
                RaisePropertyChanged();
            }
        }
    }
}