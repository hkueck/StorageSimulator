using System.Xml.Serialization;
using StorageSimulator.Core.Types;

namespace StorageSimulator.Core.Model
{
    [XmlType(TypeName = "dataentry")]
    public class MovementRequest : MovementBase
    {
        [XmlElement("TASK")]
        public AutomationTasks Task { get; set; } = AutomationTasks.Transport;
    }
}