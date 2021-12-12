using Entities.Models;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;

        public ApplicationController(IRepositoryWrapper repository, ILoggerManager logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var applications = _repository.Application.FindAll();

            return Ok(applications.ToList());
        }

        [HttpPost] 
        public IActionResult Post(Application application)
        {
            _repository.Application.Create(application);
            _repository.Save();

            return Ok(application);
        }
    }
}
