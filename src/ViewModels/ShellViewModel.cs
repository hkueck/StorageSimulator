using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.ViewModels
{
    public class ShellViewModel: Prism.Mvvm.BindableBase
    {
        private readonly IStorageSystem _storageSystem;

        public ShellViewModel(IStorageSystem storageSystem)
        {
            _storageSystem = storageSystem;
        }
    }
}