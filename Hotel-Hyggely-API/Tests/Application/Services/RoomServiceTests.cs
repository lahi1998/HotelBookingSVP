using Application.Dtos.Room;
using Application.Interfaces.Repositories;
using Application.Requests.Room;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Tests.Application.Services
{
    public class RoomServiceTests
    {
        private readonly RoomService roomService;

        private readonly Mock<IRoomRepo> roomRepoMock;
        private readonly Mock<IRoomTypeRepo> roomTypeRepoMock;
        private readonly Mock<IMapper> mapperMock;

        public RoomServiceTests()
        {
            roomRepoMock = new Mock<IRoomRepo>();
            roomTypeRepoMock = new Mock<IRoomTypeRepo>();
            mapperMock = new Mock<IMapper>();

            roomService = new RoomService(roomRepoMock.Object, mapperMock.Object, roomTypeRepoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedRoomDtos()
        {
            // Arrange
            var rooms = new List<Room>
            {
                new Room { ID = 1 },
                new Room { ID = 2 }
            };

            var roomDtos = new List<RoomDto>
            {
                new RoomDto { ID = 1, RoomStatus = "available", RoomTypeName = "enkelt" },
                new RoomDto { ID = 2, RoomStatus = "available", RoomTypeName = "enkelt" }
            };

            roomRepoMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(rooms);

            mapperMock.Setup(m => m.Map<IEnumerable<RoomDto>>(rooms))
                .Returns(roomDtos);

            // Act
            var result = await roomService.GetAllAsync();

            // Assert
            Assert.Equal(roomDtos, result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_NoRooms_ReturnsEmptyList()
        {
            // Arrange
            var rooms = new List<Room>();

            roomRepoMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(rooms);

            mapperMock.Setup(m => m.Map<IEnumerable<RoomDto>>(rooms))
                .Returns(new List<RoomDto>());

            // Act
            var result = await roomService.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_RepositoryThrowsException_ThrowsExeption()
        {
            // Arrange
            roomRepoMock.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await roomService.GetAllAsync());
        }

        [Fact]
        public async Task GetAvailableByPeriod_ReturnsAvailableByPeriod()
        {
            // Act
            var fromDate = new DateTime(2024, 7, 1);
            var toDate = new DateTime(2024, 7, 10);

            var availableRooms = new List<Room>
            {
                new Room { ID = 1 },
                new Room { ID = 2 }
            };

            var availableRoomDtos = new List<AvailableRoomDto>
            {
                new AvailableRoomDto { ID = 1 },
                new AvailableRoomDto { ID = 2 }
            };

            roomRepoMock.Setup(repo => repo.GetAvailableByPeriod(fromDate, toDate))
                .ReturnsAsync(availableRooms);

            mapperMock.Setup(m => m.Map<IEnumerable<AvailableRoomDto>>(availableRooms))
                .Returns(availableRoomDtos);

            // Act
            var result = await roomService.GetAvailableByPeriod(fromDate, toDate);

            // Assert
            Assert.Equal(availableRoomDtos, result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAvailableByPeriod_NoAvailableRooms_ReturnsEmptyList()
        {
            // Arrange
            var fromDate = new DateTime(2024, 7, 1);
            var toDate = new DateTime(2024, 7, 10);
            var availableRooms = new List<Room>();

            roomRepoMock.Setup(repo => repo.GetAvailableByPeriod(fromDate, toDate))
                .ReturnsAsync(availableRooms);

            mapperMock.Setup(m => m.Map<IEnumerable<AvailableRoomDto>>(availableRooms))
                .Returns(new List<AvailableRoomDto>());

            // Act
            var result = await roomService.GetAvailableByPeriod(fromDate, toDate);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAvailableByPeriod_RepositoryThrowsException_ThrowsExeption()
        {
            // Arrange
            var fromDate = new DateTime(2024, 7, 1);
            var toDate = new DateTime(2024, 7, 10);

            roomRepoMock.Setup(repo => repo.GetAvailableByPeriod(fromDate, toDate))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await roomService.GetAvailableByPeriod(fromDate, toDate));
        }

        [Fact]
        public async Task GetAvailableByPeriodWithDetails_ReturnsAvailableByPeriod()
        {
            // Act
            var fromDate = new DateTime(2024, 7, 1);
            var toDate = new DateTime(2024, 7, 10);

            var availableRooms = new List<Room>
            {
                new Room { ID = 1 },
                new Room { ID = 2 }
            };

            var availableRoomDtos = new List<AvailableRoomDto>
            {
                new AvailableRoomDto { ID = 1 },
                new AvailableRoomDto { ID = 2 }
            };

            roomRepoMock.Setup(repo => repo.GetAvailableByPeriod(fromDate, toDate))
                .ReturnsAsync(availableRooms);

            mapperMock.Setup(m => m.Map<IEnumerable<AvailableRoomDto>>(availableRooms))
                .Returns(availableRoomDtos);

            // Act
            var result = await roomService.GetAvailableByPeriod(fromDate, toDate);

            // Assert
            Assert.Equal(availableRoomDtos, result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAvailableByPeriodWithDetails_NoAvailableRooms_ReturnsEmptyList()
        {
            // Arrange
            var fromDate = new DateTime(2024, 7, 1);
            var toDate = new DateTime(2024, 7, 10);
            var availableRooms = new List<Room>();

            roomRepoMock.Setup(repo => repo.GetAvailableByPeriod(fromDate, toDate))
                .ReturnsAsync(availableRooms);

            mapperMock.Setup(m => m.Map<IEnumerable<AvailableRoomDto>>(availableRooms))
                .Returns(new List<AvailableRoomDto>());

            // Act
            var result = await roomService.GetAvailableByPeriod(fromDate, toDate);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAvailableByPeriodWithDetails_RepositoryThrowsException_ThrowsExeption()
        {
            // Arrange
            var fromDate = new DateTime(2024, 7, 1);
            var toDate = new DateTime(2024, 7, 10);

            roomRepoMock.Setup(repo => repo.GetAvailableByPeriod(fromDate, toDate))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await roomService.GetAvailableByPeriod(fromDate, toDate));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedRoomDto()
        {
            // Arrange
            var room = new Room { ID = 1 };
            var roomDto = new RoomDto { ID = 1, RoomStatus = "available", RoomTypeName = "enkelt" };

            roomRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(room);

            mapperMock.Setup(m => m.Map<RoomDto?>(room))
                .Returns(roomDto);
            // Act
            var result = await roomService.GetByIdAsync(1);

            // Assert
            Assert.Equal(roomDto, result);
        }

        [Fact]
        public async Task GetByIdAsync_RoomNotFound_ReturnsNull()
        {
            // Arrange
            roomRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Room?)null);

            mapperMock.Setup(m => m.Map<RoomDto?>(null))
                .Returns((RoomDto?)null);

            // Act
            var result = await roomService.GetByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_RepositoryThrowsException_ThrowsExeption()
        {
            // Arrange
            roomRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await roomService.GetByIdAsync(1));
        }

        [Fact]
        public async Task CreateAsync_ReturnsCreatedRoomDto()
        {
            // Arrange
            var createRequest = new CreateRoomRequest
            {
                Number = 101,
                RoomTypeName = "enkelt"
            };
            var roomType = new RoomType { ID = 1, Name = "enkelt" };

            var roomToCreate = new Room
            {
                Number = 101,
                RoomTypeId = roomType.ID
            };

            var createdRoom = new Room
            {
                ID = 1,
                Number = 101,
                RoomTypeId = roomType.ID
            };

            var createdRoomDto = new RoomDto
            {
                ID = 1,
                Number = 101,
                RoomStatus = "available",
                RoomTypeName = "enkelt"
            };

            mapperMock.Setup(m => m.Map<Room>(createRequest))
                .Returns(roomToCreate);

            roomTypeRepoMock.Setup(repo => repo.GetByNameAsync("enkelt"))
                .ReturnsAsync(roomType);

            roomRepoMock.Setup(repo => repo.CreateAsync(roomToCreate))
                .ReturnsAsync(createdRoom);

            mapperMock.Setup(m => m.Map<RoomDto>(createdRoom))
                .Returns(createdRoomDto);

            // Act
            var result = await roomService.CreateAsync(createRequest);

            // Assert
            Assert.Equal(createdRoomDto, result);
        }

        [Fact]
        public async Task CreateAsync_RepositoryThrowsException_ThrowsExeption()
        {
            // Arrange
            var createRequest = new CreateRoomRequest
            {
                Number = 101,
                RoomTypeName = "enkelt"
            };

            mapperMock.Setup(m => m.Map<Room>(createRequest))
                .Throws(new Exception("Mapping error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await roomService.CreateAsync(createRequest));
        }

        [Fact]
        public async Task UpdateAsync_RoomNotFound_ReturnsNull()
        {
            // Arrange
            var updateRequest = new UpdateRoomRequest
            {
                Id = 1,
                Number = 102,
                RoomTypeName = "dobbelt"
            };

            roomRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Room?)null);

            // Act
            var result = await roomService.UpdateAsync(updateRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_RepositoryThrowsException_ThrowsExeption()
        {
            // Arrange
            var updateRequest = new UpdateRoomRequest
            {
                Id = 1,
                Number = 102,
                RoomTypeName = "dobbelt"
            };

            roomRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await roomService.UpdateAsync(updateRequest));
        }

        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedRoomDto()
        {
            // Arrange
            var updateRequest = new UpdateRoomRequest
            {
                Id = 1,
                Number = 102,
                RoomTypeName = "dobbelt"
            };

            var existingRoom = new Room
            {
                ID = 1,
                Number = 101,
                RoomTypeId = 1
            };

            var updatedRoom = new Room
            {
                ID = 1,
                Number = 102,
                RoomTypeId = 2
            };

            var updatedRoomDto = new RoomDto
            {
                ID = 1,
                Number = 102,
                RoomStatus = "available",
                RoomTypeName = "dobbelt"
            };

            roomRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingRoom);

            mapperMock.Setup(m => m.Map(updateRequest, existingRoom))
                .Returns(existingRoom);

            roomTypeRepoMock.Setup(repo => repo.GetByNameAsync("dobbelt"))
                .ReturnsAsync(new RoomType { ID = 2, Name = "dobbelt" });

            roomRepoMock.Setup(repo => repo.UpdateAsync(existingRoom))
                .ReturnsAsync(updatedRoom);

            mapperMock.Setup(m => m.Map<RoomDto>(updatedRoom))
                .Returns(updatedRoomDto);

            // Act
            var result = await roomService.UpdateAsync(updateRequest);

            // Assert
            Assert.Equal(updatedRoomDto, result);
        }

        [Fact]
        public async Task DeleteAsync_RoomNotFound_ReturnsFalse()
        {
            // Arrange
            roomRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Room?)null);

            // Act
            var result = await roomService.DeleteAsync(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_RepositoryThrowsException_ThrowsExeption()
        {
            // Arrange
            roomRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ThrowsAsync(new Exception("Database error"));
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await roomService.DeleteAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_RoomExists_ReturnsTrue()
        {
            // Arrange
            var existingRoom = new Room
            {
                ID = 1,
                Number = 101,
                RoomTypeId = 1
            };

            roomRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingRoom);

            roomRepoMock.Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await roomService.DeleteAsync(1);

            // Assert
            Assert.True(result);
        }
    }
}