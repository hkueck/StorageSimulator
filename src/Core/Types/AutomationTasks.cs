using System.Xml.Serialization;

namespace StorageSimulator.Core.Types
{
    public enum AutomationTasks
    {
        [XmlEnum(Name = "1")]
        Transport = 1,
        [XmlEnum(Name = "2")]
        Insert    = 2,
        [XmlEnum(Name = "3")]
        Delete    = 3
    }
}