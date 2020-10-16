using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace StorageSimulator.Core.Model
{
    public class MovementBase
    {
        [XmlElement("TICKET")]
        public Guid Ticket { get; set; } = Guid.NewGuid();
        [XmlElement("SOURCE")]
        public string Source { get; set; }
        [XmlElement("SOURCECOMPARTMENT")]
        public string SourceCompartment { get; set; }
        [XmlElement("TARGET")]
        public string Target { get; set; }
        [XmlElement("TARGETCOMPARTMENT")]
        public string TargetCompartment { get; set; }
        [XmlElement("TIMESTAMP")] 
        public string TimestampString { get; set; }

        [XmlElement("INFO")]
        public string Info { get; set; }

        [XmlElement("QUANTITY")]
        public int Quantity { get; set; }

        [XmlElement("DATA")]
        public List<MovementData> Data { get; } = new List<MovementData>();

        [XmlIgnore]
        public DateTime Timestamp
        {
            get
            {
                if (string.IsNullOrEmpty(TimestampString))
                    return new DateTime();
                return DateTime.ParseExact(TimestampString, "dd.MM.yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            }
            set
            {
                TimestampString = value.ToString("dd.MM.yyyy hh:mm:ss");
            }
        }
    }

    public class MovementData
    {
        [XmlAttribute(AttributeName = "INDEX")] 
        public string Index { get; set; } = "1";
        [XmlElement("BARCODE")]
        public string Barcode { get; set; }
    }
}
