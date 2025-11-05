using Application.Dtos;
using Application.Dtos.Booking;
using Application.Dtos.CleaningSchedule;
using Application.Interfaces.Repositories;
using Application.Requests.Booking;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
	public class BookingService
	{
		private readonly IBookingRepo bookingRepo;
        private readonly ICustomerRepo customerRepo;
        private readonly IMapper mapper;
        private readonly IRoomRepo roomRepo;
        private readonly ICleaningScheduleRepo cleaningScheduleRepo;

        public BookingService(IBookingRepo bookingRepo, ICustomerRepo customerRepo, IMapper mapper, IRoomRepo roomRepo, ICleaningScheduleRepo cleaningScheduleRepo)
		{
			this.bookingRepo = bookingRepo;
            this.customerRepo = customerRepo;
            this.mapper = mapper;
            this.roomRepo = roomRepo;
            this.cleaningScheduleRepo = cleaningScheduleRepo;
        }

		public async Task<IEnumerable<BookingListItemDto>> GetAllBookingsAsync()
		{
			var bookings = await bookingRepo.GetAllWithCustomerAndRoomsAsync();

            return mapper.Map<IEnumerable<BookingListItemDto>>(bookings);
		}

		public async Task CheckInAsync(int id)
		{
			var booking = await bookingRepo.GetById(id);

			if(booking!.CheckInStatus == CheckInStatus.CheckedIn)
			{
				return; // Already checked in
			}

			booking.CheckInStatus = CheckInStatus.CheckedIn;

			await bookingRepo.UpdateAsync(booking);
		}

		public async Task CheckOutAsync(int id)
		{
			var booking = await bookingRepo.GetById(id);

			if (booking!.CheckInStatus == CheckInStatus.CheckedOut)
			{
				return; // Already checked in
			}

			booking.CheckInStatus = CheckInStatus.CheckedOut;

			await bookingRepo.UpdateAsync(booking);
		}

		public async Task<Booking?> GetByIdAsync(int id)
		{
			return await bookingRepo.GetById(id);
		}

		public async Task<BookingDetailsDto?> GetByIdWithDetailsAsync(int id)
		{
			var booking = await bookingRepo.GetByIdWithDetails(id);

            return mapper.Map<BookingDetailsDto>(booking);
        }

		public async Task<IEnumerable<CleaningScheduleDto>> GetCleaningSchedulesByBookingIdAsync(int bookingId)
		{
			var cleaningSchedules = await cleaningScheduleRepo.GetByBookingIdWithRoom(bookingId);

			return mapper.Map<IEnumerable<CleaningScheduleDto>>(cleaningSchedules);
		}

		public async Task<BookingDto> CreateBookingAsync(CreateBookingRequest request)
		{
            var existingCustomer = await customerRepo.GetByEmailAsync(request.Email);

            var booking = mapper.Map<Booking>(request);

            if (existingCustomer != null)
            {
                booking.CustomerId = existingCustomer.ID;
				booking.Customer = null;
            }
            else
            {
                booking.Customer = new Customer
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber
                };
            }

            var rooms = await roomRepo.GetByIdsWithRoomTypeAsync(request.RoomIds);

            booking.Rooms = rooms.ToList();
			booking.TotalPrice = CalculateTotalPrice(request.StartDate, request.EndDate, booking.Rooms);

			var createdBooking = await bookingRepo.CreateAsync(booking);

            return mapper.Map<BookingDto>(createdBooking);
        }

        public async Task<BookingDto?> UpdateBookingAsync(UpdateBookingRequest request)
		{
			var existingBooking = await bookingRepo.GetByIdWithDetails(request.Id);

			if (existingBooking == null)
			{
				return null;
			}

			mapper.Map(request, existingBooking);

			if (existingBooking.Customer != null)
			{
				existingBooking.Customer.FullName = request.FullName;
				existingBooking.Customer.Email = request.Email;
				existingBooking.Customer.PhoneNumber = request.PhoneNumber;
			}

			var rooms = await roomRepo.GetByIdsWithRoomTypeAsync(request.RoomIds);
			existingBooking.Rooms = rooms.ToList();

			var updatedBooking = await bookingRepo.UpdateAsync(existingBooking);

			return mapper.Map<BookingDto>(updatedBooking);
		}

		public async Task DeleteBookingAsync(int id)
		{
			await bookingRepo.DeleteAsync(id);
		}

		private decimal CalculateTotalPrice(DateTime startDate, DateTime endDate, IEnumerable<Room> rooms)
		{
			var totalDays = (endDate - startDate).Days;
			decimal totalPrice = 0;
			foreach (var room in rooms)
			{
				totalPrice += room.RoomType!.Price * totalDays;
			}
			return totalPrice;
		}
	}
}
