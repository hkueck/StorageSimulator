using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Events
{
    public class RemovePartFromShelfEvent
    {
        public Shelf Shelf { get; set; }
        public Part Part { get; set; }
    }
}