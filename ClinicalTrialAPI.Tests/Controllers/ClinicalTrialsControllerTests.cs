using ClinicalTrialAPI.Api.Controllers;
using ClinicalTrialAPI.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace ClinicalTrialAPI.Tests.Controllers
{
    public class ClinicalTrialsControllerTests
    {
        private readonly Mock<IClinicalTrialService> _mockService;
        private readonly Mock<ILogger<ClinicalTrialsController>> _mockLogger;
        private readonly ClinicalTrialsController _controller;

        public ClinicalTrialsControllerTests()
        {
            _mockService = new Mock<IClinicalTrialService>();
            _mockLogger = new Mock<ILogger<ClinicalTrialsController>>();
            _controller = new ClinicalTrialsController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Upload_ValidFile_ReturnsOk()
        {
            var jsonData = "{ \"trial\": { \"trialId\": \"1\", \"title\": \"Test Trial\", \"startDate\": \"2024-06-01\", \"endDate\": \"2024-06-30\", \"participants\": 10, \"status\": \"Ongoing\" } }";
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(jsonData)), 0, jsonData.Length, "file", "test.json");

            var result = await _controller.Upload(file);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Upload_LargeFile_ReturnsBadRequest()
        {
            var file = new Mock<IFormFile>();
            file.Setup(f => f.Length).Returns(2 * 1024 * 1024); // 2 MB

            var result = await _controller.Upload(file.Object);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("File size must not exceed 1 MB.", badRequestResult.Value);
        }

        [Fact]
        public async Task Upload_InvalidFileExtension_ReturnsBadRequest()
        {
            var file = new Mock<IFormFile>();
            file.Setup(f => f.FileName).Returns("test.txt");

            var result = await _controller.Upload(file.Object);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Only .json files are allowed.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetById_ValidId_ReturnsOk()
        {
            var trialId = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(trialId))
                        .ReturnsAsync(new ClinicalTrial());

            var result = await _controller.GetById(trialId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            var trialId = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(trialId)).ReturnsAsync((ClinicalTrial)null);

            var result = await _controller.GetById(trialId);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
