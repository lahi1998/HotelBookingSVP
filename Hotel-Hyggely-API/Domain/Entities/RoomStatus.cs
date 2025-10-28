using Domain.Enums;

namespace Domain.Entities
{
    public class RoomStatus
    {
        public int ID { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public required RoomStatusType Status { get; set; }
        public DateTime StartDate{ get; set; }
        public DateTime EndDate { get; set; }
    }
}
