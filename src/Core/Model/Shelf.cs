using System.Collections.Generic;

namespace StorageSimulator.Core.Model
{
    public class Shelf
    {
        public string Number { get; set; }
        public IList<Part> Parts { get; } = new List<Part>();
    }
}