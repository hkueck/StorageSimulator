using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Events
{
    public class InsertPartEvent
    {
        public string StoragePoint { get; set; }
        public Part Part { get; set; }
    }
}