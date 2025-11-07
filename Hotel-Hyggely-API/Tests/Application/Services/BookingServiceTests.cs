using Application.Dtos.Booking;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace Tests.Application.Services
{
    public class BookingServiceTests
    {
        private readonly BookingService bookingService;

        private readonly Mock<IBookingRepo> bookingRepoMock;
        private readonly Mock<IRoomRepo> roomRepoMock;
        private readonly Mock<ICustomerRepo> customerRepoMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<ICleaningScheduleRepo> cleaningScheduleMock;
        private readonly Mock<IEmailService> emailServiceMock;
		public BookingServiceTests()
        {
            bookingRepoMock = new Mock<IBookingRepo>();
            roomRepoMock = new Mock<IRoomRepo>();
            customerRepoMock = new Mock<ICustomerRepo>();
            mapperMock = new Mock<IMapper>();
            cleaningScheduleMock = new Mock<ICleaningScheduleRepo>();
            emailServiceMock = new Mock<IEmailService>();

            bookingService = new BookingService(
                bookingRepoMock.Object,
				customerRepoMock.Object,
				mapperMock.Object,
				roomRepoMock.Object,
                cleaningScheduleMock.Object,
                emailServiceMock.Object);
		}

        [Fact]
        public async Task GetAllBookingsAsync_ReturnsMappedBookingDtos()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { ID = 1 },
                new Booking { ID = 2 }
            };

            var bookingDtos = new List<BookingDto>
            {
                new BookingDto { ID = 1, Customer = new() },
                new BookingDto { ID = 2 }
            };
            bookingRepoMock.Setup(repo => repo.GetAllWithCustomerAndRoomsAsync())
                .ReturnsAsync(bookings);

            mapperMock.Setup(m => m.Map<IEnumerable<BookingDto>>(bookings))
                .Returns(bookingDtos);
            // Act
            var result = await bookingService.GetAllBookingsAsync();

            // Assert
            Assert.Equal(bookingDtos, result);
		}
	}
}
