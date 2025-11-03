namespace Application.Requests.Staff
{
    public class CreateStaffRequest
    {
        public required string Role { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string FullName { get; set; }
    }
}
