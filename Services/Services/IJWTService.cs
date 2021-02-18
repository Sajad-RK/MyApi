using Entities;

namespace Services.Services
{
    public interface IJWTService
    {
        string Generate(User user);
    }
}