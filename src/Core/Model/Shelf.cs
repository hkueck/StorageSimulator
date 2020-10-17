using System.Collections.Generic;

namespace StorageSimulator.Core.Model
{
    public class Shelf
    {
        public Store Store { get; set; }
        public string Number { get; set; }
        public IList<Part> Parts { get; } = new List<Part>();
    }
}