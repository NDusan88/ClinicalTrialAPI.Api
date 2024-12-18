using ClinicalTrialAPI.Domain.Repositories;
using Moq;

namespace ClinicalTrialAPI.Tests.UseCases
{
    public class GetClinicalTrialByIdUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldReturnCorrectTrial()
        {
            // Arrange
            var mockRepository = new Mock<IClinicalTrialRepository>();
            var useCase = new GetClinicalTrialByIdUseCase(mockRepository.Object);

            var expectedTrial = new ClinicalTrial { TrialId = Guid.NewGuid(), Title = "Test Trial" };
            mockRepository.Setup(r => r.GetByIdAsync(expectedTrial.TrialId)).ReturnsAsync(expectedTrial);

            // Act
            var result = await useCase.ExecuteAsync(expectedTrial.TrialId);

            // Assert
            Assert.Equal(expectedTrial, result);
        }
    }
}
