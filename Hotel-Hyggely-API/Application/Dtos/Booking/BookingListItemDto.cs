using Application.Dtos.Customer;

namespace Application.Dtos.Booking
{
    public class BookingListItemDto
    {
        public int Id { get; set; }
        public int RoomCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CustomerDto Customer { get; set; }
    }
}
