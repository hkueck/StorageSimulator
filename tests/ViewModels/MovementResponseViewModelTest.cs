using System;
using FluentAssertions;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class MovementResponseViewModelTest
    {
        [Fact]
        public void ConstructorShouldSetProperties()
        {
            var expectedData = new MovementData();
            expectedData.Barcode = "expected Barcode";
            expectedData.Index = "1";
            var expectedSource = "expected source";
            var expectedInfo = "expected info";
            var expectedTargetShelf = "expected target shelf";
            var expectedTarget = "expected target";
            var expectedSourceShelf = "expected source shelf";
            var expectedQuantity = 2;
            var expectedStatus = $"Insertion succeeded";
            var expectedTicket = Guid.NewGuid();
            var response = new MovementResponse()
            {
                Info = expectedInfo, Quantity = expectedQuantity, Source = expectedSource, SourceCompartment = expectedSourceShelf, Target = expectedTarget,
                TargetCompartment = expectedTargetShelf,Ticket = expectedTicket, Timestamp = DateTime.UtcNow, Status = AutomationStatus.InsertionSucceeded
            };
            response.Data.Add(expectedData);

            var viewModel = new MovementResponseViewModel(response);

            viewModel.Name.Should().Be(expectedInfo);
            viewModel.Source.Should().Be(expectedSource);
            viewModel.SourceShelf.Should().Be(expectedSourceShelf);
            viewModel.Target.Should().Be(expectedTarget);
            viewModel.TargetShelf.Should().Be(expectedTargetShelf);
            viewModel.Quantity.Should().Be(expectedQuantity);
            viewModel.Timestamp.Should().BeBefore(DateTime.Now);
            viewModel.Ticket.Should().Be(expectedTicket);
            viewModel.Status.Should().Be(expectedStatus);
            viewModel.Barcode.Should().Be(expectedData.Barcode);
            viewModel.ToString().Should().Be(viewModel.Name);
        }
        
        [Fact]
        public void ConstructorForTransportSucceededShouldSetStatus()
        {
            var expectedData = new MovementData();
            var expectedStatus = $"Transport succeeded";
            var response = new MovementResponse()
            {
                Status = AutomationStatus.TransportSucceeded
            };
            response.Data.Add(expectedData);

            var viewModel = new MovementResponseViewModel(response);

            viewModel.Status.Should().Be(expectedStatus);
        }
        
        [Fact]
        public void ConstructorForDeletionSucceededShouldSetStatus()
        {
            var expectedData = new MovementData();
            var expectedStatus = $"Deletion succeeded";
            var response = new MovementResponse()
            {
                Status = AutomationStatus.DeletionSucceeded
            };
            response.Data.Add(expectedData);

            var viewModel = new MovementResponseViewModel(response);

            viewModel.Status.Should().Be(expectedStatus);
        }
        
        [Fact]
        public void ConstructorForCountIsZeroShouldSetStatus()
        {
            var expectedData = new MovementData();
            var expectedStatus = $"Count is zero";
            var response = new MovementResponse()
            {
                Status = AutomationStatus.CountIsZero
            };
            response.Data.Add(expectedData);

            var viewModel = new MovementResponseViewModel(response);

            viewModel.Status.Should().Be(expectedStatus);
        }
        
        [Fact]
        public void ConstructorForOrderAlreadyExistShouldSetStatus()
        {
            var expectedData = new MovementData();
            var expectedStatus = $"Order already exists";
            var response = new MovementResponse()
            {
                Status = AutomationStatus.OrderAlreadyExists
            };
            response.Data.Add(expectedData);

            var viewModel = new MovementResponseViewModel(response);

            viewModel.Status.Should().Be(expectedStatus);
        }
        
        [Fact]
        public void ConstructorForWrongPartCountShouldSetStatus()
        {
            var expectedData = new MovementData();
            var expectedStatus = $"Wrong part count";
            var response = new MovementResponse()
            {
                Status = AutomationStatus.WrongPartCount
            };
            response.Data.Add(expectedData);

            var viewModel = new MovementResponseViewModel(response);

            viewModel.Status.Should().Be(expectedStatus);
        }
        
        [Fact]
        public void ConstructorForShippedNotAllItemsShouldSetStatus()
        {
            var expectedData = new MovementData();
            var expectedStatus = $"Shipped not all items";
            var response = new MovementResponse()
            {
                Status = AutomationStatus.ShippedNotAllItems
            };
            response.Data.Add(expectedData);

            var viewModel = new MovementResponseViewModel(response);

            viewModel.Status.Should().Be(expectedStatus);
        }
        
        [Fact]
        public void ConstructorForTargetOrSourceNotFoundShouldSetStatus()
        {
            var expectedData = new MovementData();
            var expectedStatus = $"Target or source not found";
            var response = new MovementResponse()
            {
                Status = AutomationStatus.InvalidOrderTargetSourceNotFound
            };
            response.Data.Add(expectedData);

            var viewModel = new MovementResponseViewModel(response);

            viewModel.Status.Should().Be(expectedStatus);
        }
        
        [Fact]
        public void ConstructorForInsertFailedShouldSetStatus()
        {
            var expectedData = new MovementData();
            var expectedStatus = $"Insertion failed";
            var response = new MovementResponse()
            {
                Status = AutomationStatus.InsertionFailed
            };
            response.Data.Add(expectedData);

            var viewModel = new MovementResponseViewModel(response);

            viewModel.Status.Should().Be(expectedStatus);
        }
    }
}