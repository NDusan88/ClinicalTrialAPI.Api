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
    }
}
