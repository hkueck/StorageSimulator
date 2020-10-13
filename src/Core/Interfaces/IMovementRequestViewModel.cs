namespace StorageSimulator.Core.Interfaces
{
    public interface IMovementRequestViewModel : IMovementViewModel
    {
        string Type { get; set; }
    }
}