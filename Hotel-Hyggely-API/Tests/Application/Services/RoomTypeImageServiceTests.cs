using Application.Dtos.RoomType;
using Application.Interfaces.Repositories;
using Application.Requests.RoomTypeImage;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Tests.Application.Services
{
    public class RoomTypeImageServiceTests
    {
        private readonly RoomTypeImageService roomTypeImageService;

        private readonly Mock<IRoomTypeImageRepo> roomTypeImageRepoMock;
        private readonly Mock<IMapper> mapperMock;
        public RoomTypeImageServiceTests()
        {
            roomTypeImageRepoMock = new Mock<IRoomTypeImageRepo>();
            mapperMock = new Mock<IMapper>();

            roomTypeImageService = new RoomTypeImageService(roomTypeImageRepoMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedRoomTypeImageDtos()
        {
            // Arrange
            var roomTypeImages = new List<RoomTypeImage>
            {
                new RoomTypeImage { Id = 1, RoomTypeId = 1, FilePath = "/images/roomtypes/image1.jpg", UploadedAt = DateTime.UtcNow, FileType = "jpg" },
                new RoomTypeImage { Id = 2, RoomTypeId = 1, FilePath = "/images/roomtypes/image2.jpg", UploadedAt = DateTime.UtcNow, FileType = "jpg" }
            };

            roomTypeImageRepoMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(roomTypeImages);

            var roomTypeImageDtos = new List<RoomTypeImageDto>
            {
                new RoomTypeImageDto { Id = 1, RoomTypeId = 1, FilePath = "/images/roomtypes/image1.jpg" },
                new RoomTypeImageDto { Id = 2, RoomTypeId = 1, FilePath = "/images/roomtypes/image2.jpg" }
            };

            mapperMock.Setup(m => m.Map<IEnumerable<RoomTypeImageDto>>(roomTypeImages))
                .Returns(roomTypeImageDtos);

            // Act
            var result = await roomTypeImageService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(roomTypeImageDtos, result);
        }

        [Fact]
        public async Task GetAllAsync_NoImages_ReturnsEmptyList()
        {
            // Arrange
            var roomTypeImages = new List<RoomTypeImage>();

            roomTypeImageRepoMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(roomTypeImages);

            var roomTypeImageDtos = new List<RoomTypeImageDto>();

            mapperMock.Setup(m => m.Map<IEnumerable<RoomTypeImageDto>>(roomTypeImages))
                .Returns(roomTypeImageDtos);

            // Act
            var result = await roomTypeImageService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_RepoThrows_ThrowsExeption()
        {
            // Arrange
            roomTypeImageRepoMock.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await roomTypeImageService.GetAllAsync());
        }

        [Fact]
        public async Task GetByRoomTypeIdAsync_ReturnsMappedRoomTypeImageDtos()
        {
            // Arrange
            int roomTypeId = 1;

            var roomTypeImages = new List<RoomTypeImage>
            {
                new RoomTypeImage { Id = 1, RoomTypeId = roomTypeId, FilePath = "/images/roomtypes/image1.jpg", UploadedAt = DateTime.UtcNow, FileType = "jpg" },
                new RoomTypeImage { Id = 2, RoomTypeId = roomTypeId, FilePath = "/images/roomtypes/image2.jpg", UploadedAt = DateTime.UtcNow, FileType = "jpg" }
            };

            roomTypeImageRepoMock.Setup(repo => repo.GetByRoomTypeIdAsync(roomTypeId))
                .ReturnsAsync(roomTypeImages);

            var roomTypeImageDtos = new List<RoomTypeImageDto>
            {
                new RoomTypeImageDto { Id = 1, RoomTypeId = roomTypeId, FilePath = "/images/roomtypes/image1.jpg" },
                new RoomTypeImageDto { Id = 2, RoomTypeId = roomTypeId, FilePath = "/images/roomtypes/image2.jpg" }
            };

            mapperMock.Setup(m => m.Map<IEnumerable<RoomTypeImageDto>>(roomTypeImages))
                .Returns(roomTypeImageDtos);

            // Act
            var result = await roomTypeImageService.GetByRoomTypeIdAsync(roomTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(roomTypeImageDtos, result);
        }

        [Fact]
        public async Task GetByRoomTypeIdAsync_NoImages_ReturnsEmptyList()
        {
            // Arrange
            int roomTypeId = 1;

            var roomTypeImages = new List<RoomTypeImage>();
            roomTypeImageRepoMock.Setup(repo => repo.GetByRoomTypeIdAsync(roomTypeId))
                .ReturnsAsync(roomTypeImages);

            var roomTypeImageDtos = new List<RoomTypeImageDto>();

            mapperMock.Setup(m => m.Map<IEnumerable<RoomTypeImageDto>>(roomTypeImages))
                .Returns(roomTypeImageDtos);

            // Act
            var result = await roomTypeImageService.GetByRoomTypeIdAsync(roomTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByRoomTypeIdAsync_RepoThrows_ThrowsExeption()
        {
            // Arrange
            int roomTypeId = 1;

            roomTypeImageRepoMock.Setup(repo => repo.GetByRoomTypeIdAsync(roomTypeId))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await roomTypeImageService.GetByRoomTypeIdAsync(roomTypeId));
        }

        [Fact]
        public async Task DeleteAsync_ImageExists_ReturnsTrue()
        {
            // Arrange
            int imageId = 1;

            var roomTypeImage = new RoomTypeImage
            {
                Id = imageId,
                RoomTypeId = 1,
                FilePath = "/images/roomtypes/image1.jpg",
                UploadedAt = DateTime.UtcNow,
                FileType = "jpg"
            };

            roomTypeImageRepoMock.Setup(repo => repo.GetByIdAsync(imageId))
                .ReturnsAsync(roomTypeImage);

            roomTypeImageRepoMock.Setup(repo => repo.DeleteAsync(roomTypeImage.Id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await roomTypeImageService.DeleteAsync(imageId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ImageDoesNotExist_ReturnsFalse()
        {
            // Arrange
            int imageId = 1;

            roomTypeImageRepoMock.Setup(repo => repo.GetByIdAsync(imageId))
                .ReturnsAsync((RoomTypeImage?)null);

            // Act
            var result = await roomTypeImageService.DeleteAsync(imageId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_RepoThrows_ThrowsExeption()
        {
            // Arrange
            int imageId = 1;

            roomTypeImageRepoMock.Setup(repo => repo.GetByIdAsync(imageId))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await roomTypeImageService.DeleteAsync(imageId));
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsMappedRoomTypeImageDto()
        {
            // Arrange
            var createRequest = new CreateImageRequest
            {
                RoomTypeId = 1,
                ImgData = new byte[] { 0x1, 0x2, 0x3 },
                FileType = "jpg"
            };

            var roomTypeImage = new RoomTypeImage
            {
                Id = 1,
                RoomTypeId = createRequest.RoomTypeId,
                FilePath = "/images/roomtypes/roomtype_1_guid.jpg",
                UploadedAt = DateTime.UtcNow,
                FileType = createRequest.FileType
            };

            roomTypeImageRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<RoomTypeImage>()))
                .ReturnsAsync(roomTypeImage);

            var roomTypeImageDto = new RoomTypeImageDto
            {
                Id = roomTypeImage.Id,
                RoomTypeId = roomTypeImage.RoomTypeId,
                FilePath = roomTypeImage.FilePath
            };

            mapperMock.Setup(m => m.Map<RoomTypeImage>(createRequest))
                .Returns(roomTypeImage);

            mapperMock.Setup(m => m.Map<RoomTypeImageDto>(roomTypeImage))
                .Returns(roomTypeImageDto);

            // Act
            var result = await roomTypeImageService.CreateAsync(createRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(roomTypeImageDto, result);
        }

        [Fact]
        public async Task CreateAsync_RepoThrows_ThrowsExeption()
        {
            // Arrange
            var createRequest = new CreateImageRequest
            {
                RoomTypeId = 1,
                ImgData = new byte[] { 0x1, 0x2, 0x3 },
                FileType = "jpg"
            };

            var roomTypeImage = new RoomTypeImage
            {
                Id = 1,
                RoomTypeId = createRequest.RoomTypeId,
                FilePath = "/images/roomtypes/roomtype_1_guid.jpg",
                UploadedAt = DateTime.UtcNow,
                FileType = createRequest.FileType
            };

            mapperMock.Setup(m => m.Map<RoomTypeImage>(createRequest))
                .Returns(roomTypeImage);

            roomTypeImageRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<RoomTypeImage>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await roomTypeImageService.CreateAsync(createRequest));
        }
    }
}
