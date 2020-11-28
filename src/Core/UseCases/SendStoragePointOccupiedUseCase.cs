using System;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;

namespace StorageSimulator.Core.UseCases
{
    public class SendStoragePointOccupiedUseCase : ISendStoragePointOccupiedUseCase
    {
        private readonly ISendResponseUseCase _sendResponseUseCase;

        public SendStoragePointOccupiedUseCase(ISendResponseUseCase sendResponseUseCase)
        {
            _sendResponseUseCase = sendResponseUseCase;
        }

        public void Execute(MovementRequest request)
        {
            var response = new MovementResponse
            {
                Info = $"Storage point occupied: {request.Info}", Quantity = request.Quantity, Source = request.Source,
                Status = AutomationStatus.InsertionFailed, Target = request.Target, TargetCompartment = request.TargetCompartment,
                SourceCompartment = request.SourceCompartment, Ticket = request.Ticket, Timestamp = DateTime.UtcNow
            };
            _sendResponseUseCase.Execute(response);

        }
    }
}