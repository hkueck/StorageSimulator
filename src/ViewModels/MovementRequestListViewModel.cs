using System.Collections.ObjectModel;
using Prism.Mvvm;
using StorageSimulator.Core.Model;

namespace StorageSimulator.ViewModels
{
    public class MovementRequestListViewModel: BindableBase
    {
        public ObservableCollection<MovementRequestViewModel> Requests { get; } = new ObservableCollection<MovementRequestViewModel>();

        public MovementRequestListViewModel()
        {
            var request = new MovementRequest{Info = "Transport von Teilen"};
            Requests.Add(new MovementRequestViewModel(request));
        }
        
    }
}