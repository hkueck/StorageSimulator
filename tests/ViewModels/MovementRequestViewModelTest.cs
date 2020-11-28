using System;
using CommonServiceLocator;
using FluentAssertions;
using Moq;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;
using StorageSimulator.ViewModels;
using Xunit;

namespace StorageSimulatorTests.ViewModels
{
    public class MovementRequestViewModelTest
    {
        private bool _storagePointOccupiedCalled;
        private bool _insertCalled;
        private bool _transportCalled;
        private bool _wrongPartNumberCalled;
        private bool _wrongSourceTargetCalled;
        private bool _countZeroCalled;
        private bool _orderExistCalled;
        private bool _storeOccupiedCalled;

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
            viewModel.SendTransportSucceededCommand.CanExecute().Should().BeTrue();
            viewModel.SendWrongPartCountCommand.CanExecute().Should().BeTrue();
            viewModel.SendInsertSucceededCommand.CanExecute().Should().BeFalse();
            viewModel.SendWrongSourceTargetCommand.CanExecute().Should().BeTrue();
            viewModel.SendStoragePointOccupiedCommand.CanExecute().Should().BeFalse();
            viewModel.SendCountZeroCommand.CanExecute().Should().BeTrue();
        }
        
        [Fact]
        public void TransportSucceededCommandExecuteShouldCallSendTransportSucceededUseCase()
        {
            var useCase = new Mock<ISendTransportSucceededUseCase>();
            var serviceLocator = new Mock<IServiceLocator>();
            serviceLocator.Setup(l => l.GetInstance<ISendTransportSucceededUseCase>()).Returns(useCase.Object);
            ServiceLocator.SetLocatorProvider(() => serviceLocator.Object);
            var expectedData = new MovementData();
            expectedData.Barcode = "expected Barcode";
            expectedData.Index = "1";
            var expectedSource = "expected source";
            var expectedInfo = "expected info";
            var expectedTargetShelf = "expected target shelf";
            var expectedTarget = "expected target";
            var expectedSourceShelf = "expected source shelf";
            var expectedQuantity = 2;
            var expectedTicket = Guid.NewGuid();
            var request = new MovementRequest
            {
                Info = expectedInfo, Quantity = expectedQuantity, Source = expectedSource, SourceCompartment = expectedSourceShelf, Target = expectedTarget,
                TargetCompartment = expectedTargetShelf, Task = AutomationTasks.Transport, Ticket = expectedTicket, Timestamp = DateTime.UtcNow
            };
            request.Data.Add(expectedData);
            var viewModel = new MovementRequestViewModel(request);
            CommandRaised(viewModel);
            
            viewModel.SendTransportSucceededCommand.Execute();

            useCase.Verify(u => u.Execute(request));
            CheckExecutesShouldBeFalse(viewModel);
            CheckCommandRaises();
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
            viewModel.SendTransportSucceededCommand.CanExecute().Should().BeFalse();
            viewModel.SendWrongSourceTargetCommand.CanExecute().Should().BeTrue();
            viewModel.SendWrongPartCountCommand.CanExecute().Should().BeFalse();
            viewModel.SendStoragePointOccupiedCommand.CanExecute().Should().BeTrue();
            viewModel.SendInsertSucceededCommand.CanExecute().Should().BeTrue();
            viewModel.SendCountZeroCommand.CanExecute().Should().BeTrue();
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

        [Fact]
        public void ConstructorWithTransportRequestShouldEnableTransportCommands()
        {
            var request = new MovementRequest{Task = AutomationTasks.Transport};

            var viewModel = new MovementRequestViewModel(request);

            viewModel.SendTransportSucceededCommand.CanExecute().Should().BeTrue();
            viewModel.SendWrongPartCountCommand.CanExecute().Should().BeTrue();
        }

        [Fact]
        public void ConstructorWithInsertRequestShouldEnableSendInsertSucceededCommand()
        {
            
            var request = new MovementRequest{Task = AutomationTasks.Insert};

            var viewModel = new MovementRequestViewModel(request);

            viewModel.SendInsertSucceededCommand.CanExecute().Should().BeTrue();
        }

        [Fact]
        public void InsertSucceededCommandExecuteShouldCallSendInsertSucceededUseCase()
        {
            var useCase = new Mock<ISendInsertSucceededUseCase>();
            var serviceLocator = new Mock<IServiceLocator>();
            serviceLocator.Setup(l => l.GetInstance<ISendInsertSucceededUseCase>()).Returns(useCase.Object);
            ServiceLocator.SetLocatorProvider(() => serviceLocator.Object);
            var expectedData = new MovementData();
            expectedData.Barcode = "expected Barcode";
            expectedData.Index = "1";
            var expectedSource = "expected source";
            var expectedInfo = "expected info";
            var expectedTargetShelf = "expected target shelf";
            var expectedTarget = "expected target";
            var expectedSourceShelf = "expected source shelf";
            var expectedQuantity = 2;
            var expectedTicket = Guid.NewGuid();
            var request = new MovementRequest
            {
                Info = expectedInfo, Quantity = expectedQuantity, Source = expectedSource, SourceCompartment = expectedSourceShelf, Target = expectedTarget,
                TargetCompartment = expectedTargetShelf, Task = AutomationTasks.Insert, Ticket = expectedTicket, Timestamp = DateTime.UtcNow
            };
            request.Data.Add(expectedData);
            var viewModel = new MovementRequestViewModel(request);
            CommandRaised(viewModel);
            
            viewModel.SendInsertSucceededCommand.Execute();

            useCase.Verify(u => u.Execute(request));
            CheckExecutesShouldBeFalse(viewModel);
            CheckCommandRaises();
        }

        [Fact]
        public void WrongSourceTargetCommandExecuteShouldCallSendWrongSourceTargetUseCase()
        {
            var useCase = new Mock<ISendWrongSourceTargetUseCase>();
            var serviceLocator = new Mock<IServiceLocator>();
            serviceLocator.Setup(l => l.GetInstance<ISendWrongSourceTargetUseCase>()).Returns(useCase.Object);
            ServiceLocator.SetLocatorProvider(() => serviceLocator.Object);
            var expectedData = new MovementData();
            expectedData.Barcode = "expected Barcode";
            expectedData.Index = "1";
            var expectedSource = "expected source";
            var expectedInfo = "expected info";
            var expectedTargetShelf = "expected target shelf";
            var expectedTarget = "expected target";
            var expectedSourceShelf = "expected source shelf";
            var expectedQuantity = 2;
            var expectedTicket = Guid.NewGuid();
            var request = new MovementRequest
            {
                Info = expectedInfo, Quantity = expectedQuantity, Source = expectedSource, SourceCompartment = expectedSourceShelf, Target = expectedTarget,
                TargetCompartment = expectedTargetShelf, Task = AutomationTasks.Insert, Ticket = expectedTicket, Timestamp = DateTime.UtcNow
            };
            request.Data.Add(expectedData);
            var viewModel = new MovementRequestViewModel(request);
            CommandRaised(viewModel);
            
            viewModel.SendWrongSourceTargetCommand.Execute();

            useCase.Verify(u => u.Execute(request));
            CheckExecutesShouldBeFalse(viewModel);
            CheckCommandRaises();
        }

        [Fact]
        public void WrongPartNumberCommandExecuteShouldCallSendWrongPartNumberUseCase()
        {
            var useCase = new Mock<ISendWrongPartCountUseCase>();
            var serviceLocator = new Mock<IServiceLocator>();
            serviceLocator.Setup(l => l.GetInstance<ISendWrongPartCountUseCase>()).Returns(useCase.Object);
            ServiceLocator.SetLocatorProvider(() => serviceLocator.Object);
            var expectedData = new MovementData();
            expectedData.Barcode = "expected Barcode";
            expectedData.Index = "1";
            var expectedSource = "expected source";
            var expectedInfo = "expected info";
            var expectedTargetShelf = "expected target shelf";
            var expectedTarget = "expected target";
            var expectedSourceShelf = "expected source shelf";
            var expectedQuantity = 2;
            var expectedTicket = Guid.NewGuid();
            var request = new MovementRequest
            {
                Info = expectedInfo, Quantity = expectedQuantity, Source = expectedSource, SourceCompartment = expectedSourceShelf, Target = expectedTarget,
                TargetCompartment = expectedTargetShelf, Task = AutomationTasks.Insert, Ticket = expectedTicket, Timestamp = DateTime.UtcNow
            };
            request.Data.Add(expectedData);
            var viewModel = new MovementRequestViewModel(request);
            CommandRaised(viewModel);

            viewModel.SendWrongPartCountCommand.Execute();

            useCase.Verify(u => u.Execute(request));
            CheckExecutesShouldBeFalse(viewModel);
            CheckCommandRaises();
        }

        [Fact]
        public void StoragePointOccupiedCommandExecuteShouldCallSendStoragePointOccupiedUseCase()
        {
            var useCase = new Mock<ISendStoragePointOccupiedUseCase>();
            var serviceLocator = new Mock<IServiceLocator>();
            serviceLocator.Setup(l => l.GetInstance<ISendStoragePointOccupiedUseCase>()).Returns(useCase.Object);
            ServiceLocator.SetLocatorProvider(() => serviceLocator.Object);
            var expectedData = new MovementData();
            expectedData.Barcode = "expected Barcode";
            expectedData.Index = "1";
            var expectedSource = "expected source";
            var expectedInfo = "expected info";
            var expectedTargetShelf = "expected target shelf";
            var expectedTarget = "expected target";
            var expectedSourceShelf = "expected source shelf";
            var expectedQuantity = 2;
            var expectedTicket = Guid.NewGuid();
            var request = new MovementRequest
            {
                Info = expectedInfo, Quantity = expectedQuantity, Source = expectedSource, SourceCompartment = expectedSourceShelf, Target = expectedTarget,
                TargetCompartment = expectedTargetShelf, Task = AutomationTasks.Insert, Ticket = expectedTicket, Timestamp = DateTime.UtcNow
            };
            request.Data.Add(expectedData);
            var viewModel = new MovementRequestViewModel(request);
            CommandRaised(viewModel);

            viewModel.SendStoragePointOccupiedCommand.Execute();

            useCase.Verify(u => u.Execute(request));
            CheckExecutesShouldBeFalse(viewModel);
            CheckCommandRaises();
        }
        
        [Fact]
        public void CountZeroCommandExecuteShouldCallSendCountZeroUseCase()
        {
            var useCase = new Mock<ISendCountZeroUseCase>();
            var serviceLocator = new Mock<IServiceLocator>();
            serviceLocator.Setup(l => l.GetInstance<ISendCountZeroUseCase>()).Returns(useCase.Object);
            ServiceLocator.SetLocatorProvider(() => serviceLocator.Object);
            var expectedData = new MovementData();
            expectedData.Barcode = "expected Barcode";
            expectedData.Index = "1";
            var expectedSource = "expected source";
            var expectedInfo = "expected info";
            var expectedTargetShelf = "expected target shelf";
            var expectedTarget = "expected target";
            var expectedSourceShelf = "expected source shelf";
            var expectedQuantity = 2;
            var expectedTicket = Guid.NewGuid();
            var request = new MovementRequest
            {
                Info = expectedInfo, Quantity = expectedQuantity, Source = expectedSource, SourceCompartment = expectedSourceShelf, Target = expectedTarget,
                TargetCompartment = expectedTargetShelf, Task = AutomationTasks.Insert, Ticket = expectedTicket, Timestamp = DateTime.UtcNow
            };
            request.Data.Add(expectedData);
            var viewModel = new MovementRequestViewModel(request);
            CommandRaised(viewModel);

            viewModel.SendCountZeroCommand.Execute();

            useCase.Verify(u => u.Execute(request));
            CheckExecutesShouldBeFalse(viewModel);
            CheckCommandRaises();
        }
        
        [Fact]
        public void OrderExistCommandExecuteShouldCallSendOrderExistUseCase()
        {
            var useCase = new Mock<ISendOrderExistsUseCase>();
            var serviceLocator = new Mock<IServiceLocator>();
            serviceLocator.Setup(l => l.GetInstance<ISendOrderExistsUseCase>()).Returns(useCase.Object);
            ServiceLocator.SetLocatorProvider(() => serviceLocator.Object);
            var expectedData = new MovementData();
            expectedData.Barcode = "expected Barcode";
            expectedData.Index = "1";
            var expectedSource = "expected source";
            var expectedInfo = "expected info";
            var expectedTargetShelf = "expected target shelf";
            var expectedTarget = "expected target";
            var expectedSourceShelf = "expected source shelf";
            var expectedQuantity = 2;
            var expectedTicket = Guid.NewGuid();
            var request = new MovementRequest
            {
                Info = expectedInfo, Quantity = expectedQuantity, Source = expectedSource, SourceCompartment = expectedSourceShelf, Target = expectedTarget,
                TargetCompartment = expectedTargetShelf, Task = AutomationTasks.Insert, Ticket = expectedTicket, Timestamp = DateTime.UtcNow
            };
            request.Data.Add(expectedData);
            var viewModel = new MovementRequestViewModel(request);
            CommandRaised(viewModel);

            viewModel.SendOrderExistCommand.Execute();

            useCase.Verify(u => u.Execute(request));
            CheckExecutesShouldBeFalse(viewModel);
            CheckCommandRaises();
        }
        
        [Fact]
        public void StoreOccupiedCommandExecuteShouldCallSendStoreOccupiedUseCase()
        {
            var useCase = new Mock<ISendStoreOccupiedUseCase>();
            var serviceLocator = new Mock<IServiceLocator>();
            serviceLocator.Setup(l => l.GetInstance<ISendStoreOccupiedUseCase>()).Returns(useCase.Object);
            ServiceLocator.SetLocatorProvider(() => serviceLocator.Object);
            var expectedData = new MovementData();
            expectedData.Barcode = "expected Barcode";
            expectedData.Index = "1";
            var expectedSource = "expected source";
            var expectedInfo = "expected info";
            var expectedTargetShelf = "expected target shelf";
            var expectedTarget = "expected target";
            var expectedSourceShelf = "expected source shelf";
            var expectedQuantity = 2;
            var expectedTicket = Guid.NewGuid();
            var request = new MovementRequest
            {
                Info = expectedInfo, Quantity = expectedQuantity, Source = expectedSource, SourceCompartment = expectedSourceShelf, Target = expectedTarget,
                TargetCompartment = expectedTargetShelf, Task = AutomationTasks.Insert, Ticket = expectedTicket, Timestamp = DateTime.UtcNow
            };
            request.Data.Add(expectedData);
            var viewModel = new MovementRequestViewModel(request);
            CommandRaised(viewModel);

            viewModel.SendStoreOccupiedCommand.Execute();

            useCase.Verify(u => u.Execute(request));
            CheckExecutesShouldBeFalse(viewModel);
            CheckCommandRaises();
        }

        private void CheckCommandRaises()
        {
            _insertCalled.Should().BeTrue();
            _transportCalled.Should().BeTrue();
            _storagePointOccupiedCalled.Should().BeTrue();
            _wrongPartNumberCalled.Should().BeTrue();
            _wrongSourceTargetCalled.Should().BeTrue();
            _countZeroCalled.Should().BeTrue();
            _orderExistCalled.Should().BeTrue();
            _storeOccupiedCalled.Should().BeTrue();
        }

        private void CommandRaised(MovementRequestViewModel viewModel)
        {
            viewModel.SendStoragePointOccupiedCommand.CanExecuteChanged += (sender, args) => _storagePointOccupiedCalled = true;
            viewModel.SendInsertSucceededCommand.CanExecuteChanged += (sender, args) => _insertCalled = true;
            viewModel.SendTransportSucceededCommand.CanExecuteChanged += (sender, args) => _transportCalled = true;
            viewModel.SendWrongPartCountCommand.CanExecuteChanged += (sender, args) => _wrongPartNumberCalled = true;
            viewModel.SendWrongSourceTargetCommand.CanExecuteChanged += (sender, args) => _wrongSourceTargetCalled = true;
            viewModel.SendCountZeroCommand.CanExecuteChanged += (sender, args) => _countZeroCalled = true;
            viewModel.SendOrderExistCommand.CanExecuteChanged += (sender, args) => _orderExistCalled = true;
            viewModel.SendStoreOccupiedCommand.CanExecuteChanged += (sender, args) => _storeOccupiedCalled = true;
        }

        private static void CheckExecutesShouldBeFalse(MovementRequestViewModel viewModel)
        {
            viewModel.SendWrongSourceTargetCommand.CanExecute().Should().BeFalse();
            viewModel.SendInsertSucceededCommand.CanExecute().Should().BeFalse();
            viewModel.SendTransportSucceededCommand.CanExecute().Should().BeFalse();
            viewModel.SendWrongPartCountCommand.CanExecute().Should().BeFalse();
            viewModel.SendCountZeroCommand.CanExecute().Should().BeFalse();
            viewModel.SendOrderExistCommand.CanExecute().Should().BeFalse();
            viewModel.SendStoreOccupiedCommand.CanExecute().Should().BeFalse();
        }
    }
}