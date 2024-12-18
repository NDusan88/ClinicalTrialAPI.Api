using ClinicalTrialAPI.Application.UseCases;
using ClinicalTrialAPI.Domain.Repositories;
using Moq;

namespace ClinicalTrialAPI.Tests.UseCases
{
    public class AddClinicalTrialUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldCallAddAsync_Once()
        {
            // Arrange
            var mockRepository = new Mock<IClinicalTrialRepository>();
            var useCase = new AddClinicalTrialUseCase(mockRepository.Object);

            var trial = new ClinicalTrial
            {
                Title = "Test Trial",
                StartDate = new DateTime(2024, 6, 1),
                EndDate = new DateTime(2024, 6, 10)
            };

            // Act
            await useCase.ExecuteAsync(trial);

            // Assert
            mockRepository.Verify(r => r.AddAsync(trial), Times.Once);
            Assert.Equal(9, trial.Duration); // Duration: 10 - 1 = 9
        }
        [Fact]
        public async Task AddTrial_ValidData_SavesToRepository()
        {
            var mockRepository = new Mock<IClinicalTrialRepository>();
            var useCase = new AddClinicalTrialUseCase(mockRepository.Object);

            var trial = new ClinicalTrial { Title = "COVID-19 Trial", StartDate = new DateTime(2024, 6, 1), EndDate = new DateTime(2024, 6, 30) };

            await useCase.ExecuteAsync(trial);

            mockRepository.Verify(r => r.AddAsync(trial), Times.Once);
        }

        [Fact]
        public async Task AddTrial_OngoingStatus_SetsEndDateToOneMonth()
        {
            var mockRepository = new Mock<IClinicalTrialRepository>();
            var useCase = new AddClinicalTrialUseCase(mockRepository.Object);

            var trial = new ClinicalTrial { Title = "Ongoing Trial", Status = "Ongoing", StartDate = new DateTime(2024, 6, 1) };

            await useCase.ExecuteAsync(trial);

            Assert.Equal(new DateTime(2024, 7, 1), trial.EndDate);
        }
    }
}
