using Application.Dtos.Customer;
using Domain.Enums;

namespace Application.Requests.Booking
{
    public class CreateBookingRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public int PersonCount { get; set; }
        public string? Comment { get; set; }
        public required CustomerRequestDto Customer { get; set; }
        public required IEnumerable<int> RoomIds { get; set; } = new List<int>();
    }
}
