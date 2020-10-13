namespace StorageSimulator.Core.Interfaces
{
    public interface IMovementResponseViewModel: IMovementViewModel
    {
        string Status { get; set; }
    }
}