using System;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;

namespace StorageSimulator.Core.UseCases
{
    public class SendWrongSourceTargetUseCase : ISendWrongSourceTargetUseCase
    {
        private readonly ISendResponseUseCase _sendResponseUseCase;

        public SendWrongSourceTargetUseCase(ISendResponseUseCase sendResponseUseCase)
        {
            _sendResponseUseCase = sendResponseUseCase;
        }

        public void Execute(MovementRequest request)
        {
            var response = new MovementResponse
            {
                Info = $"Wrong source or target: {request.Info}", Quantity = request.Quantity, Source = request.Source,
                Status = AutomationStatus.InvalidOrderTargetSourceNotFound, Target = request.Target, TargetCompartment = request.TargetCompartment,
                SourceCompartment = request.SourceCompartment, Ticket = request.Ticket, Timestamp = DateTime.UtcNow
            };
            _sendResponseUseCase.Execute(response);
        }
    }
}