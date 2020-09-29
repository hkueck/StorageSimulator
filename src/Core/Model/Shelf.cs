using System.Collections.Generic;

namespace StorageSimulator.Core.Model
{
    public class Shelf
    {
        public int Number { get; set; }
        public IList<Part> Parts { get; set; }
    }
}