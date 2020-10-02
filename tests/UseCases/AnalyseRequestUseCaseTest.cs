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
        private IAnalyseRequestUseCase _useCase;

        public AnalyseRequestUseCaseTest()
        {
            _useCase = new AnalyseRequestUseCase();
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
            _useCase.StorageSystem = storageSystem.Object;
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Info = "part in new storage point", Quantity = 1, Target = "TV01",
                TargetCompartment = "1", Task = AutomationTasks.Insert
            };
            request.Data.Add(new MovementData {Barcode = "12345"});

            var response = _useCase.Execute(request);

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
                Ticket = Guid.NewGuid(), Info = "part in new storage point", Quantity = 1, Target = "TV01",
                TargetCompartment = "1", Task = AutomationTasks.Insert
            };
            request.Data.Add(new MovementData {Barcode = "12345"});
            var storageSystem = new Mock<IStorageSystem>();
            storageSystem.Setup(s => s.AddStoragePoint(It.IsAny<StoragePoint>())).Callback<StoragePoint>(point =>
            {
                newStoragePoint = point;
                storageSystem.SetupGet(s => s.StoragePoints).Returns(new List<StoragePoint> {newStoragePoint});
            });
            storageSystem.SetupGet(s => s.StoragePoints).Returns(new List<StoragePoint>());
            _useCase.StorageSystem = storageSystem.Object;

            _useCase.Execute(request);

            storageSystem.Verify(s => s.AddStoragePoint(It.IsAny<StoragePoint>()));
            newStoragePoint.Name.Should().Be(request.Target);
        }

        [Fact]
        public void AnalyseExistingStoragePointShouldNotAddStoragePoint()
        {
            var storagePoint = new StoragePoint {Name = "TV01"};
            var storagePoints = new List<StoragePoint> {storagePoint};
            var storageSystem = new Mock<IStorageSystem>();
            storageSystem.SetupGet(s => s.StoragePoints).Returns(storagePoints);
            _useCase.StorageSystem = storageSystem.Object;
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Info = "part in new storage point", Quantity = 1, Target = "TV01",
                TargetCompartment = "1", Task = AutomationTasks.Insert
            };
            request.Data.Add(new MovementData {Barcode = "12345"});

            _useCase.Execute(request);

            storageSystem.Verify(s => s.AddStoragePoint(It.IsAny<StoragePoint>()), Times.Never);
        }

        [Fact]
        public void AnalyseNewPartForStoragePointShouldAddPartToStoragePoint()
        {
            var expected = "12345";
            var storagePoint = new StoragePoint {Name = "TV01"};
            var storagePoints = new List<StoragePoint> {storagePoint};
            var storageSystem = new Mock<IStorageSystem>();
            storageSystem.SetupGet(s => s.StoragePoints).Returns(storagePoints);
            _useCase.StorageSystem = storageSystem.Object;
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Info = "part in new storage point", Quantity = 1, Target = "TV01",
                TargetCompartment = "1", Task = AutomationTasks.Insert
            };
            request.Data.Add(new MovementData {Barcode = "12345"});

            _useCase.Execute(request);

            storagePoint.Parts.Count.Should().Be(1);
            var part = storagePoint.Parts.First();
            part.Barcode.Should().Be(expected);
            part.Position.Should().Be(0);
        }

        [Fact]
        public void AnalyseMovementToStoreShouldAddStoreAndShelf()
        {
            Store newStore = null;
            var storagePoint = new StoragePoint {Name = "TV01"};
            var storagePoints = new List<StoragePoint> {storagePoint};
            var stores = new List<Store>();
            var storageSystem = new Mock<IStorageSystem>();
            storageSystem.SetupGet(s => s.StoragePoints).Returns(storagePoints);
            storageSystem.SetupGet(s => s.Stores).Returns(stores);
            storageSystem.Setup(s => s.AddStore(It.IsAny<Store>())).Callback<Store>(addStore =>
            {
                newStore = addStore;
                storageSystem.SetupGet(s => s.Stores).Returns(new List<Store> {newStore});
            });
            _useCase.StorageSystem = storageSystem.Object;
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Info = "part in new store", Quantity = 1, Target = "B01",
                TargetCompartment = "1", Task = AutomationTasks.Transport, Source = "TV01", SourceCompartment = "1", Timestamp = DateTime.UtcNow
            };
            request.Data.Add(new MovementData {Barcode = "12345"});

            _useCase.Execute(request);

            storageSystem.Verify(s => s.AddStore(It.IsAny<Store>()));
            newStore.Name.Should().Be("B01");
            newStore.Shelves.Count.Should().Be(1);
            var shelf = newStore.Shelves.First();
            shelf.Number.Should().Be("1");
        }

        [Fact]
        public void AnalyseMovementToWorkstationShouldAddStoreAndShelf()
        {
            var expected = "12345";
            Store newStore = null;
            StoragePoint newStoragePoint = null;
            var storagePoint = new StoragePoint {Name = "TV01"};
            var storagePoints = new List<StoragePoint> {storagePoint};
            var stores = new List<Store>();
            var storageSystem = new Mock<IStorageSystem>();
            storageSystem.SetupGet(s => s.StoragePoints).Returns(storagePoints);
            storageSystem.Setup(s => s.AddStoragePoint(It.IsAny<StoragePoint>())).Callback<StoragePoint>(point =>
            {
                newStoragePoint = point;
                storageSystem.SetupGet(s => s.StoragePoints).Returns(new List<StoragePoint> {newStoragePoint});
            });
            storageSystem.SetupGet(s => s.Stores).Returns(stores);
            storageSystem.Setup(s => s.AddStore(It.IsAny<Store>())).Callback<Store>(addStore =>
            {
                newStore = addStore;
                storageSystem.SetupGet(s => s.Stores).Returns(new List<Store> {newStore});
            });
            _useCase.StorageSystem = storageSystem.Object;
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Info = "part to workstation", Quantity = 1, Target = "AV01",
                TargetCompartment = "1", Task = AutomationTasks.Transport, Source = "B01", SourceCompartment = "1", Timestamp = DateTime.UtcNow
            };
            request.Data.Add(new MovementData {Barcode = expected});
            
            _useCase.Execute(request);

            storageSystem.Verify(s => s.AddStore(It.IsAny<Store>()));
            storageSystem.Verify(s => s.AddStoragePoint(It.IsAny<StoragePoint>()));
            newStore.Name.Should().Be("B01");
            newStore.Shelves.Count.Should().Be(1);
            var shelf = newStore.Shelves.First();
            shelf.Number.Should().Be("1");
            newStoragePoint.Name.Should().Be("AV01");
        }

        [Fact]
        public void AnalyseMovementToStoreShouldReturnResponse()
        {
            var store = new Store {Name = "B01"};
            store.Shelves.Add(new Shelf {Number = "1"});
            var stores = new List<Store> {store};
            var storagePoint = new StoragePoint{Name = "TV01"};
            var storagePoints = new List<StoragePoint>{storagePoint};
            var storageSystem = new Mock<IStorageSystem>();
            storageSystem.SetupGet(s => s.Stores).Returns(stores);
            storageSystem.SetupGet((s => s.StoragePoints)).Returns(storagePoints);
            _useCase.StorageSystem = storageSystem.Object;
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Info = "part to workstation", Quantity = 3, Target = "B01",
                TargetCompartment = "1", Task = AutomationTasks.Transport, Source = "TV01", SourceCompartment = "1", Timestamp = DateTime.UtcNow
            };
            request.Data.Add(new MovementData {Barcode = "expected"});

            var response = _useCase.Execute(request);

            response.Should().NotBeNull();
            response.Status.Should().Be(AutomationStatus.TransportSucceeded);
            response.Quantity.Should().Be(3);
            response.Data.First().Barcode.Should().Be("expected");
            response.Ticket.Should().Be(request.Ticket);
            response.Source.Should().Be(request.Source);
            response.SourceCompartment.Should().Be(request.SourceCompartment);
            response.Target.Should().Be(request.Target);
            response.TargetCompartment.Should().Be(request.TargetCompartment);
            request.Timestamp.Should().BeBefore(DateTime.UtcNow);
        }

        [Fact]
        public void AnalyseMovementToStoreShouldAddPartsToStore()
        {
            var store = new Store {Name = "B01"};
            store.Shelves.Add(new Shelf {Number = "1"});
            var stores = new List<Store> {store};
            var storagePoint = new StoragePoint{Name = "TV01"};
            var storagePoints = new List<StoragePoint>{storagePoint};
            var storageSystem = new Mock<IStorageSystem>();
            storageSystem.SetupGet(s => s.Stores).Returns(stores);
            storageSystem.SetupGet((s => s.StoragePoints)).Returns(storagePoints);
            _useCase.StorageSystem = storageSystem.Object;
            storageSystem.Setup(s => s.AddPartToShelf(It.IsAny<Shelf>(), It.IsAny<Part>()))
                .Callback<Shelf, Part>(((shelf1, part) => shelf1.Parts.Add(part)));
            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Info = "part to store", Quantity = 3, Target = "B01",
                TargetCompartment = "1", Task = AutomationTasks.Transport, Source = "TV01", SourceCompartment = "1", Timestamp = DateTime.UtcNow
            };
            request.Data.Add(new MovementData{Barcode = "barcode1", Index = "1"});
            request.Data.Add(new MovementData{Barcode = "barcode2", Index = "2"});

            _useCase.Execute(request);

            var shelf = store.Shelves.First();
            shelf.Parts.Count.Should().Be(3);
            shelf.Parts[0].Barcode.Should().Be("barcode1");
            shelf.Parts[0].Position.Should().Be(0);
            shelf.Parts[1].Barcode.Should().Be("barcode2");
            shelf.Parts[1].Position.Should().Be(1);
            shelf.Parts[2].Barcode.Should().Be("Part on position 2");
            shelf.Parts[2].Position.Should().Be(2);
        }

        [Fact]
        public void AnanlyseMovementToWorkstationShouldRemovePartsFromStore()
        {
            var store = new Store {Name = "B01"};
            var shelf = new Shelf {Number = "1"};
            for (int i = 0; i < 4; i++)
            {
                shelf.Parts.Add(new Part{Position = i});
            }
            store.Shelves.Add(shelf);
            var stores = new List<Store> {store};
            var storagePoint = new StoragePoint{Name = "AV01"};
            var storagePoints = new List<StoragePoint>{storagePoint};
            var storageSystem = new Mock<IStorageSystem>();
            storageSystem.SetupGet(s => s.Stores).Returns(stores);
            storageSystem.SetupGet((s => s.StoragePoints)).Returns(storagePoints);
            storageSystem.Setup(s => s.RemovePartFromShelf(It.IsAny<Shelf>(), It.IsAny<Part>()))
                .Callback<Shelf, Part>(((shelf1, part) => shelf1.Parts.Remove(part)));
            storageSystem.Setup(s => s.AddPartToStoragePoint(It.IsAny<StoragePoint>(), It.IsAny<Part>()))
                .Callback<StoragePoint, Part>(((sp, part) => sp.Parts.Add(part)));
            _useCase.StorageSystem = storageSystem.Object;

            var request = new MovementRequest
            {
                Ticket = Guid.NewGuid(), Info = "part to workstation", Quantity = 3, Target = "AV01",
                TargetCompartment = "1", Task = AutomationTasks.Transport, Source = "B01", SourceCompartment = "1", Timestamp = DateTime.UtcNow
            };

            _useCase.Execute(request);

            shelf.Parts.Count.Should().Be(1);
            shelf.Parts[0].Position.Should().Be(0);
            storagePoint.Parts.Count.Should().Be(3);
        }
    }
}
