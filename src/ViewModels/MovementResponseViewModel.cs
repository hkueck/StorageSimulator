using System;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;

namespace StorageSimulator.ViewModels
{
    public class MovementResponseViewModel: MovementViewModelBase, IMovementResponseViewModel
    {
        public string Status { get; set; }

        public MovementResponseViewModel(MovementResponse response): base(response)
        {
            SetStatus(response.Status);
        }

        private void SetStatus(AutomationStatus status)
        {
            switch (status)
            {
                case AutomationStatus.TransportSucceeded:
                    Status = "Transport succeeded";
                    break;
                case AutomationStatus.InsertionSucceeded:
                    Status = "Insertion succeeded";
                    break;
                case AutomationStatus.DeletionSucceeded:
                    Status = "Deletion succeeded";
                    break;
                case AutomationStatus.InvalidOrderTargetSourceNotFound:
                    Status = "Target or source not found";
                    break;
                case AutomationStatus.WrongPartCount:
                    Status = "Wrong part count";
                    break;
                case AutomationStatus.ShippedNotAllItems:
                    Status = "Shipped not all items";
                    break;
                case AutomationStatus.InsertionFailed:
                    Status = "Insertion failed";
                    break;
                case AutomationStatus.CountIsZero:
                    Status = "Count is zero";
                    break;
                case AutomationStatus.OrderAlreadyExists:
                    Status = "Order already exists";
                    break;
            }
        }
    }
}