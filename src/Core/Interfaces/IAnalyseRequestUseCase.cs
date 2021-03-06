using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Interfaces
{
    public interface IAnalyseRequestUseCase
    {
        void Execute(MovementRequest request);
        IStorageSystem StorageSystem { get; set; }
    }
}