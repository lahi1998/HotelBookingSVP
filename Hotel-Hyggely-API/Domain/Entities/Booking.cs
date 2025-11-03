using Domain.Enums;

namespace Domain.Entities
{
    public class Booking
    {
        public int ID { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CheckInStatus CheckInStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public int PersonCount { get; set; }
        public string? Comment { get; set; }

        public ICollection<Room> Rooms { get; set; } = [];
    }
}
