using ClinicalTrialAPI.Application.UseCases;
using ClinicalTrialAPI.Domain.Repositories;
using Moq;

namespace ClinicalTrialAPI.Tests.UseCases
{
    public class GetAllClinicalTrialsByStatusUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ShouldReturnTrialsByStatus()
        {
            // Arrange
            var mockRepository = new Mock<IClinicalTrialRepository>();
            var useCase = new GetAllClinicalTrialsByStatusUseCase(mockRepository.Object);

            var trials = new List<ClinicalTrial>
            {
                new ClinicalTrial { Title = "Trial 1", Status = "Ongoing" },
                new ClinicalTrial { Title = "Trial 2", Status = "Ongoing" }
            };
            mockRepository.Setup(r => r.GetByStatusAsync("Ongoing")).ReturnsAsync(trials);

            // Act
            var result = await useCase.ExecuteAsync("Ongoing");

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, t => Assert.Equal("Ongoing", t.Status));
        }
    }
}
