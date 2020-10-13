using System.Collections.Generic;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;

namespace StorageSimulator.ViewModels
{
    public class MovementRequestViewModel: MovementViewModelBase, IMovementRequestViewModel
    {
        public string Type { get; set; }

        public MovementRequestViewModel(MovementRequest movement): base (movement)
        {
            SetType(movement.Task);
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
    }
}