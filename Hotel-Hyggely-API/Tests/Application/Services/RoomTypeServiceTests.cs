using Application.Dtos.RoomType;
using Application.Interfaces.Repositories;
using Application.Requests.RoomType;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Tests.Application.Services
{
    public class RoomTypeServiceTests
    {
        private readonly RoomTypeService roomTypeService;

        private readonly Mock<IRoomTypeRepo> roomTypeRepoMock;
        private readonly Mock<IMapper> mapperMock;

        public RoomTypeServiceTests()
        {
            roomTypeRepoMock = new Mock<IRoomTypeRepo>();
            mapperMock = new Mock<IMapper>();

            roomTypeService = new RoomTypeService(roomTypeRepoMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedRoomTypes()
        {
            // Arrange
            var roomTypes = new List<RoomType>
            {
                new RoomType { ID = 1, Name = "Single" },
                new RoomType { ID = 2, Name = "Double" }
            };

            var roomTypeDtos = new List<RoomTypeDto>
            {
                new RoomTypeDto { ID = 1, Name = "Single" },
                new RoomTypeDto { ID = 2, Name = "Double" }
            };

            roomTypeRepoMock.Setup(repo => repo.GetAll())
                .ReturnsAsync(roomTypes);

            mapperMock.Setup(m => m.Map<IEnumerable<RoomTypeDto>>(roomTypes))
                .Returns(roomTypeDtos);

            // Act
            var result = await roomTypeService.GetAllAsync();

            // Assert
            Assert.Equal(roomTypeDtos, result);
        }

        [Fact]
        public async Task GetAllAsync_NoRoomTypes_ShouldReturnEmptyList()
        {
            // Arrange
            var roomTypes = new List<RoomType>();
            var roomTypeDtos = new List<RoomTypeDto>();

            roomTypeRepoMock.Setup(repo => repo.GetAll())
                .ReturnsAsync(roomTypes);

            mapperMock.Setup(m => m.Map<IEnumerable<RoomTypeDto>>(roomTypes))
                .Returns(roomTypeDtos);

            // Act
            var result = await roomTypeService.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_RepoThrowsException_ShouldThrowExeption()
        {
            // Arrange
            roomTypeRepoMock.Setup(repo => repo.GetAll())
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => roomTypeService.GetAllAsync());
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ShouldReturnMappedRoomType()
        {
            // Arrange
            var roomType = new RoomType { ID = 1, Name = "Single" };
            var roomTypeDto = new RoomTypeDto { ID = 1, Name = "Single" };

            roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(roomType);

            mapperMock.Setup(m => m.Map<RoomTypeDto?>(roomType))
                .Returns(roomTypeDto);

            // Act
            var result = await roomTypeService.GetByIdAsync(1);

            // Assert
            Assert.Equal(roomTypeDto, result);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ShouldReturnNull()
        {
            // Arrange
            roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(99))
                .ReturnsAsync((RoomType?)null);

            // Act
            var result = await roomTypeService.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_RepoThrowsException_ShouldThrowExeption()
        {
            // Arrange
            roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => roomTypeService.GetByIdAsync(1));
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ShouldReturnMappedRoomType()
        {
            // Arrange
            var createRequest = new CreateRoomTypeRequest
            {
                Name = "Single"
            };

            var roomType = new RoomType { ID = 1, Name = "Single" };
            var createdRoomType = new RoomType { ID = 1, Name = "Single" };
            var roomTypeDto = new RoomTypeDto { ID = 1, Name = "Single" };

            mapperMock.Setup(m => m.Map<RoomType>(createRequest))
                .Returns(roomType);

            roomTypeRepoMock.Setup(repo => repo.CreateAsync(roomType))
                .ReturnsAsync(createdRoomType);

            mapperMock.Setup(m => m.Map<RoomTypeDto>(createdRoomType))
                .Returns(roomTypeDto);

            // Act
            var result = await roomTypeService.CreateAsync(createRequest);

            // Assert
            Assert.Equal(roomTypeDto, result);
        }

        [Fact]
        public async Task CreateAsync_RepoThrowsException_ShouldThrowExeption()
        {
            // Arrange
            var createRequest = new CreateRoomTypeRequest
            {
                Name = "Single"
            };

            var roomType = new RoomType { ID = 1, Name = "Single" };

            mapperMock.Setup(m => m.Map<RoomType>(createRequest))
                .Returns(roomType);

            roomTypeRepoMock.Setup(repo => repo.CreateAsync(roomType))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => roomTypeService.CreateAsync(createRequest));
        }

        [Fact]
        public async Task UpdateAsync_ExistingRoomType_ShouldReturnMappedRoomType()
        {
            // Arrange
            var updateRequest = new UpdateRoomTypeRequest
            {
                Id = 1,
                Name = "Updated Single"
            };

            var existingRoomType = new RoomType { ID = 1, Name = "Single" };
            var updatedRoomType = new RoomType { ID = 1, Name = "Updated Single" };
            var roomTypeDto = new RoomTypeDto { ID = 1, Name = "Updated Single" };

            roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingRoomType);

            mapperMock.Setup(m => m.Map(updateRequest, existingRoomType))
                .Returns(existingRoomType);

            roomTypeRepoMock.Setup(repo => repo.UpdateAsync(existingRoomType))
                .ReturnsAsync(updatedRoomType);

            mapperMock.Setup(m => m.Map<RoomTypeDto>(updatedRoomType))
                .Returns(roomTypeDto);

            // Act
            var result = await roomTypeService.UpdateAsync(updateRequest);

            // Assert
            Assert.Equal(roomTypeDto, result);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingRoomType_ShouldReturnNull()
        {
            // Arrange
            var updateRequest = new UpdateRoomTypeRequest
            {
                Id = 99,
                Name = "Updated Single"
            };

            roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(99))
                .ReturnsAsync((RoomType?)null);

            // Act
            var result = await roomTypeService.UpdateAsync(updateRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_RepoThrowsException_ShouldThrowExeption()
        {
            // Arrange
            var updateRequest = new UpdateRoomTypeRequest
            {
                Id = 1,
                Name = "Updated Single"
            };

            roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => roomTypeService.UpdateAsync(updateRequest));
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_ShouldReturnTrue()
        {
            // Arrange
            roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(new RoomType { ID = 1, Name = "Single" });

            roomTypeRepoMock.Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await roomTypeService.DeleteAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ShouldReturnFalse()
        {
            // Arrange
            roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(99))
                .ReturnsAsync((RoomType?)null);

            // Act
            var result = await roomTypeService.DeleteAsync(99);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_RepoThrowsException_ShouldThrowExeption()
        {
            // Arrange
            roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => roomTypeService.DeleteAsync(1));
        }
    }
}
