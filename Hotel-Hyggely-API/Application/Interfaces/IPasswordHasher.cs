namespace Application.Interfaces
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string enteredPassword, string storedHash);
    }
}
