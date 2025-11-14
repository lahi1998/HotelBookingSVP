using Application.Dtos.Booking;
using Application.Dtos.Customer;
using Application.Dtos.Room;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Requests.Booking;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Moq;

namespace Tests.Application.Services
{
    public class BookingServiceTests
    {
        private readonly BookingService bookingService;

        private readonly Mock<IBookingRepo> bookingRepoMock;
        private readonly Mock<IRoomRepo> roomRepoMock;
        private readonly Mock<IGuestRepo> guestRepoMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<ICleaningScheduleRepo> cleaningScheduleMock;
        private readonly Mock<IEmailService> emailServiceMock;
        public BookingServiceTests()
        {
            bookingRepoMock = new Mock<IBookingRepo>();
            roomRepoMock = new Mock<IRoomRepo>();
            guestRepoMock = new Mock<IGuestRepo>();
            mapperMock = new Mock<IMapper>();
            cleaningScheduleMock = new Mock<ICleaningScheduleRepo>();
            emailServiceMock = new Mock<IEmailService>();

            bookingService = new BookingService(
                bookingRepoMock.Object,
                guestRepoMock.Object,
                mapperMock.Object,
                roomRepoMock.Object,
                cleaningScheduleMock.Object,
                emailServiceMock.Object);
        }

        [Fact]
        public async Task GetAllBookingsAsync_ReturnsMappedBookingDtos()
        {
            // Arrange
            var customerDto = new GuestDto
            {
                FullName = "John",
                Email = "",
                PhoneNumber = ""
            };

            var bookings = new List<Booking>
            {
                new Booking { ID = 1 },
                new Booking { ID = 2 }
            };

            var bookingListItemDtos = new List<BookingListItemDto>
            {
                new BookingListItemDto { Id = 1, Customer = customerDto },
                new BookingListItemDto { Id = 2, Customer = customerDto }
            };

            bookingRepoMock.Setup(repo => repo.GetAllWithCustomerAndRoomsAsync())
                .ReturnsAsync(bookings);

            mapperMock.Setup(m => m.Map<IEnumerable<BookingListItemDto>>(bookings))
                .Returns(bookingListItemDtos);

            // Act
            var result = await bookingService.GetAllBookingsAsync();

            // Assert
            Assert.Equal(bookingListItemDtos, result);
        }

        [Fact]
        public async Task GetAllBookingsAsync_NoBookings_ReturnsEmptyList()
        {
            // Arrange
            var bookings = new List<Booking>();

            bookingRepoMock.Setup(repo => repo.GetAllWithCustomerAndRoomsAsync())
                .ReturnsAsync(bookings);

            mapperMock.Setup(m => m.Map<IEnumerable<BookingListItemDto>>(bookings))
                .Returns(new List<BookingListItemDto>());

            // Act
            var result = await bookingService.GetAllBookingsAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllBookingsAsync_BookingRepoThrows_ThrowsException()
        {
            // Arrange
            bookingRepoMock.Setup(repo => repo.GetAllWithCustomerAndRoomsAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await bookingService.GetAllBookingsAsync());
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsBooking()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking { ID = bookingId };

            bookingRepoMock.Setup(repo => repo.GetById(bookingId))
                .ReturnsAsync(booking);

            // Act
            var result = await bookingService.GetByIdAsync(bookingId);

            // Assert
            Assert.Equal(booking, result);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var bookingId = 999;

            bookingRepoMock.Setup(repo => repo.GetById(bookingId))
                .ReturnsAsync((Booking?)null);

            // Act
            var result = await bookingService.GetByIdAsync(bookingId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_BookingRepoThrows_ThrowsException()
        {
            // Arrange
            var bookingId = 1;
            bookingRepoMock.Setup(repo => repo.GetById(bookingId))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await bookingService.GetByIdAsync(bookingId));
        }

        [Fact]
        public async Task CheckInAsync_ValidId_UpdatesCheckInStatus()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking { ID = bookingId, CheckInStatus = Domain.Enums.CheckInStatus.NotCheckedIn };

            bookingRepoMock.Setup(repo => repo.GetById(bookingId))
                .ReturnsAsync(booking);

            // Act
            await bookingService.CheckInAsync(bookingId);

            // Assert
            bookingRepoMock.Verify(repo => repo.UpdateAsync(It.Is<Booking>(b => b.CheckInStatus == Domain.Enums.CheckInStatus.CheckedIn)), Times.Once);
        }

        [Fact]
        public async Task CheckInAsync_AlreadyCheckedIn_DoesNotUpdate()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking { ID = bookingId, CheckInStatus = Domain.Enums.CheckInStatus.CheckedIn };

            bookingRepoMock.Setup(repo => repo.GetById(bookingId))
                .ReturnsAsync(booking);

            // Act
            await bookingService.CheckInAsync(bookingId);

            // Assert
            bookingRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public async Task CheckInAsync_BookingRepoThrows_ThrowsException()
        {
            // Arrange
            var bookingId = 1;

            bookingRepoMock.Setup(repo => repo.GetById(bookingId))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await bookingService.CheckInAsync(bookingId));
        }

