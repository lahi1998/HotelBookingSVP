using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Requests;

namespace Application.Services
{
    public class AuthService
    {
        private readonly IJwtService jwtService;
        private readonly IStaffRepo staffRepo;
        private readonly IPasswordHasher passwordHasher;

        public AuthService(IJwtService jwtService, IStaffRepo staffRepo, IPasswordHasher passwordHasher) 
        {
            this.jwtService = jwtService;
            this.staffRepo = staffRepo;
            this.passwordHasher = passwordHasher;
        }

        public async Task<string?> AuthenticateAsync(LoginRequest request)
        {
            var existingStaff = await staffRepo.GetByUserNameAsync(request.UserName);

            if(existingStaff is null || !passwordHasher.VerifyPassword(request.Password, existingStaff.Password))
            {
                return null; // Invalid credentials
            }

            return jwtService.GenerateToken(existingStaff);
        }
    }
}
