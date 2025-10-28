using Domain.Enums;

namespace Domain.Entities
{
    public class Staff
    {
        public int ID { get; set; }
        public required StaffRole Role { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string FullName { get; set; }
    }
}
