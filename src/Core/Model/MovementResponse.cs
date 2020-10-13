using System.Xml.Serialization;
using StorageSimulator.Core.Types;

namespace StorageSimulator.Core.Model
{
    [XmlType(TypeName = "dataentry")]
    public class MovementResponse : MovementBase
    {
        [XmlElement("STATUS")]
        public AutomationStatus Status { get; set; } = AutomationStatus.TransportSucceeded;
    }
}