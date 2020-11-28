using System;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;

namespace StorageSimulator.Core.UseCases
{
    public class SendOrderExistsUseCase : ISendOrderExistsUseCase
    {
        private readonly ISendResponseUseCase _sendResponseUseCase;

        public SendOrderExistsUseCase(ISendResponseUseCase sendResponseUseCase)
        {
            _sendResponseUseCase = sendResponseUseCase;
        }

        public void Execute(MovementRequest request)
        {
            var response = new MovementResponse
            {
                Info = $"Order already exists: {request.Info}", Quantity = request.Quantity, Source = request.Source,
                Status = AutomationStatus.OrderAlreadyExists, Target = request.Target, TargetCompartment = request.TargetCompartment,
                SourceCompartment = request.SourceCompartment, Ticket = request.Ticket, Timestamp = DateTime.UtcNow
            };
            _sendResponseUseCase.Execute(response);

        }
    }
}