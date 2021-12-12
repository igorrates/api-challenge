using Entities.Models;

namespace Interfaces
{
    public interface IApplicationRepository : IRepositoryBase<Application>
    {
        Task<IEnumerable<Application>> GetAllApplicationsAsync();
        Task<Application> GetApplicationByIdAsync(int id);
    }
}
