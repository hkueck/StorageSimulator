using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Events
{
    public class RemovePartEvent
    {
        public Shelf Shelf { get; set; }
        public Part Part { get; set; }
    }
}