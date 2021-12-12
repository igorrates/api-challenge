using API.Controllers;
using API.Mapper;
using AutoMapper;
using Entities.DTO;
using Entities.Models;
using FluentAssertions;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Tests.Controller
{
    [TestClass]
    public class ApplicationControllerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly IMapper _mapper;
        private readonly ApplicationController _applicationController;

        public ApplicationControllerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerManager>();
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(config =>
                {
                    config.AddProfile(new MappingProfile());
                });
                var mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            _applicationController = new ApplicationController(_mockRepo.Object, _mockLogger.Object, _mapper);
        }

        [TestMethod]
        public async Task Get_AllApplicationsAsync()
        {
            IEnumerable<Application> applications = new List<Application>()
            {
                new Application()
                {
                    Id = 1,
                    DebuggingMode = true,
                    PathLocal = "path/local",
                    Url = "google.com"
                },
                new Application()
                {
                    Id=2,
                    DebuggingMode=false,
                    PathLocal = "another/path",
                    Url = "microsoft.com"
                }
            };

            var applicationRepository = new Mock<IApplicationRepository>();
            applicationRepository.Setup(x => x.GetAllApplicationsAsync()).ReturnsAsync(applications);
            _mockRepo.Setup(repo => repo.Application).Returns(applicationRepository.Object);

            var response = await _applicationController.GetAsync();

            response.Should().NotBeNull();
            response.Should().BeOfType<OkObjectResult>();
            response.As<OkObjectResult>().StatusCode.Should().Be(200);
            response.As<OkObjectResult>().Value.Should().NotBeNull();
            response.As<OkObjectResult>().Value.Should().BeOfType<List<ApplicationDTO>>();
            response.As<OkObjectResult>().Value.As<IEnumerable<ApplicationDTO>>().Should().HaveCount(2);
        }

        [TestMethod]
        public async Task Get_ApplicationByIdAsync_Ok()
        {
            Application application = new Application()
            {
                Id = 1,
                DebuggingMode = true,
                PathLocal = "path/local",
                Url = "google.com"
            };

            ApplicationDTO applicationDtoToVerify = new ApplicationDTO()
            {
                Id = 1,
                DebuggingMode = true,
                PathLocal = "path/local",
                Url = "google.com"
            };


            var applicationRepository = new Mock<IApplicationRepository>();
            applicationRepository.Setup(x => x.GetApplicationByIdAsync(1)).ReturnsAsync(application);
            _mockRepo.Setup(repo => repo.Application).Returns(applicationRepository.Object);

            var response = await _applicationController.GetAsync(1);

            response.Should().NotBeNull();
            response.Should().BeOfType<OkObjectResult>();
            response.As<OkObjectResult>().StatusCode.Should().Be(200);
            response.As<OkObjectResult>().Value.Should().NotBeNull();
            response.As<OkObjectResult>().Value.Should().BeOfType<ApplicationDTO>();
            _mockRepo.Verify(repo => repo.Application.GetApplicationByIdAsync(It.IsAny<int>()), Times.Once);
            response.As<OkObjectResult>().Value.As<ApplicationDTO>().Id.Should().Be(applicationDtoToVerify.Id);
            response.As<OkObjectResult>().Value.As<ApplicationDTO>().DebuggingMode.Should().Be(applicationDtoToVerify.DebuggingMode);
            response.As<OkObjectResult>().Value.As<ApplicationDTO>().PathLocal.Should().Be(applicationDtoToVerify.PathLocal);
            response.As<OkObjectResult>().Value.As<ApplicationDTO>().Url.Should().Be(applicationDtoToVerify.Url);
        }

        [TestMethod]
        public async Task Get_ApplicationByIdAsync_NotFound()
        {
            Application application = new Application()
            {
                Id = 1,
                DebuggingMode = true,
                PathLocal = "path/local",
                Url = "google.com"
            };


            var applicationRepository = new Mock<IApplicationRepository>();
            applicationRepository.Setup(x => x.GetApplicationByIdAsync(1)).ReturnsAsync(application);
            _mockRepo.Setup(repo => repo.Application).Returns(applicationRepository.Object);

            var response = await _applicationController.GetAsync(2);

            response.Should().NotBeNull();
            response.Should().BeOfType<NotFoundResult>();
            response.As<NotFoundResult>().StatusCode.Should().Be(404);
            _mockRepo.Verify(repo => repo.Application.GetApplicationByIdAsync(It.IsAny<int>()), Times.Once);

        }

        [TestMethod]
        public async Task Post_Application_Ok()
        {
            Application? app = null;

            ApplicationDTO? applicationDto = new ApplicationDTO()
            {
                Id = 1,
                DebuggingMode = true,
                PathLocal = "path/local",
                Url = "google.com"
            };

            var applicationRepository = new Mock<IApplicationRepository>();
            applicationRepository.Setup(x => x.CreateAsync(It.IsAny<Application>())).Callback<Application>(x => app = x);

            _mockRepo.Setup(repo => repo.Application).Returns(applicationRepository.Object);

            var response = await _applicationController.PostAsync(applicationDto);

            response.Should().NotBeNull();
            response.Should().BeOfType<OkObjectResult>();
            response.As<OkObjectResult>().StatusCode.Should().Be(200);
            response.As<OkObjectResult>().Value.Should().NotBeNull();
            response.As<OkObjectResult>().Value.Should().BeOfType<ApplicationDTO>();
            _mockRepo.Verify(repo => repo.Application.CreateAsync(It.IsAny<Application>()), Times.Once);
            response.As<OkObjectResult>().Value.As<ApplicationDTO>().Id.Should().Be(1);
            response.As<OkObjectResult>().Value.As<ApplicationDTO>().Url.Should().Be("google.com");
            response.As<OkObjectResult>().Value.As<ApplicationDTO>().PathLocal.Should().Be("path/local");
            response.As<OkObjectResult>().Value.As<ApplicationDTO>().DebuggingMode.Should().BeTrue();
        }

        [TestMethod]
        public async Task Post_Application_InvalidModel()
        {
            Application? app = null;

            ApplicationDTO? applicationDto = new ApplicationDTO()
            {
                Id = 1,
                DebuggingMode = true,
                PathLocal = "path/local",
                Url = "google.com"
            };

            var applicationRepository = new Mock<IApplicationRepository>();
            applicationRepository.Setup(x => x.CreateAsync(It.IsAny<Application>())).Callback<Application>(x => app = x);

            _mockRepo.Setup(repo => repo.Application).Returns(applicationRepository.Object);

            _applicationController.ModelState.AddModelError("Url", "Url is invalid");
            var response = await _applicationController.PostAsync(applicationDto);

            response.Should().NotBeNull();
            _mockRepo.Verify(repo => repo.Application.CreateAsync(It.IsAny<Application>()), Times.Never);
        }

        [TestMethod]
        public async Task Post_Application_NullModel()
        {
            Application? app = null;

            ApplicationDTO? applicationDto = new ApplicationDTO()
            {
                Id = 1,
                DebuggingMode = true,
                PathLocal = "path/local",
                Url = "google.com"
            };

            var applicationRepository = new Mock<IApplicationRepository>();
            applicationRepository.Setup(x => x.CreateAsync(It.IsAny<Application>())).Callback<Application>(x => app = x);

            _mockRepo.Setup(repo => repo.Application).Returns(applicationRepository.Object);

            var response = await _applicationController.PostAsync(null);

            response.Should().NotBeNull();
            _mockRepo.Verify(repo => repo.Application.CreateAsync(It.IsAny<Application>()), Times.Never);
        }

        [TestMethod]
        public async Task Patch_Application_Ok()
        {
            Application? app = null;

            ApplicationDTO? applicationDto = new ApplicationDTO()
            {
                Url = "google.com"
            };

            Application existingApplication = new Application()
            {
                Id = 1,
                DebuggingMode = true,
                PathLocal = "path.local",
                Url = "microsoft.com"
            };


            var applicationRepository = new Mock<IApplicationRepository>();
            applicationRepository.Setup(x => x.GetApplicationByIdAsync(1)).ReturnsAsync(existingApplication);
            applicationRepository.Setup(x => x.Update(It.IsAny<Application>())).Callback<Application>(x => app = x);

            _mockRepo.Setup(repo => repo.Application).Returns(applicationRepository.Object);

            var response = await _applicationController.PatchAsync(1, applicationDto);

            response.Should().NotBeNull();
            response.Should().BeOfType<OkObjectResult>();
            response.As<OkObjectResult>().StatusCode.Should().Be(200);
            response.As<OkObjectResult>().Value.Should().NotBeNull();
            response.As<OkObjectResult>().Value.Should().BeOfType<ApplicationDTO>();
            _mockRepo.Verify(repo => repo.Application.GetApplicationByIdAsync(It.IsAny<int>()), Times.Once);
            _mockRepo.Verify(repo => repo.Application.Update(It.IsAny<Application>()), Times.Once);
            response.As<OkObjectResult>().Value.As<ApplicationDTO>().Id.Should().Be(1);
            response.As<OkObjectResult>().Value.As<ApplicationDTO>().Url.Should().Be("google.com");
            response.As<OkObjectResult>().Value.As<ApplicationDTO>().PathLocal.Should().Be("path.local");
            response.As<OkObjectResult>().Value.As<ApplicationDTO>().DebuggingMode.Should().Be(true);
        }

        [TestMethod]
        public async Task Patch_Application_NotFound()
        {
            Application? app = null;

            ApplicationDTO? applicationDto = new ApplicationDTO()
            {
                Url = "google.com"
            };

            Application existingApplication = new Application()
            {
                Id = 1,
                DebuggingMode = true,
                PathLocal = "path.local",
                Url = "microsoft.com"
            };


            var applicationRepository = new Mock<IApplicationRepository>();
            applicationRepository.Setup(x => x.GetApplicationByIdAsync(1)).ReturnsAsync(existingApplication);
            applicationRepository.Setup(x => x.Update(It.IsAny<Application>())).Callback<Application>(x => app = x);

            _mockRepo.Setup(repo => repo.Application).Returns(applicationRepository.Object);

            var response = await _applicationController.PatchAsync(2, applicationDto);

            response.Should().NotBeNull();
            response.Should().BeOfType<NotFoundResult>();
            response.As<NotFoundResult>().StatusCode.Should().Be(404);
            _mockRepo.Verify(repo => repo.Application.GetApplicationByIdAsync(It.IsAny<int>()), Times.Once);
            _mockRepo.Verify(repo => repo.Application.Update(It.IsAny<Application>()), Times.Never);
        }

        [TestMethod]
        public async Task Delete_Application_Ok()
        {
            Application? app = null;

            Application existingApplication = new Application()
            {
                Id = 1,
                DebuggingMode = true,
                PathLocal = "path.local",
                Url = "microsoft.com"
            };


            var applicationRepository = new Mock<IApplicationRepository>();
            applicationRepository.Setup(x => x.GetApplicationByIdAsync(1)).ReturnsAsync(existingApplication);
            applicationRepository.Setup(x => x.Delete(It.IsAny<Application>())).Callback<Application>(x => app = x);

            _mockRepo.Setup(repo => repo.Application).Returns(applicationRepository.Object);

            var response = await _applicationController.DeleteAsync(1);

            response.Should().NotBeNull();
            response.Should().BeOfType<NoContentResult>();
            response.As<NoContentResult>().StatusCode.Should().Be(204);
            _mockRepo.Verify(repo => repo.Application.GetApplicationByIdAsync(It.IsAny<int>()), Times.Once);
            _mockRepo.Verify(repo => repo.Application.Delete(It.IsAny<Application>()), Times.Once);
            app.Should().NotBeNull();
            app.Id.Should().Be(existingApplication.Id);
            app.DebuggingMode.Should().Be(existingApplication.DebuggingMode);
            app.PathLocal.Should().Be(existingApplication.PathLocal);
            app.Url.Should().Be(existingApplication.Url);
        }

        [TestMethod]
        public async Task Delete_Application_NotFound()
        {
            Application? app = null;

            Application existingApplication = new Application()
            {
                Id = 1,
                DebuggingMode = true,
                PathLocal = "path.local",
                Url = "microsoft.com"
            };


            var applicationRepository = new Mock<IApplicationRepository>();
            applicationRepository.Setup(x => x.GetApplicationByIdAsync(1)).ReturnsAsync(existingApplication);
            applicationRepository.Setup(x => x.Delete(It.IsAny<Application>())).Callback<Application>(x => app = x);

            _mockRepo.Setup(repo => repo.Application).Returns(applicationRepository.Object);

            var response = await _applicationController.DeleteAsync(2);

            response.Should().NotBeNull();
            response.Should().BeOfType<NotFoundResult>();
            response.As<NotFoundResult>().StatusCode.Should().Be(404);
            _mockRepo.Verify(repo => repo.Application.GetApplicationByIdAsync(It.IsAny<int>()), Times.Once);
            _mockRepo.Verify(repo => repo.Application.Delete(It.IsAny<Application>()), Times.Never);
            app.Should().BeNull();            
        }
    }
}
