using System.Linq;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;

namespace StorageSimulator.Core.UseCases
{
    public class AnalyseRequestUseCase : IAnalyseRequestUseCase
    {
        public IStorageSystem StorageSystem { get; set; }

        public void Execute(MovementRequest request)
        {
            if (StorageSystem != null)
            {
                switch (request.Task)
                {
                    case AutomationTasks.Transport:
                        AnalyseTransport(request);
                        break;
                    case AutomationTasks.Insert:
                        AnalyseStoragePoint(request);
                        break;
                }
            }
        }

        private void AnalyseTransport(MovementRequest request)
        {
            CheckStoresAndStoragePoints(request);
            var store = StorageSystem.Stores.FirstOrDefault(s => s.Name == request.Target);
            if (null != store)
            {
                MovePartToStore(store, request);
            }
            else
            {
                store = StorageSystem.Stores.FirstOrDefault(sp => sp.Name == request.Source);
                var storagePoint = StorageSystem.StoragePoints.FirstOrDefault(sp => sp.Name == request.Target);
                MovePartToWorkstation(store, storagePoint, request);
            }
        }

        private void MovePartToWorkstation(Store store, StoragePoint storagePoint, MovementRequest request)
        {
            var shelf = store.Shelves.FirstOrDefault(s => s.Number == request.SourceCompartment);
            if (null == shelf)
            {
                shelf = new Shelf {Number = request.TargetCompartment, Store = store};
                StorageSystem.AddShelfToStore(store, shelf);
            }

            var position = shelf.Parts.Count - 1;
            for (int i = 0; i < request.Quantity; i++)
            {
                if (position < shelf.Parts.Count)
                {
                    var part = shelf.Parts.FirstOrDefault(p => p.Position == position);
                    StorageSystem.RemovePartFromShelf(shelf, part);
                    StorageSystem.AddPartToStoragePoint(storagePoint, part);
                }

                position--;
            }
        }

        private void MovePartToStore(Store store, MovementRequest request)
        {
            var shelf = store.Shelves.FirstOrDefault(s => s.Number == request.TargetCompartment);
            if (null == shelf)
            {
                shelf = new Shelf {Number = request.TargetCompartment, Store = store};
                StorageSystem.AddShelfToStore(store, shelf);
            }

            var position = shelf.Parts.Count;
            for (int i = 0; i < request.Quantity; i++)
            {
                var part = new Part();
                var movementData = request.Data.FirstOrDefault(d => d.Index == (i + 1).ToString());
                if (null != movementData)
                {
                    part.Barcode = movementData.Barcode;
                }
                else
                {
                    part.Barcode = $"Part on position {position}";
                }

                part.Position = position;
                StorageSystem.AddPartToShelf(shelf, part);
                position++;
            }
        }

        private void AnalyseStoragePoint(MovementRequest request)
        {
            if (StorageSystem.StoragePoints.All(sp => sp.Name != request.Target))
            {
                CreateStoreOrStoragePoint(request.Target, request.TargetCompartment);
            }

            var storagePoint = StorageSystem.StoragePoints.First(sp => sp.Name == request.Target);
            storagePoint.Parts.Add(new Part {Barcode = request.Data.First().Barcode, Position = storagePoint.Parts.Count});
        }

        private void CheckStoresAndStoragePoints(MovementRequest request)
        {
            if (StorageSystem.StoragePoints.All(sp => sp.Name != request.Target) && StorageSystem.Stores.All(s => s.Name != request.Target))
            {
                CreateStoreOrStoragePoint(request.Target, request.TargetCompartment);
            }

            if (StorageSystem.StoragePoints.All(sp => sp.Name != request.Source) && StorageSystem.Stores.All(s => s.Name != request.Source))
            {
                CreateStoreOrStoragePoint(request.Source, request.SourceCompartment);
            }
        }

        private void CreateStoreOrStoragePoint(string name, string shelf)
        {
            if (name.Contains("TV") || name.Contains("AV"))
            {
                StorageSystem.AddStoragePoint(new StoragePoint {Name = name});
            }
            else
            {
                var store = new Store {Name = name};
                store.Shelves.Add(new Shelf {Number = shelf, Store = store});
                StorageSystem.AddStore(store);
            }
        }
    }
}