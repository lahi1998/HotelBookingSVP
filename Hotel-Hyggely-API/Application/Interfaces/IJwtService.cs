using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(Staff staff);
    }
}
