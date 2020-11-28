using System;
using System.Collections.Generic;
using Prism.Mvvm;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;

namespace StorageSimulator.ViewModels
{
    public class MovementViewModelBase : BindableBase, IMovementViewModel
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string SourceShelf { get; set; }
        public string Target { get; set; }
        public string TargetShelf { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid Ticket { get; set; }
        public string Barcode { get; set; }

        protected MovementViewModelBase(MovementBase movement)
        {
            Name = movement.Info;
            Source = movement.Source;
            SourceShelf = movement.SourceCompartment;
            Target = movement.Target;
            TargetShelf = movement.TargetCompartment;
            Quantity = movement.Quantity;
            Timestamp = movement.Timestamp.ToLocalTime();
            Ticket = movement.Ticket;
            SetBarcode(movement.Data);
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

        public override string ToString()
        {
            return Name;
        }
    }
}