        [Fact]
        public async Task CheckOutAsync_ValidId_UpdatesCheckInStatus()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking { ID = bookingId, CheckInStatus = Domain.Enums.CheckInStatus.CheckedIn };

            bookingRepoMock.Setup(repo => repo.GetById(bookingId))
                .ReturnsAsync(booking);

            // Act
            await bookingService.CheckOutAsync(bookingId);

            // Assert
            bookingRepoMock.Verify(repo => repo.UpdateAsync(It.Is<Booking>(b => b.CheckInStatus == Domain.Enums.CheckInStatus.CheckedOut)), Times.Once);
        }

        [Fact]
        public async Task CheckOutAsync_AlreadyCheckedOut_DoesNotUpdate()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking { ID = bookingId, CheckInStatus = Domain.Enums.CheckInStatus.CheckedOut };

            bookingRepoMock.Setup(repo => repo.GetById(bookingId))
                .ReturnsAsync(booking);

            // Act
            await bookingService.CheckOutAsync(bookingId);

            // Assert
            bookingRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public async Task CheckOutAsync_BookingRepoThrows_ThrowsException()
        {
            // Arrange
            var bookingId = 1;

            bookingRepoMock.Setup(repo => repo.GetById(bookingId))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await bookingService.CheckOutAsync(bookingId));
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_ValidId_ReturnsMappedBookingDto()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking { ID = bookingId };
            var bookingDetailsDto = new BookingDetailsDto
            {
                Id = bookingId,
                CheckInStatus = CheckInStatus.NotCheckedIn.ToString(),
                Customer = new GuestDto() { Email = "", FullName = "", PhoneNumber = "" },
                Rooms = new List<RoomDto>() { new RoomDto { RoomStatus = "", RoomTypeName = "" } }
            };

            bookingRepoMock.Setup(repo => repo.GetByIdWithDetails(bookingId))
                .ReturnsAsync(booking);

            mapperMock.Setup(m => m.Map<BookingDetailsDto>(booking))
                .Returns(bookingDetailsDto);

            // Act
            var result = await bookingService.GetByIdWithDetailsAsync(bookingId);

            // Assert
            Assert.Equal(bookingDetailsDto, result);
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var bookingId = 999;

            bookingRepoMock.Setup(repo => repo.GetByIdWithDetails(bookingId))
                .ReturnsAsync((Booking?)null);

            // Act
            var result = await bookingService.GetByIdWithDetailsAsync(bookingId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_BookingRepoThrows_ThrowsException()
        {
            // Arrange
            var bookingId = 1;

            bookingRepoMock.Setup(repo => repo.GetByIdWithDetails(bookingId))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await bookingService.GetByIdWithDetailsAsync(bookingId));
        }

