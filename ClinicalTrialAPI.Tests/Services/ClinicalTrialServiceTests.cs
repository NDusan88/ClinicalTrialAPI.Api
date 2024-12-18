using ClinicalTrialAPI.Application.Services;
using ClinicalTrialAPI.Application.UseCases;
using ClinicalTrialAPI.Domain.Repositories;
using Moq;

namespace ClinicalTrialAPI.Tests.Services
{
    public class ClinicalTrialServiceTests
    {
        [Fact]
        public async Task AddClinicalTrialAsync_ShouldCallRepositoryAddAsync()
        {
            // Arrange
            var mockRepository = new Mock<IClinicalTrialRepository>();
            var addUseCase = new AddClinicalTrialUseCase(mockRepository.Object);

            var service = new ClinicalTrialService(addUseCase, null, null);

            var trial = new ClinicalTrial
            {
                Title = "COVID-19 Trial",
                StartDate = new DateTime(2024, 6, 1),
                EndDate = new DateTime(2024, 6, 10)
            };

            // Act
            await service.AddClinicalTrialAsync(trial);

            // Assert
            mockRepository.Verify(r => r.AddAsync(trial), Times.Once);
            Assert.Equal(9, trial.Duration);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldCallUseCase()
        {
            // Arrange
            var mockRepository = new Mock<IClinicalTrialRepository>();
            var getByIdUseCase = new GetClinicalTrialByIdUseCase(mockRepository.Object);

            var service = new ClinicalTrialService(null, getByIdUseCase, null);

            var TrialId = Guid.NewGuid();
            var expectedTrial = new ClinicalTrial { TrialId = TrialId, Title = "COVID-19 Trial" };

            mockRepository.Setup(r => r.GetByIdAsync(TrialId)).ReturnsAsync(expectedTrial);

            // Act
            var result = await service.GetByIdAsync(TrialId);

            // Assert
            Assert.Equal(expectedTrial, result);
        }

        [Fact]
        public async Task GetAllByStatusAsync_ShouldCallRepositoryGetByStatusAsync()
        {
            // Arrange
            var mockRepository = new Mock<IClinicalTrialRepository>();
            var getAllByStatusUseCase = new GetAllClinicalTrialsByStatusUseCase(mockRepository.Object);

            var service = new ClinicalTrialService(null, null, getAllByStatusUseCase);

            var status = "Ongoing";
            var trials = new List<ClinicalTrial>
            {
                new ClinicalTrial { Title = "Trial 1", Status = "Ongoing" },
                new ClinicalTrial { Title = "Trial 2", Status = "Ongoing" }
            };

            mockRepository.Setup(r => r.GetByStatusAsync(status)).ReturnsAsync(trials);

            // Act
            var result = await service.GetAllByStatusAsync(status);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, t => Assert.Equal(status, t.Status));
        }

        [Fact]
        public async Task AddClinicalTrialAsync_ValidTrial_CallsUseCase()
        {
            var mockRepository = new Mock<IClinicalTrialRepository>();
            var useCase = new AddClinicalTrialUseCase(mockRepository.Object);
            var service = new ClinicalTrialService(useCase, null, null);

            var trial = new ClinicalTrial { Title = "Trial 1", Status = "Ongoing", StartDate = DateTime.Now };

            await service.AddClinicalTrialAsync(trial);

            mockRepository.Verify(r => r.AddAsync(trial), Times.Once);
        }
    }
}
