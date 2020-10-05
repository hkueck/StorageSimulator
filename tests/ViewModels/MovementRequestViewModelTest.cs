using System;
using FluentAssertions;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class MovementRequestViewModelTest
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
            var expectedType = $"Transport";
            var expectedTicket = Guid.NewGuid();
            var request = new MovementRequest
            {
                Info = expectedInfo, Quantity = expectedQuantity, Source = expectedSource, SourceCompartment = expectedSourceShelf, Target = expectedTarget,
                TargetCompartment = expectedTargetShelf, Task = AutomationTasks.Transport, Ticket = expectedTicket, Timestamp = DateTime.UtcNow
            };
            request.Data.Add(expectedData);

            var viewModel = new MovementRequestViewModel(request);

            viewModel.Name.Should().Be(expectedInfo);
            viewModel.Source.Should().Be(expectedSource);
            viewModel.SourceShelf.Should().Be(expectedSourceShelf);
            viewModel.Target.Should().Be(expectedTarget);
            viewModel.TargetShelf.Should().Be(expectedTargetShelf);
            viewModel.Quantity.Should().Be(expectedQuantity);
            viewModel.Timestamp.Should().BeBefore(DateTime.UtcNow);
            viewModel.Ticket.Should().Be(expectedTicket);
            viewModel.Type.Should().Be(expectedType);
            viewModel.Barcode.Should().Be(expectedData.Barcode);
            viewModel.ToString().Should().Be(viewModel.Name);
        }

        [Fact]
        public void ConstructorWithInsertRequestShouldSetTypeToInsert()
        {
            var expectedType = $"Insert";
            var request = new MovementRequest
            {
                Task = AutomationTasks.Insert
            };

            var viewModel = new MovementRequestViewModel(request);

            viewModel.Type.Should().Be(expectedType);
        }

        [Fact]
        public void ConstructorWithDeleteRequestShouldSetTypeToDelete()
        {
            var expectedType = $"Delete";
            var request = new MovementRequest
            {
                Task = AutomationTasks.Delete
            };

            var viewModel = new MovementRequestViewModel(request);

            viewModel.Type.Should().Be(expectedType);
        }

        [Fact]
        public void ConstructorWithTwoBarcodesRequestShouldSetBarcode()
        {
            var data1 = new MovementData{Barcode = "barcode1", Index = "1"};
            var data2 = new MovementData{Barcode = "barcode2", Index = "2"};
            var request = new MovementRequest();
            request.Data.Add(data1);
            request.Data.Add(data2);

            var viewModel = new MovementRequestViewModel(request);

            viewModel.Barcode.Should().Be("barcode1  barcode2");
        }
        
    }
}