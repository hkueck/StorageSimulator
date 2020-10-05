using System;

namespace StorageSimulator.Core.Interfaces
{
    public interface IMovementRequestViewModel
    {
        string Name { get; set; }
        string Source { get; set; }
        string SourceShelf { get; set; }
        string Target { get; set; }
        string TargetShelf { get; set; }
        int Quantity { get; set; }
        DateTime Timestamp { get; set; }
        Guid Ticket { get; set; }
        string Type { get; set; }
        string Barcode { get; set; }
    }
}