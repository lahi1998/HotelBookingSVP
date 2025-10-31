using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Requests.Booking;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
	public class BookingService
	{
		private readonly IBookingRepo bookingRepo;
        private readonly ICustomerRepo customerRepo;

        public BookingService(IBookingRepo bookingRepo, ICustomerRepo customerRepo)
		{
			this.bookingRepo = bookingRepo;
            this.customerRepo = customerRepo;
        }

		public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
		{
			return await bookingRepo.GetAllWithCustomerAsync();
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

		public async Task<Booking?> GetByIdWithDetailsAsync(int id)
		{
			return await bookingRepo.GetByIdWithDetails(id);
		}

		public async Task<Booking> CreateBookingAsync(CreateBookingRequest request)
		{
			var existingCustomer = await customerRepo.GetByEmailAsync(request.Customer.Email);

			var booking = new Booking
			{
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				CheckInStatus = CheckInStatus.NotCheckedIn,
				TotalPrice = request.Price,
				PersonCount = request.PersonCount,
				Comment = request.Comment,
				Customer = existingCustomer ?? new Customer
				{
					FullName = request.Customer.FullName,
					Email = request.Customer.Email,
					PhoneNumber = request.Customer.PhoneNumber
				}
			};

			return await bookingRepo.CreateAsync(booking);
		}

		public async Task<Booking?> UpdateBookingAsync(UpdateBookingRequest request)
		{
			var booking = new Booking
			{
				ID = request.Id,
				CustomerId = request.CustomerId,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				CheckInStatus = request.Status,
				TotalPrice = request.Price,
				PersonCount = request.PersonCount,
				Comment = request.Comment
			};

			return await bookingRepo.UpdateAsync(booking);
		}

		public async Task DeleteBookingAsync(int id)
		{
			await bookingRepo.DeleteAsync(id);
		}
	}
}
