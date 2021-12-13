using Entities;
using Entities.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ApplicationRepository : RepositoryBase<Application>, IApplicationRepository
    {
        public ApplicationRepository(RepoContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Application?> GetApplicationByIdAsync(int id)
        {
            return await FindByCondition(app => app.Id.Equals(id)).FirstOrDefaultAsync();
        }
    }
}
