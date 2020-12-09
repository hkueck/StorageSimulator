using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Events
{
    public class InsertPartToDeliveryEvent
    {
        public StoragePoint DeliveryPoint { get; set; }
        public Part Part { get; set; }
    }
}