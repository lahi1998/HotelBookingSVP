using Domain.Enums;

namespace Application.Requests.Booking
{
    public class UpdateBookingRequest
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CheckInStatus Status { get; set; }
        public decimal Price { get; set; }
        public int PersonCount { get; set; }
        public string? Comment { get; set; }
    }
}
