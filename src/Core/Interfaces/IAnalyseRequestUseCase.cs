using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Interfaces
{
    public interface IAnalyseRequestUseCase
    {
        MovementResponse Execute(MovementRequest request);
        IStorageSystem StorageSystem { get; set; }
    }
}