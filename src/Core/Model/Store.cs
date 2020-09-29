using System.Collections.Generic;

namespace StorageSimulator.Core.Model
{
    public class Store
    {
        public string Name { get; set; }
        public IList<Shelf> Shelves { get; set; }
    }
}