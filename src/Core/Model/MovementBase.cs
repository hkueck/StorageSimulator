using System;
using System.Xml.Serialization;

namespace StorageSimulator.Core.Model
{
    public class MovementBase
    {
        public Guid Ticket { get; set; } = Guid.NewGuid();
        public string Source { get; set; }
        public string SourceCompartment { get; set; }
        public string Target { get; set; }
        public string TargetCompartment { get; set; }
        public DateTime Timestamp { get; set; }
        public string Info { get; set; }
        public int Quantity { get; set; }
        public MovementData Data { get; set; }
    }

    public class MovementData
    {
        [XmlAttribute] public string Index { get; set; } = "1";
        public string Barcode { get; set; }
    }
}