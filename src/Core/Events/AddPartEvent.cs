using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Events
{
    public class AddPartEvent
    {
        public Part Part { get; set; }
        public Shelf Shelf { get; set; }
    }
}