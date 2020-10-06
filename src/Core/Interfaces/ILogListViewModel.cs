using System.Collections.ObjectModel;
using StorageSimulator.ViewModels;

namespace StorageSimulator.Core.Interfaces
{
    public interface ILogListViewModel
    {
        ObservableCollection<LogViewModel> Logs { get; }
    }
}