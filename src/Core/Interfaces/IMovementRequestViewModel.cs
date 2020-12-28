using Prism.Commands;

namespace StorageSimulator.Core.Interfaces
{
    public interface IMovementRequestViewModel : IMovementViewModel
    {
        string Type { get; set; }
        public DelegateCommand SendTransportSucceededCommand { get; }
        public DelegateCommand SendInsertSucceededCommand { get; }
        public DelegateCommand SendWrongSourceTargetCommand { get; }
        public DelegateCommand SendWrongPartCountCommand { get; }
        public DelegateCommand SendStoreOccupiedCommand { get; }
        public DelegateCommand SendStoragePointOccupiedCommand { get; }
        public DelegateCommand SendCountZeroCommand { get; }
        public DelegateCommand SendOrderExistCommand { get; }
    }
}