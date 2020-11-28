using System;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;

namespace StorageSimulator.Core.UseCases
{
    public class SendWrongPartCountUseCase : ISendWrongPartCountUseCase
    {
        private readonly ISendResponseUseCase _sendResponseUseCase;

        public SendWrongPartCountUseCase(ISendResponseUseCase sendResponseUseCase)
        {
            _sendResponseUseCase = sendResponseUseCase;
        }

        public void Execute(MovementRequest request)
        {
            var response = new MovementResponse
            {
                Info = $"Wrong part count: {request.Info}", Quantity = request.Quantity, Source = request.Source,
                Status = AutomationStatus.WrongPartCount, Target = request.Target, TargetCompartment = request.TargetCompartment,
                SourceCompartment = request.SourceCompartment, Ticket = request.Ticket, Timestamp = DateTime.UtcNow
            };
            _sendResponseUseCase.Execute(response);
        }
    }
}