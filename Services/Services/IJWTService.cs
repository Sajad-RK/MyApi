using Entities;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IJWTService
    {
        Task<string> Generate(User user);
    }
}