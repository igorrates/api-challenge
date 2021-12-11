using Entities;
using Entities.Models;
using Interfaces;

namespace Repository
{
    public class ApplicationRepository : RepositoryBase<Application>, IApplicationRepository
    {
        public ApplicationRepository(RepoContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
