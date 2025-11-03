namespace Application.Requests.Staff
{
    public class UpdateStaffRequest
    {
        public int Id { get; set; }
        public required string Role { get; set; }
		public required string UserName { get; set; }
		public required string Password { get; set; }
		public required string FullName { get; set; }
	}
}
