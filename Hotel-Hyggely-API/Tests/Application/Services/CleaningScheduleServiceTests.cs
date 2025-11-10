using Application.Dtos.CleaningSchedule;
using Application.Interfaces.Repositories;
using Application.Requests.CleaningSchedule;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Moq;
using System.Security.AccessControl;

namespace Tests.Application.Services
{
    public class CleaningScheduleServiceTests
    {
        private readonly CleaningScheduleService cleaningScheduleService;

        private readonly Mock<ICleaningScheduleRepo> cleaningScheduleRepoMock;
        private readonly Mock<IMapper> mapperMock;
        public CleaningScheduleServiceTests()
        {
            cleaningScheduleRepoMock = new Mock<ICleaningScheduleRepo>();
            mapperMock = new Mock<IMapper>();

            cleaningScheduleService = new CleaningScheduleService(cleaningScheduleRepoMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task GetByBookingIdAsync_ReturnsCleaningSchedules()
        {
            // Arrange
            int bookingId = 1;

            var cleaningSchedules = new List<CleaningSchedule>
            {
                new CleaningSchedule { ID = 1, RoomId = 101, Cleaned = false },
                new CleaningSchedule { ID = 2, RoomId = 102, Cleaned = true }
            };

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByBookingIdWithRoomAsync(bookingId))
                .ReturnsAsync(cleaningSchedules);

            var cleaningScheduleDtos = new List<CleaningScheduleDto>
            {
                new CleaningScheduleDto { Id = 1, Cleaned = false },
                new CleaningScheduleDto { Id = 2, Cleaned = true }
            };

            mapperMock
                .Setup(m => m.Map<IEnumerable<CleaningScheduleDto>>(cleaningSchedules))
                .Returns(cleaningScheduleDtos);

            // Act
            var result = await cleaningScheduleService.GetByBookingIdAsync(bookingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(cleaningScheduleDtos, result);
        }

        [Fact]
        public async Task GetByBookingIdAsync_NoCleaningSchedules_ReturnsEmptyList()
        {
            // Arrange
            int bookingId = 1;

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByBookingIdWithRoomAsync(bookingId))
                .ReturnsAsync(new List<CleaningSchedule>());

            mapperMock
                .Setup(m => m.Map<IEnumerable<CleaningScheduleDto>>(It.IsAny<IEnumerable<CleaningSchedule>>()))
                .Returns(new List<CleaningScheduleDto>());

            // Act
            var result = await cleaningScheduleService.GetByBookingIdAsync(bookingId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByBookingIdAsync_RepoThrowsException_ThrowsException()
        {
            // Arrange
            int bookingId = 1;

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByBookingIdWithRoomAsync(bookingId))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => cleaningScheduleService.GetByBookingIdAsync(bookingId));
        }

        [Fact]
        public async Task GetPendingWithRoomAsync_ReturnsCleaningSchedules()
        {
            // Arrange
            var cleaningSchedules = new List<CleaningSchedule>
            {
                new CleaningSchedule { ID = 1, RoomId = 101, Cleaned = false },
                new CleaningSchedule { ID = 2, RoomId = 102, Cleaned = false }
            };

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetPendingWithRoomAsync())
                .ReturnsAsync(cleaningSchedules);

            var cleaningScheduleDtos = new List<CleaningScheduleDto>
            {
                new CleaningScheduleDto { Id = 1, Cleaned = false },
                new CleaningScheduleDto { Id = 2, Cleaned = false }
            };

            mapperMock
                .Setup(m => m.Map<IEnumerable<CleaningScheduleDto>>(cleaningSchedules))
                .Returns(cleaningScheduleDtos);

            // Act
            var result = await cleaningScheduleService.GetPendingWithRoomAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(cleaningScheduleDtos, result);
        }

        [Fact]
        public async Task GetPendingWithRoomAsync_NoCleaningSchedules_ReturnsEmptyList()
        {
            // Arrange
            cleaningScheduleRepoMock
                .Setup(repo => repo.GetPendingWithRoomAsync())
                .ReturnsAsync(new List<CleaningSchedule>());
            mapperMock
                .Setup(m => m.Map<IEnumerable<CleaningScheduleDto>>(It.IsAny<IEnumerable<CleaningSchedule>>()))
                .Returns(new List<CleaningScheduleDto>());
            // Act
            var result = await cleaningScheduleService.GetPendingWithRoomAsync();
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPendingWithRoomAsync_RepoThrowsException_ThrowsException()
        {
            // Arrange
            cleaningScheduleRepoMock
                .Setup(repo => repo.GetPendingWithRoomAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => cleaningScheduleService.GetPendingWithRoomAsync());
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsCleaningSchedule()
        {
            // Arrange
            int cleaningScheduleId = 1;

            var cleaningSchedule = new CleaningSchedule { ID = cleaningScheduleId, RoomId = 101, Cleaned = false };

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByIdAsync(cleaningScheduleId))
                .ReturnsAsync(cleaningSchedule);

            var cleaningScheduleDto = new CleaningScheduleDto { Id = cleaningScheduleId, Cleaned = false };

            mapperMock
                .Setup(m => m.Map<CleaningScheduleDto?>(cleaningSchedule))
                .Returns(cleaningScheduleDto);

            // Act
            var result = await cleaningScheduleService.GetByIdAsync(cleaningScheduleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cleaningScheduleDto, result);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            int cleaningScheduleId = 1;

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByIdAsync(cleaningScheduleId))
                .ReturnsAsync((CleaningSchedule?)null);

            mapperMock
                .Setup(m => m.Map<CleaningScheduleDto?>(It.IsAny<CleaningSchedule>()))
                .Returns((CleaningScheduleDto?)null);

            // Act
            var result = await cleaningScheduleService.GetByIdAsync(cleaningScheduleId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_RepoThrowsException_ThrowsException()
        {
            // Arrange
            int cleaningScheduleId = 1;

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByIdAsync(cleaningScheduleId))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => cleaningScheduleService.GetByIdAsync(cleaningScheduleId));
        }

