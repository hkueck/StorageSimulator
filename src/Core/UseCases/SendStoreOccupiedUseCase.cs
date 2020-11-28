using System;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;

namespace StorageSimulator.Core.UseCases
{
    public class SendStoreOccupiedUseCase : ISendStoreOccupiedUseCase
    {
        private readonly ISendResponseUseCase _sendResponseUseCase;

        public SendStoreOccupiedUseCase(ISendResponseUseCase sendResponseUseCase)
        {
            _sendResponseUseCase = sendResponseUseCase;
        }

        public void Execute(MovementRequest request)
        {
            var response = new MovementResponse
            {
                Info = $"Store occupied: {request.Info}", Quantity = request.Quantity, Source = request.Source,
                Status = AutomationStatus.ShippedNotAllItems, Target = request.Target, TargetCompartment = request.TargetCompartment,
                SourceCompartment = request.SourceCompartment, Ticket = request.Ticket, Timestamp = DateTime.UtcNow
            };
            _sendResponseUseCase.Execute(response);
        }
    }
}