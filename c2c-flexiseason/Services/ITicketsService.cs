using System.Threading.Tasks;

namespace c2c_flexiseason.Services
{
    public interface ITicketsService
    {
        Task<int> GetTicketsRemaining();
    }
}
