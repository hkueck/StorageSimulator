using System;
using System.Linq;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;

namespace StorageSimulator.Core.UseCases
{
    public class AnalyseRequestUseCase : IAnalyseRequestUseCase
    {
        public IStorageSystem StorageSystem { get; set; }

        public MovementResponse Analyse(MovementRequest request)
        {
            MovementResponse response = null;
            if (StorageSystem != null)
            {
                switch (request.Task)
                {
                    case AutomationTasks.Transport:
                        break;
                    case AutomationTasks.Insert:
                        response = AnalyseStoragePoint(request);
                        break;
                    case AutomationTasks.Delete:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }
            return response;
        }

        private MovementResponse AnalyseStoragePoint(MovementRequest request)
        {
            if (StorageSystem.StoragePoints.All(sp => sp.Name != request.Target))
            {
                var newStoragePoint = new StoragePoint {Name = request.Target};
                StorageSystem.AddStoragePoint(newStoragePoint);
            }

            var storagePoint = StorageSystem.StoragePoints.First(sp => sp.Name == request.Target);
            storagePoint.Parts.Add(new Part{Barcode = request.Data.Barcode, Position = storagePoint.Parts.Count});
            
            var response = new MovementResponse
            {
                Target = request.Target, Quantity = request.Quantity, Info = $"Response for {request.Info}", Ticket = request.Ticket,
                Timestamp = DateTime.UtcNow, Status = AutomationStatus.InsertionSucceeded, Data = request.Data, TargetCompartment = request.TargetCompartment
            };
            return response;
        }
    }
}