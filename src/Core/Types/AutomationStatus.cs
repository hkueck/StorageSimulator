using System.Xml.Serialization;

namespace StorageSimulator.Core.Types
{
    public enum AutomationStatus
    {
        [XmlEnum(Name = "1")]
        TransportOrder = 1,
        [XmlEnum(Name = "2")]
        InsertionSucceeded = 2,
        [XmlEnum(Name = "3")]
        DeletionSucceeded = 3,
        [XmlEnum(Name = "11")]
        InvalidOrderTargetSourceNotFound = 11,
        [XmlEnum(Name = "12")]
        WrongPartCount = 12,
        [XmlEnum(Name = "13")]
        ShippedNotAllItems = 13,
        [XmlEnum(Name = "14")]
        InsertionFailed = 14,
        [XmlEnum(Name = "98")]
        CountIsZero = 98,
        [XmlEnum(Name = "99")]
        OrderAlreadyExists = 99
    }
}