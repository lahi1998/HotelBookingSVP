namespace Application.Dtos.Staff
{
    public class StaffDto
    {
        public int Id { get; set; }
        public required string Role { get; set; }
        public required string UserName { get; set; }
        public required string FullName { get; set; }
    }
}
