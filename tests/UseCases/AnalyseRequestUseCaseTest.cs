using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.Types;
using StorageSimulator.Core.UseCases;
using Xunit;

namespace StorageSimulatorTests.UseCases
{
    public class AnalyseRequestUseCaseTest
    {
        private IAnalyseRequestUseCase _analyseRequestUseCase;

        public AnalyseRequestUseCaseTest()
        {
            _analyseRequestUseCase = new AnalyseRequestUseCase();
        }

        [Fact]
        public void AnalyseStoragePointShouldReturnStoragePointResponse()
        {
            var storageSystem = new Mock<IStorageSystem>();
            storageSystem.Setup(s => s.AddStoragePoint(It.IsAny<StoragePoint>())).Callback<StoragePoint>(point =>
            {
                storageSystem.SetupGet(s => s.StoragePoints).Returns(new List<StoragePoint> {point});
            });
            storageSystem.SetupGet(s => s.StoragePoints).Returns(new List<StoragePoint>());
            _analyseRequestUseCase.StorageSystem = storageSystem.Object;
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Data = new MovementData {Barcode = "12345"}, Info = "part in new storage point", Quantity = 1, Target = "TV01",
                TargetCompartment = "1", Task = AutomationTasks.Insert
            };

            var response = _analyseRequestUseCase.Analyse(request);

            response.Should().NotBeNull();
            response.Status.Should().Be(AutomationStatus.InsertionSucceeded);
            response.Quantity.Should().Be(request.Quantity);
            response.Target.Should().Be(request.Target);
            response.TargetCompartment.Should().Be(request.TargetCompartment);
            response.Timestamp.Should().BeBefore(DateTime.UtcNow);
            response.Ticket.Should().Be(request.Ticket);
        }

        [Fact]
        public void AnalyseNewStoragePointShouldAddStoragePoint()
        {
            StoragePoint newStoragePoint = null;
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Data = new MovementData {Barcode = "12345"}, Info = "part in new storage point", Quantity = 1, Target = "TV01",
                TargetCompartment = "1", Task = AutomationTasks.Insert
            };
            var storageSystem = new Mock<IStorageSystem>();
            storageSystem.Setup(s => s.AddStoragePoint(It.IsAny<StoragePoint>())).Callback<StoragePoint>(point =>
            {
                newStoragePoint = point;
                storageSystem.SetupGet(s => s.StoragePoints).Returns(new List<StoragePoint> {newStoragePoint});
            });
            storageSystem.SetupGet(s => s.StoragePoints).Returns(new List<StoragePoint>());
            _analyseRequestUseCase.StorageSystem = storageSystem.Object;

            _analyseRequestUseCase.Analyse(request);
            
            storageSystem.Verify(s => s.AddStoragePoint(It.IsAny<StoragePoint>()));
            newStoragePoint.Name.Should().Be(request.Target);
        }

        [Fact]
        public void AnalyseExistingStoragePointShouldNotAddStoragePoint()
        {
            var storagePoint = new StoragePoint{Name = "TV01"};
            var storagePoints = new List<StoragePoint>{storagePoint};
            var storageSystem = new Mock<IStorageSystem>();
            storageSystem.SetupGet(s => s.StoragePoints).Returns(storagePoints);
            _analyseRequestUseCase.StorageSystem = storageSystem.Object;
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Data = new MovementData {Barcode = "12345"}, Info = "part in new storage point", Quantity = 1, Target = "TV01",
                TargetCompartment = "1", Task = AutomationTasks.Insert
            };

            _analyseRequestUseCase.Analyse(request);
            
            storageSystem.Verify(s => s.AddStoragePoint(It.IsAny<StoragePoint>()),Times.Never);
        }

        [Fact]
        public void AnalyseNewPartForStoragePointShouldAddPartToStoragePoint()
        {
            var expected = "12345";
            var storagePoint = new StoragePoint{Name = "TV01"};
            var storagePoints = new List<StoragePoint>{storagePoint};
            var storageSystem = new Mock<IStorageSystem>();
            storageSystem.SetupGet(s => s.StoragePoints).Returns(storagePoints);
            _analyseRequestUseCase.StorageSystem = storageSystem.Object;
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Data = new MovementData {Barcode = expected}, Info = "part in new storage point", Quantity = 1, Target = "TV01",
                TargetCompartment = "1", Task = AutomationTasks.Insert
            };

            _analyseRequestUseCase.Analyse(request);

            storagePoint.Parts.Count.Should().Be(1);
            var part = storagePoint.Parts.First();
            part.Barcode.Should().Be(expected);
            part.Position.Should().Be(0);
        }
    }
}