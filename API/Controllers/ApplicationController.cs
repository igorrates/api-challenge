using Entities.Models;
using Interfaces;
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
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var applications = await _repository.Application.GetAllApplicationsAsync();
                return Ok(applications);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on GetAllApplicationsAsync()");
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var application = await _repository.Application.GetApplicationByIdAsync(id);
                if (application == null)
                {
                    _logger.LogWarn($"Application with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                return Ok(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on GetApplicationByIdAsync(id)");
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Application application)
        {
            try
            {
                await _repository.Application.CreateAsync(application);
                await _repository.SaveAsync();

                return Ok(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Create(application)");
                return Problem(ex.Message);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> PatchAsync(Application application)
        {
            try
            {
                _repository.Application.Update(application);
                await _repository.SaveAsync();
                return Ok(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Update(application)");
                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                Application? entity = _repository.Application.FindByCondition(a => a.Id == id).FirstOrDefault();
                if (entity != null)
                { 
                    _repository.Application.Delete(entity);
                    await _repository.SaveAsync();
                }
                else
                {
                    _logger.LogWarn($"Application with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                return Ok("Application deleted!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Delete(id)");
                return Problem(ex.Message);
            }
        }
    }
}
