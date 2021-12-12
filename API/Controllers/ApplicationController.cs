using AutoMapper;
using Entities.DTO;
using Entities.Models;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Application controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ILoggerManager _logger;
        private IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ApplicationController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all applications
        /// </summary>
        /// <returns>List of applications</returns>
        /// <response code="200">All the applications</response> 
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var applications = await _repository.Application.GetAllApplicationsAsync();
                return Ok(_mapper.Map<IEnumerable<ApplicationDTO>?>(applications));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on GetAllApplicationsAsync()");
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Get Application by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The application</returns>
        /// <response code="200">Returns the application object</response>
        /// <response code="404">If the application is not found</response>
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

                return Ok(_mapper.Map<ApplicationDTO>(application));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on GetApplicationByIdAsync(id)");
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Create application
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(ApplicationDTO application)
        {
            try
            {
                if (application == null)
                {
                    _logger.LogError("Application object sent from client is null.");
                    return BadRequest("Application object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Application object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var entity = _mapper.Map<Application>(application);
                await _repository.Application.CreateAsync(entity);
                await _repository.SaveAsync();

                var createdEntity = _mapper.Map<ApplicationDTO>(entity);

                return Ok(createdEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Create(application)");
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Perform partial updates to the Application
        /// </summary>
        /// <param name="id">Id of the Application to be updated</param>
        /// <param name="application"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] ApplicationDTO application)
        {
            try
            {
                if (application == null)
                {
                    _logger.LogError("Application object sent from client is null.");
                    return BadRequest("Application object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Application object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var entity = await _repository.Application.GetApplicationByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarn($"Application with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(application, entity);

                _repository.Application.Update(entity);
                await _repository.SaveAsync();

                var updatedEntity = _mapper.Map<ApplicationDTO>(entity);

                return Ok(updatedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Update(application)");
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Delete an Application
        /// </summary>
        /// <param name="id">Id of the application to be removed</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.Application.GetApplicationByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarn($"Application with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                
                _repository.Application.Delete(entity);
                await _repository.SaveAsync();
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Delete(id)");
                return Problem(ex.Message);
            }
        }
    }
}
