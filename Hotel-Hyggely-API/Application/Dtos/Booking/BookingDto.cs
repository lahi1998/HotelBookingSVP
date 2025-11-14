using Application.Dtos.Customer;
using Application.Dtos.Room;
using Domain.Enums;

namespace Application.Dtos.Booking
{
    public class BookingDto
    {
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public CheckInStatus CheckInStatus { get; set; }
		public decimal TotalPrice { get; set; }
		public int PersonCount { get; set; }
		public string? Comment { get; set; }
		public required GuestDto Customer { get; set; }
		public required IEnumerable<int> RoomIds { get; set; } = new List<int>();
	}
}