        [Fact]
        public async Task GetByIdWithRooms_ValidId_ReturnsBooking()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking { ID = bookingId };

            bookingRepoMock.Setup(repo => repo.GetByIdWithRooms(bookingId))
                .ReturnsAsync(booking);

            // Act
            var result = await bookingService.GetByIdWithRooms(bookingId);

            // Assert
            Assert.Equal(booking, result);
        }

        [Fact]
        public async Task GetByIdWithRooms_InvalidId_ReturnsNull()
        {
            // Arrange
            var bookingId = 999;
            bookingRepoMock.Setup(repo => repo.GetByIdWithRooms(bookingId))
                .ReturnsAsync((Booking?)null);
            // Act
            var result = await bookingService.GetByIdWithRooms(bookingId);
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdWithRooms_BookingRepoThrows_ThrowsException()
        {
            // Arrange
            var bookingId = 1;
            bookingRepoMock.Setup(repo => repo.GetByIdWithRooms(bookingId))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await bookingService.GetByIdWithRooms(bookingId));
        }

        [Fact]
        public async Task DeleteBookingAsync_ValidBooking_DeletesBooking()
        {
            // Arrange
            var booking = new Booking { ID = 1 };

            // Act
            await bookingService.DeleteBookingAsync(booking);

            // Assert
            bookingRepoMock.Verify(repo => repo.DeleteAsync(booking.ID), Times.Once);
        }

        [Fact]
        public async Task DeleteBookingAsync_BookingRepoThrows_ThrowsException()
        {
            // Arrange
            var booking = new Booking { ID = 1 };

            bookingRepoMock.Setup(repo => repo.DeleteAsync(booking.ID))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await bookingService.DeleteBookingAsync(booking));
        }

        [Fact]
        public async Task CreateBookingAsync_ValidBooking_CreatesAndReturnsBookingDto()
        {
            // Arrange
            var booking = new Booking { ID = 1 };
            var returnedBookingDto = new BookingDto
            {
                Id = 1,
                Customer = new GuestDto { Email = "", FullName = "", PhoneNumber = "" },
                RoomIds = new List<int> { 101, 102 }
            };

            var createBookingRequest = new CreateBookingRequest
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                PersonCount = 2,
                Comment = "Test booking",
                RoomIds = new List<int> { 101, 102 },
                Email = "",
                FullName = "",
                PhoneNumber = ""
            };

            bookingRepoMock.Setup(repo => repo.CreateAsync(booking))
                .ReturnsAsync(booking);

            mapperMock.Setup(m => m.Map<Booking>(createBookingRequest))
            .Returns(booking);

            mapperMock.Setup(m => m.Map<BookingDto>(booking))
            .Returns(returnedBookingDto);

            // Act
            var result = await bookingService.CreateBookingAsync(createBookingRequest);

            // Assert
            bookingRepoMock.Verify(repo => repo.CreateAsync(booking), Times.Once);
            Assert.Equal(result, returnedBookingDto);
        }

        [Fact]
        public async Task CreateBookingAsync_BookingRepoThrows_ThrowsException()
        {
            // Arrange
            var booking = new Booking { ID = 1 };
            var createBookingRequest = new CreateBookingRequest
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                PersonCount = 2,
                Comment = "Test booking",
                RoomIds = new List<int> { 101, 102 },
                Email = "",
                FullName = "",
                PhoneNumber = ""
            };

            bookingRepoMock.Setup(repo => repo.CreateAsync(booking))
                .ThrowsAsync(new Exception("Database error"));

            mapperMock.Setup(m => m.Map<Booking>(createBookingRequest))
            .Returns(booking);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await bookingService.CreateBookingAsync(createBookingRequest));
        }

        [Fact]
        public async Task CreateBookingAsync_ValidBooking_SendsEmail()
        {
            // Arrange
            var booking = new Booking { ID = 1 };
            var returnedBookingDto = new BookingDto
            {
                Id = 1,
                Customer = new GuestDto { Email = "", FullName = "", PhoneNumber = "" },
                RoomIds = new List<int> { 101, 102 }
            };

            var createBookingRequest = new CreateBookingRequest
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                PersonCount = 2,
                Comment = "Test booking",
                RoomIds = new List<int> { 101, 102 },
                Email = "",
                FullName = "",
                PhoneNumber = ""
            };

            bookingRepoMock.Setup(repo => repo.CreateAsync(booking))
                .ReturnsAsync(booking);

            mapperMock.Setup(m => m.Map<Booking>(createBookingRequest))
            .Returns(booking);

            // Act
            var result = await bookingService.CreateBookingAsync(createBookingRequest);

            // Assert
            emailServiceMock.Verify(email => email.SendEmailAsync(booking), Times.Once);
        }

        [Fact]
        public async Task UpdateBookingAsync_BookingExists_UpdatesAndReturnsBookingDto()
        {
            // Arrange
            var booking = new Booking { ID = 1 };
            var returnedBookingDto = new BookingDto
            {
                Id = 1,
                Customer = new GuestDto { Email = "", FullName = "", PhoneNumber = "" },
                RoomIds = new List<int> { 101, 102 }
            };

            var updateBookingRequest = new UpdateBookingRequest
            {
                Id = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                PersonCount = 2,
                Comment = "Updated booking",
                RoomIds = new List<int> { 101, 102 },
                Email = "",
                FullName = "",
                PhoneNumber = ""
            };
            bookingRepoMock.Setup(repo => repo.UpdateAsync(booking))
                .ReturnsAsync(booking);

            bookingRepoMock.Setup(repo => repo.GetByIdWithDetails(updateBookingRequest.Id))
                .ReturnsAsync(booking);

            mapperMock.Setup(m => m.Map<Booking>(updateBookingRequest))
            .Returns(booking);

            mapperMock.Setup(m => m.Map<BookingDto>(booking))
            .Returns(returnedBookingDto);

            // Act
            var result = await bookingService.UpdateBookingAsync(updateBookingRequest);

            // Assert
            bookingRepoMock.Verify(repo => repo.UpdateAsync(booking), Times.Once);

            Assert.Equal(result, returnedBookingDto);
        }

        [Fact]
        public async Task UpdateBookingAsync_BookingDoesNotExist_ReturnsNull()
        {
            // Arrange
            var updateBookingRequest = new UpdateBookingRequest
            {
                Id = 999,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                PersonCount = 2,
                Comment = "Updated booking",
                RoomIds = new List<int> { 101, 102 },
                Email = "",
                FullName = "",
                PhoneNumber = ""
            };

            bookingRepoMock.Setup(repo => repo.GetByIdWithDetails(updateBookingRequest.Id))
                .ReturnsAsync((Booking?)null);

            // Act
            var result = await bookingService.UpdateBookingAsync(updateBookingRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateBookingAsync_BookingRepoThrows_ThrowsException()
        {
            // Arrange
            var booking = new Booking { ID = 1 };
            var updateBookingRequest = new UpdateBookingRequest
            {
                Id = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                PersonCount = 2,
                Comment = "Updated booking",
                RoomIds = new List<int> { 101, 102 },
                Email = "",
                FullName = "",
                PhoneNumber = ""
            };

            bookingRepoMock.Setup(repo => repo.GetByIdWithDetails(updateBookingRequest.Id))
                .ReturnsAsync(booking);

            mapperMock.Setup(m => m.Map<Booking>(updateBookingRequest))
            .Returns(booking);

            bookingRepoMock.Setup(repo => repo.UpdateAsync(booking))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await bookingService.UpdateBookingAsync(updateBookingRequest));
        }
    }
}
