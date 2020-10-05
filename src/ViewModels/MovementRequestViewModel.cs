using System;
using System.Collections.Generic;
using Prism.Mvvm;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;

namespace StorageSimulator.ViewModels
{
    public class MovementRequestViewModel: BindableBase
    {
        public string Name { get; set; }

        public string Source { get; set; }
        public string SourceShelf { get; set; }
        public string Target { get; set; }
        public string TargetShelf { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid Ticket { get; set; }
        public string Type { get; set; }
        public string Barcode { get; set; }

        public MovementRequestViewModel(MovementRequest request)
        {
            Name = request.Info;
            Source = request.Source;
            SourceShelf = request.SourceCompartment;
            Target = request.Target;
            TargetShelf = request.TargetCompartment;
            Quantity = request.Quantity;
            Timestamp = request.Timestamp;
            Ticket = request.Ticket;
            SetType(request.Task);
            SetBarcode(request.Data);
        }

        private void SetBarcode(List<MovementData> requestData)
        {
            foreach (var movementData in requestData)
            {
                if (!string.IsNullOrWhiteSpace(Barcode))
                {
                    Barcode += "  ";
                }
                Barcode += movementData.Barcode;
            }
        }

        private void SetType(AutomationTasks requestTask)
        {
            switch (requestTask)
            {
                case AutomationTasks.Transport:
                    Type = "Transport";
                    break;
                case AutomationTasks.Insert:
                    Type = "Insert";
                    break;
                case AutomationTasks.Delete:
                    Type = "Delete";
                    break;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}