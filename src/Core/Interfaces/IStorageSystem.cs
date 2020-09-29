using System.Collections.Generic;
using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Interfaces
{
    public interface IStorageSystem
    {
        void AddStoragePoint(StoragePoint storagePoint);
        IList<StoragePoint> StoragePoints { get; set; }
    }
}