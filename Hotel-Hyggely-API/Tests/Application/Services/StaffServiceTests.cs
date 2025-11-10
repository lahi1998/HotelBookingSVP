using Application.Dtos.Staff;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Requests.Staff;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Moq;

namespace Tests.Application.Services
{
    public class StaffServiceTests
    {
        private readonly StaffService staffService;

        private readonly Mock<IStaffRepo> staffRepoMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IPasswordHasher> passwordHasherMock;

        public StaffServiceTests()
        {
            staffRepoMock = new Mock<IStaffRepo>();
            mapperMock = new Mock<IMapper>();
            passwordHasherMock = new Mock<IPasswordHasher>();

            staffService = new StaffService(staffRepoMock.Object, mapperMock.Object, passwordHasherMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedStaff()
        {
            // Arrange
            var staffList = new List<Staff>
            {
                new Staff { ID = 1, UserName = "Test", FullName = "", Password = "", Role = StaffRole.Admin },
                new Staff { ID = 2, UserName = "Test1", FullName = "", Password = "", Role = StaffRole.Admin }
            };

            var staffDtos = new List<StaffDto>
            {
                new StaffDto { Id = 1, UserName = "Test", FullName = "", Role = "Admin" },
                new StaffDto { Id = 2, UserName = "Test1", FullName = "", Role = "Admin" }
            };

            staffRepoMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(staffList);

            mapperMock.Setup(m => m.Map<IEnumerable<StaffDto>>(staffList))
                .Returns(staffDtos);

            // Act
            var result = await staffService.GetAllAsync();

            // Assert
            Assert.Equal(staffDtos, result);
        }

        [Fact]
        public async Task GetAllAsync_NoStaff_ShouldReturnEmptyList()
        {
            // Arrange
            var staffList = new List<Staff>();
            var staffDtos = new List<StaffDto>();

            staffRepoMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(staffList);

            mapperMock.Setup(m => m.Map<IEnumerable<StaffDto>>(staffList))
                .Returns(staffDtos);
            // Act
            var result = await staffService.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_RepoThrowsException_ShouldThrowExeption()
        {
            // Arrange
            staffRepoMock.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await staffService.GetAllAsync());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedStaff()
        {
            // Arrange
            var staff = new Staff { ID = 1, UserName = "Test", FullName = "", Password = "", Role = StaffRole.Admin };
            var staffDto = new StaffDto { Id = 1, UserName = "Test1", FullName = "", Role = "Admin" };

            staffRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(staff);

            mapperMock.Setup(m => m.Map<StaffDto>(staff))
                .Returns(staffDto);

            // Act
            var result = await staffService.GetByIdAsync(1);

            // Assert
            Assert.Equal(staffDto, result);
        }

        [Fact]
        public async Task GetByIdAsync_StaffNotFound_ShouldReturnNull()
        {
            // Arrange
            staffRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Staff?)null);

            // Act
            var result = await staffService.GetByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_RepoThrowsException_ShouldThrowExeption()
        {
            // Arrange
            staffRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await staffService.GetByIdAsync(1));
        }

        [Fact]
        public async Task CreateAsync_ShouldHashPasswordAndReturnMappedStaff()
        {
            // Arrange
            var createRequest = new CreateStaffRequest
            {
                UserName = "Test",
                FullName = "Test",
                Password = "password123",
                Role = "Admin"
            };

            var hashedPassword = "hashed_password";

            var staffEntity = new Staff
            {
                ID = 1,
                UserName = "Test",
                FullName = "Test",
                Password = hashedPassword,
                Role = StaffRole.Admin
            };

            var staffDto = new StaffDto
            {
                Id = 1,
                UserName = "Test",
                FullName = "Test",
                Role = "Admin"
            };

            passwordHasherMock.Setup(ph => ph.HashPassword("password123"))
                .Returns(hashedPassword);

            mapperMock.Setup(m => m.Map<Staff>(createRequest))
                .Returns(new Staff
                {
                    UserName = "Test",
                    FullName = "Test",
                    Password = hashedPassword,
                    Role = StaffRole.Admin
                });

            staffRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<Staff>()))
                .ReturnsAsync(staffEntity);

