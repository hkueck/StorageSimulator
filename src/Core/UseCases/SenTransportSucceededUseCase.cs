using System;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;

namespace StorageSimulator.Core.UseCases
{
    public class SenTransportSucceededUseCase : ISendTransportSucceededUseCase
    {
        private readonly ISendResponseUseCase _sendResponseUseCase;

        public SenTransportSucceededUseCase(ISendResponseUseCase sendResponseUseCase)
        {
            _sendResponseUseCase = sendResponseUseCase;
        }

        public void Execute(MovementRequest request)
        {
            var response = new MovementResponse
            {
                Info = $"Transport: {request.Info}", Quantity = request.Quantity, Source = request.Source,
                Status = AutomationStatus.TransportSucceeded, Target = request.Target, TargetCompartment = request.TargetCompartment,
                SourceCompartment = request.SourceCompartment, Ticket = request.Ticket, Timestamp = DateTime.UtcNow
            };
            _sendResponseUseCase.Execute(response);
        }
    }
}