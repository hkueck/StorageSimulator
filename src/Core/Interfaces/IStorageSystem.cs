using System.Collections.Generic;
using StorageSimulator.Core.Model;

namespace StorageSimulator.Core.Interfaces
{
    public interface IStorageSystem
    {
        IList<StoragePoint> StoragePoints { get; set; }
        IList<Store> Stores { get; set; }
        void AddStoragePoint(StoragePoint storagePoint);
        void AddStore(Store store);
        void AddPartToShelf(Shelf shelf, Part part);
        void AddShelfToStore(Store store, Shelf shelf);
        void RemovePartFromShelf(Shelf shelf, Part part);
        void AddPartToStoragePoint(StoragePoint storagePoint, Part part);
    }
}