        [Fact]
        public async Task CreateForRoomAsync_ValidRequest_ReturnsCreatedCleaningSchedule()
        {
            // Arrange
            var createRequest = new CreateCleaningScheduleRequest
            {
                RoomId = 101,
                CleaningDate = DateTime.Now.AddDays(1)
            };

            var cleaningSchedule = new CleaningSchedule
            {
                ID = 1,
                RoomId = createRequest.RoomId,
                CleaningDate = createRequest.CleaningDate,
                Cleaned = false
            };

            cleaningScheduleRepoMock
                .Setup(repo => repo.CreateForRoomAsync(It.IsAny<CleaningSchedule>()))
                .ReturnsAsync(cleaningSchedule);

            var cleaningScheduleDto = new CleaningScheduleDto
            {
                Id = cleaningSchedule.ID,
                Cleaned = cleaningSchedule.Cleaned
            };

            mapperMock
                .Setup(m => m.Map<CleaningSchedule>(createRequest))
                .Returns(cleaningSchedule);

            mapperMock
                .Setup(m => m.Map<CleaningScheduleDto>(cleaningSchedule))
                .Returns(cleaningScheduleDto);

            // Act
            var result = await cleaningScheduleService.CreateForRoomAsync(createRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cleaningScheduleDto, result);
        }

        [Fact]
        public async Task CreateForRoomAsync_RepoThrowsException_ThrowsException()
        {
            // Arrange
            var createRequest = new CreateCleaningScheduleRequest
            {
                RoomId = 101,
                CleaningDate = DateTime.Now.AddDays(1)
            };

            cleaningScheduleRepoMock
                .Setup(repo => repo.CreateForRoomAsync(It.IsAny<CleaningSchedule>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => cleaningScheduleService.CreateForRoomAsync(createRequest));
        }

        [Fact]
        public async Task MarkAsCleaned_CleaningScheduleExists_ReturnsTrue()
        {
            // Arrange
            int cleaningScheduleId = 1;

            var existingCleaningSchedule = new CleaningSchedule
            {
                ID = cleaningScheduleId,
                Room = new Room { ID = 101 },
                Cleaned = false
            };

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByIdWithRoomAsync(cleaningScheduleId))
                .ReturnsAsync(existingCleaningSchedule);

            cleaningScheduleRepoMock
                .Setup(repo => repo.UpdateAsync(existingCleaningSchedule))
                .ReturnsAsync(existingCleaningSchedule);

            // Act
            var result = await cleaningScheduleService.MarkAsCleaned(cleaningScheduleId);

            // Assert
            Assert.True(result);
            Assert.True(existingCleaningSchedule.Cleaned);
        }

        [Fact]
        public async Task MarkAsCleaned_CleaningScheduleDoesNotExist_ReturnsFalse()
        {
            // Arrange
            int cleaningScheduleId = 1;

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByIdWithRoomAsync(cleaningScheduleId))
                .ReturnsAsync((CleaningSchedule?)null);

            // Act
            var result = await cleaningScheduleService.MarkAsCleaned(cleaningScheduleId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task MarkAsCleaned_RepoThrowsException_ThrowsException()
        {
            // Arrange
            int cleaningScheduleId = 1;

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByIdWithRoomAsync(cleaningScheduleId))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => cleaningScheduleService.MarkAsCleaned(cleaningScheduleId));
        }

        [Fact]
        public async Task MarkAsCleaned_UpdateThrowsException_ThrowsException()
        {
            // Arrange
            int cleaningScheduleId = 1;

            var existingCleaningSchedule = new CleaningSchedule
            {
                ID = cleaningScheduleId,
                Room = new Room { ID = 101 },
                Cleaned = false
            };

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByIdWithRoomAsync(cleaningScheduleId))
                .ReturnsAsync(existingCleaningSchedule);

            cleaningScheduleRepoMock
                .Setup(repo => repo.UpdateAsync(existingCleaningSchedule))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => cleaningScheduleService.MarkAsCleaned(cleaningScheduleId));
        }

        [Fact]
        public async Task DeleteAsync_CleaningScheduleExists_ReturnsTrue()
        {
            // Arrange
            int cleaningScheduleId = 1;

            var existingCleaningSchedule = new CleaningSchedule
            {
                ID = cleaningScheduleId,
                RoomId = 101,
                Cleaned = false
            };

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByIdAsync(cleaningScheduleId))
                .ReturnsAsync(existingCleaningSchedule);

            cleaningScheduleRepoMock
                .Setup(repo => repo.DeleteAsync(cleaningScheduleId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await cleaningScheduleService.DeleteAsync(cleaningScheduleId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_CleaningScheduleDoesNotExist_ReturnsFalse()
        {
            // Arrange
            int cleaningScheduleId = 1;

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByIdAsync(cleaningScheduleId))
                .ReturnsAsync((CleaningSchedule?)null);

            // Act
            var result = await cleaningScheduleService.DeleteAsync(cleaningScheduleId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_RepoThrowsException_ThrowsException()
        {
            // Arrange
            int cleaningScheduleId = 1;

            cleaningScheduleRepoMock
                .Setup(repo => repo.GetByIdAsync(cleaningScheduleId))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => cleaningScheduleService.DeleteAsync(cleaningScheduleId));
        }
    }
}
