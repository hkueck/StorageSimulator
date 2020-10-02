using System.Collections.Generic;

namespace StorageSimulator.Core.Model
{
    public class StoragePoint
    {
        public string Name { get; set; }
        public IList<Part> Parts { get; } = new List<Part>();
    }
}