using System.Collections.ObjectModel;
using StorageSimulator.ViewModels;

namespace StorageSimulator.Core.Interfaces
{
    public interface IMovementRequestListViewModel
    {
        ObservableCollection<IMovementRequestViewModel> Requests { get; }
    }
}