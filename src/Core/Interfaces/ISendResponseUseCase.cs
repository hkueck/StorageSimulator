using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Interfaces
{
    public interface ISendResponseUseCase
    {
        void Execute(MovementResponse movementResponse);
    }
}