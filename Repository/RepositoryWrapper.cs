using Entities;
using Interfaces;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepoContext _repoContext;
        private IApplicationRepository _applicationRepository;

        public RepositoryWrapper(RepoContext repoContext)
        {
            _repoContext = repoContext ?? throw new ArgumentNullException(nameof(repoContext));
        }

        public IApplicationRepository Application { 
            get {
                if (_applicationRepository == null)
                {
                    _applicationRepository = new ApplicationRepository(_repoContext);
                }
                return _applicationRepository;
            } }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
