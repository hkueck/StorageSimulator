using System;
using System.Xml.Serialization;
using StorageSimulator.Core.Types;

namespace StorageSimulator.Core.Model
{
    [XmlType(TypeName = "dataentry")]
    public class Movement
    {
        public Guid Ticket { get; set; } = Guid.NewGuid();
        public string Source { get; set; }
        public string SourceCompartment { get; set; }
        public string Target { get; set; }
        public string TargetCompartment { get; set; }
        public DateTime Timestamp { get; set; }
        public AutomationStatus Status { get; set; } = AutomationStatus.TransportOrder;
        public AutomationTasks Task { get; set; } = AutomationTasks.Transport;
        public string Info { get; set; }
        public int Quantity { get; set; }
    }
}