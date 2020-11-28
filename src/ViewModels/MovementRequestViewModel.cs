using CommonServiceLocator;
using Prism.Commands;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;

namespace StorageSimulator.ViewModels
{
    public class MovementRequestViewModel: MovementViewModelBase, IMovementRequestViewModel
    {
        private readonly MovementRequest _movementRequest;
        private bool _executed;
        public string Type { get; set; }
        public DelegateCommand SendTransportSucceededCommand { get; }
        public DelegateCommand SendInsertSucceededCommand { get; }
        public DelegateCommand SendWrongSourceTargetCommand { get; }
        public DelegateCommand SendWrongPartCountCommand { get; }
        public DelegateCommand SendStoragePointOccupiedCommand { get; }
        public DelegateCommand SendCountZeroCommand { get; }
        public DelegateCommand SendOrderExistCommand { get; }
        public DelegateCommand SendStoreOccupiedCommand { get; }

        public MovementRequestViewModel(MovementRequest movement): base (movement)
        {
            _movementRequest = movement;
            SetType(movement.Task);
            SendTransportSucceededCommand = new DelegateCommand(TransportSucceeded, CanTransportSucceeded);
            SendInsertSucceededCommand = new DelegateCommand(InsertSucceeded, CanInsertSucceeded);
            SendWrongSourceTargetCommand = new DelegateCommand(WrongSourceTarget, CanExecute);
            SendWrongPartCountCommand = new DelegateCommand(WrongPartCount, CanWrongPartCount);
            SendStoragePointOccupiedCommand = new DelegateCommand(StoragePointOccupied, CanStoragePointOccupied);
            SendCountZeroCommand = new DelegateCommand(CountZero, CanExecute);
            SendOrderExistCommand = new DelegateCommand(OrderExist, CanExecute);
            SendStoreOccupiedCommand = new DelegateCommand(StoreOccupied, CanStoreOccupied);
        }

        private void StoreOccupied()
        {
            var useCase = ServiceLocator.Current.GetInstance<ISendStoreOccupiedUseCase>();
            ExecuteUseCase(useCase);
        }

        private bool CanStoreOccupied()
        {
            return _movementRequest.Task == AutomationTasks.Transport && CanExecute();
        }

        private void OrderExist()
        {
            var useCase = ServiceLocator.Current.GetInstance<ISendOrderExistsUseCase>();
            ExecuteUseCase(useCase);
        }

        private void CountZero()
        {
            var useCase = ServiceLocator.Current.GetInstance<ISendCountZeroUseCase>();
            ExecuteUseCase(useCase);
        }

        private void StoragePointOccupied()
        {
            var useCase = ServiceLocator.Current.GetInstance<ISendStoragePointOccupiedUseCase>();
            ExecuteUseCase(useCase);
        }

        private bool CanStoragePointOccupied()
        {
            return _movementRequest.Task == AutomationTasks.Insert && CanExecute();
        }

        private void WrongPartCount()
        {
            var useCase = ServiceLocator.Current.GetInstance<ISendWrongPartCountUseCase>();
            ExecuteUseCase(useCase);
        }

        private bool CanWrongPartCount()
        {
            return _movementRequest.Task == AutomationTasks.Transport && CanExecute();
        }

        private bool CanExecute()
        {
            return !_executed;
        }

        private void WrongSourceTarget()
        {
            var useCase = ServiceLocator.Current.GetInstance<ISendWrongSourceTargetUseCase>();
            ExecuteUseCase(useCase);
        }

        private void InsertSucceeded()
        {
            var useCase = ServiceLocator.Current.GetInstance<ISendInsertSucceededUseCase>();
            ExecuteUseCase(useCase);
        }

        private bool CanInsertSucceeded()
        {
            return _movementRequest.Task == AutomationTasks.Insert && !_executed;
        }

        private void TransportSucceeded()
        {
            var useCase = ServiceLocator.Current.GetInstance<ISendTransportSucceededUseCase>();
            ExecuteUseCase(useCase);
        }

        private void ExecuteUseCase(ISendUseCase useCase)
        {
            useCase.Execute(_movementRequest);
            _executed = true;
            RaiseCommands();
        }

        private void RaiseCommands()
        {
            SendTransportSucceededCommand.RaiseCanExecuteChanged();
            SendInsertSucceededCommand.RaiseCanExecuteChanged();
            SendWrongSourceTargetCommand.RaiseCanExecuteChanged();
            SendWrongPartCountCommand.RaiseCanExecuteChanged();
            SendStoragePointOccupiedCommand.RaiseCanExecuteChanged();
            SendCountZeroCommand.RaiseCanExecuteChanged();
            SendOrderExistCommand.RaiseCanExecuteChanged();
            SendStoreOccupiedCommand.RaiseCanExecuteChanged();
        }

        private bool CanTransportSucceeded()
        {
            return _movementRequest.Task == AutomationTasks.Transport && !_executed;
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