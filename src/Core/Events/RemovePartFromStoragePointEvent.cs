using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Events
{
    public class RemovePartFromStoragePointEvent
    {
        public StoragePoint StoragePoint { get; set; }
        public Part Part { get; set; }
    }
}