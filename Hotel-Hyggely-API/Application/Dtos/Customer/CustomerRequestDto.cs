namespace Application.Dtos.Customer
{
    public class CustomerRequestDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
