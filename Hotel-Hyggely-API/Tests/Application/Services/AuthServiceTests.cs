using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Requests;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Moq;

namespace Tests.Application.Services
{
	public class AuthServiceTests
	{
		private readonly Mock<IStaffRepo> mockStaffRepo;
		private readonly Mock<IJwtService> mockJwtService;
		private readonly Mock<IPasswordHasher> mockPasswordHasher;
		private readonly AuthService authService;

		public AuthServiceTests()
		{
			mockStaffRepo = new Mock<IStaffRepo>();
			mockJwtService = new Mock<IJwtService>();
			mockPasswordHasher = new Mock<IPasswordHasher>();
			authService = new AuthService(mockJwtService.Object, mockStaffRepo.Object, mockPasswordHasher.Object);
		}

		[Fact]
		public async Task AuthenticateAsync_ValidCredentials_ReturnsToken()
		{
			// Arrange
			var staff = new Staff
			{
				UserName = "userName",
				Password = "password",
				Role = StaffRole.Admin,
				FullName = "Test User"
			};

			mockStaffRepo.Setup(repo => repo.GetByUserNameAsync("userName"))
				.ReturnsAsync(staff);

			mockPasswordHasher.Setup(hasher => hasher.VerifyPassword("password", staff.Password))
				.Returns(true);

			mockJwtService.Setup(jwt => jwt.GenerateToken(staff))
				.Returns("valid_token");

			var loginRequest = new LoginRequest
			{
				UserName = "userName",
				Password = "password"
			};

			// Act
			var token = await authService.AuthenticateAsync(loginRequest);

			// Assert
			Assert.NotNull(token);
			Assert.Equal("valid_token", token);
		}

		[Fact]
		public async Task AuthenticateAsync_InvalidCredentials_ReturnsNull()
		{
			// Arrange
			mockStaffRepo.Setup(repo => repo.GetByUserNameAsync("invalidUser"))
				.ReturnsAsync((Staff?)null);

			var loginRequest = new LoginRequest
			{
				UserName = "invalidUser",
				Password = "wrongPassword"
			};

			// Act
			var token = await authService.AuthenticateAsync(loginRequest);

			// Assert
			Assert.Null(token);
		}

		[Fact]
		public async Task AuthenticateAsync_WrongPassword_ReturnsNull()
		{
			// Arrange
			var staff = new Staff
			{
				UserName = "userName",
				Password = "correctPassword",
				Role = StaffRole.Admin,
				FullName = "Test User"
			};

			mockStaffRepo.Setup(repo => repo.GetByUserNameAsync("userName"))
				.ReturnsAsync(staff);

			mockPasswordHasher.Setup(hasher => hasher.VerifyPassword("wrongPassword", staff.Password))
				.Returns(false);

			var loginRequest = new LoginRequest
			{
				UserName = "userName",
				Password = "wrongPassword"
			};

			// Act
			var token = await authService.AuthenticateAsync(loginRequest);

			// Assert
			Assert.Null(token);
		}

		[Fact]
		public async Task AuthenticateAsync_StaffRepoThrowsException_ThrowsException()
		{
			// Arrange
			mockStaffRepo.Setup(repo => repo.GetByUserNameAsync(It.IsAny<string>()))
				.ThrowsAsync(new Exception("Database error"));

			var loginRequest = new LoginRequest
			{
				UserName = "userName",
				Password = "password"
			};

			// Act & Assert
			await Assert.ThrowsAsync<Exception>(() => authService.AuthenticateAsync(loginRequest));
		}
	}
}