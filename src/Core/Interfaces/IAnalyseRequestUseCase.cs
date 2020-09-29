using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Interfaces
{
    public interface IAnalyseRequestUseCase
    {
        MovementResponse Analyse(MovementRequest request);
        IStorageSystem StorageSystem { get; set; }
    }
}