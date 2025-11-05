using Domain.Enums;

namespace Domain.Entities
{
    public class RoomMaintenance
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public DateTime StartDate{ get; set; }
        public DateTime EndDate { get; set; }
        public required string Reason { get; set; }
    }
}
