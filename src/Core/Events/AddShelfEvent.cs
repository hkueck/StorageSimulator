using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Events
{
    public class AddShelfEvent
    {
        public Shelf Shelf { get; set; }
        public Store Store { get; set; }
    }
}