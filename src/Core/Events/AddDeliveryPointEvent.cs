using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Events
{
    public class AddDeliveryPointEvent
    {
        public StoragePoint DeliveryPoint { get; set; }
    }
}