using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Booking
{
    public class CreateBookingRequest
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int PersonCount { get; set; }
        public string? Comment { get; set; }
        [Required]
        public required string FullName { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string PhoneNumber { get; set; }
        [Required]
        public required IEnumerable<int> RoomIds { get; set; } = new List<int>();
    }
}
