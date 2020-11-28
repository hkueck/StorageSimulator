using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Interfaces
{
    public interface ISendUseCase
    {
        void Execute(MovementRequest request);
    }
}