            mapperMock.Setup(m => m.Map<StaffDto>(staffEntity))
                .Returns(staffDto);

            // Act
            var result = await staffService.CreateAsync(createRequest);

            // Assert
            Assert.Equal(staffDto, result);
            passwordHasherMock.Verify(ph => ph.HashPassword("password123"), Times.Once);

        }

        [Fact]
        public async Task CreateAsync_RepoThrowsException_ShouldThrowExeption()
        {
            // Arrange
            var createRequest = new CreateStaffRequest
            {
                UserName = "Test",
                FullName = "Test",
                Password = "password123",
                Role = "Admin"
            };

            passwordHasherMock.Setup(ph => ph.HashPassword("password123"))
                .Returns("hashed_password");

            mapperMock.Setup(m => m.Map<Staff>(createRequest))
                .Returns(new Staff
                {
                    UserName = "Test",
                    FullName = "Test",
                    Password = "hashed_password",
                    Role = StaffRole.Admin
                });

            staffRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<Staff>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await staffService.CreateAsync(createRequest));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateStaffAndReturnMappedStaff()
        {
            // Arrange
            var updateRequest = new UpdateStaffRequest
            {
                Id = 1,
                UserName = "UpdatedUser",
                FullName = "Updated Name",
                Password = "newpassword123",
                Role = "Admin"
            };

            var existingStaff = new Staff
            {
                ID = 1,
                UserName = "OldUser",
                FullName = "Old Name",
                Password = "oldhashedpassword",
                Role = StaffRole.Cleaning
            };

            var hashedPassword = "newhashedpassword";

            var updatedStaff = new Staff
            {
                ID = 1,
                UserName = "UpdatedUser",
                FullName = "Updated Name",
                Password = hashedPassword,
                Role = StaffRole.Admin
            };

            var staffDto = new StaffDto
            {
                Id = 1,
                UserName = "UpdatedUser",
                FullName = "Updated Name",
                Role = "Admin"
            };

            staffRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingStaff);

            passwordHasherMock.Setup(ph => ph.HashPassword("newpassword123"))
                .Returns(hashedPassword);

            staffRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Staff>()))
                .ReturnsAsync(updatedStaff);

            mapperMock.Setup(m => m.Map<StaffDto>(updatedStaff))
                .Returns(staffDto);

            // Act
            var result = await staffService.UpdateAsync(updateRequest);

            // Assert
            Assert.Equal(staffDto, result);
            passwordHasherMock.Verify(ph => ph.HashPassword("newpassword123"), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_StaffNotFound_ShouldReturnNull()
        {
            // Arrange
            var updateRequest = new UpdateStaffRequest
            {
                Id = 1,
                UserName = "UpdatedUser",
                FullName = "Updated Name",
                Password = "newpassword123",
                Role = "Admin"
            };

            staffRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Staff?)null);

            // Act
            var result = await staffService.UpdateAsync(updateRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_RepoThrowsException_ShouldThrowExeption()
        {
            // Arrange
            var updateRequest = new UpdateStaffRequest
            {
                Id = 1,
                UserName = "UpdatedUser",
                FullName = "Updated Name",
                Password = "newpassword123",
                Role = "Admin"
            };

            staffRepoMock.Setup(repo => repo.GetByIdAsync(1))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await staffService.UpdateAsync(updateRequest));
        }

        [Fact]
        public async Task DeleteAsync_RepoThrowsException_ShouldThrowExeption()
        {
            // Arrange
            staffRepoMock.Setup(repo => repo.DeleteAsync(1))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await staffService.DeleteAsync(1));
        }
    }
}
