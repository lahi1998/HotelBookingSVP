using Application.Dtos;
using Application.Dtos.Booking;
using Application.Dtos.CleaningSchedule;
using Application.Exeptions;
using Application.Interfaces;
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
		private readonly IGuestRepo guestRepo;
		private readonly IMapper mapper;
		private readonly IRoomRepo roomRepo;
		private readonly ICleaningScheduleRepo cleaningScheduleRepo;
        private readonly IEmailService emailService;

        public BookingService(IBookingRepo bookingRepo, IGuestRepo customerRepo, IMapper mapper, IRoomRepo roomRepo, ICleaningScheduleRepo cleaningScheduleRepo, IEmailService emailService)
		{
			this.bookingRepo = bookingRepo;
			this.guestRepo = customerRepo;
			this.mapper = mapper;
			this.roomRepo = roomRepo;
			this.cleaningScheduleRepo = cleaningScheduleRepo;
            this.emailService = emailService;
        }

		public async Task<IEnumerable<BookingListItemDto>> GetAllBookingsAsync()
		{
			var bookings = await bookingRepo.GetAllWithCustomerAndRoomsAsync();

			return mapper.Map<IEnumerable<BookingListItemDto>>(bookings);
		}

		public async Task CheckInAsync(int id)
		{
			var booking = await bookingRepo.GetById(id);

			if (booking!.CheckInStatus == CheckInStatus.CheckedIn)
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
		public async Task<Booking?> GetByIdWithRooms(int id)
		{
			return await bookingRepo.GetByIdWithRooms(id);
		}
		public async Task<BookingDetailsDto?> GetByIdWithDetailsAsync(int id)
		{
			var booking = await bookingRepo.GetByIdWithDetails(id);

			return mapper.Map<BookingDetailsDto>(booking);
		}

		public async Task<BookingDto> CreateBookingAsync(CreateBookingRequest request)
		{
			var existingCustomer = await guestRepo.GetByEmailAsync(request.Email);

			var booking = mapper.Map<Booking>(request);

			if (existingCustomer != null)
			{
				booking.GuestId = existingCustomer.ID;
				booking.Guest = null;
			}
			else
			{
				booking.Guest = new Guest
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

			await emailService.SendEmailAsync(createdBooking);

			// Schedule cleaning for each booked room on the booking end date
			var cleaningDate = request.EndDate.Date;

			foreach (var room in createdBooking.Rooms)
			{
				await cleaningScheduleRepo.CreateForRoomAsync(new CleaningSchedule
				{
					RoomId = room.ID,
					CleaningDate = cleaningDate
				});
			}

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

			if (existingBooking.Guest != null)
			{
				existingBooking.Guest.FullName = request.FullName;
				existingBooking.Guest.Email = request.Email;
				existingBooking.Guest.PhoneNumber = request.PhoneNumber;
			}

			var rooms = await roomRepo.GetByIdsWithRoomTypeAsync(request.RoomIds);
			existingBooking.Rooms = rooms.ToList();

			var updatedBooking = await bookingRepo.UpdateAsync(existingBooking);

			return mapper.Map<BookingDto>(updatedBooking);
		}

		public async Task DeleteBookingAsync(Booking booking)
		{
			await bookingRepo.DeleteAsync(booking.ID);

			// Remove schedule cleaning for each booked room
			foreach (var room in booking.Rooms)
			{
				var schedules = room.CleaningSchedules.ToList();

				foreach (var cleaningSchedule in schedules)
				{
					await cleaningScheduleRepo.DeleteAsync(cleaningSchedule.ID);
				}
			}

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
