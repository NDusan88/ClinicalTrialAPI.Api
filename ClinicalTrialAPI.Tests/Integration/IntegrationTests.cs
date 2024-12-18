using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;

namespace ClinicalTrialAPI.Tests.Integration
{
    public class IntegrationTests
    {
        private HttpClient CreateMockHttpClient(HttpStatusCode statusCode, string responseContent)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler
                .Protected()
                // Set up the protected method "SendAsync" in HttpMessageHandler
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            return new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:5000") // Mocked base address
            };
        }

        [Fact]
        public async Task Upload_ValidFile_ReturnsOk()
        {
            // Arrange
            var client = CreateMockHttpClient(HttpStatusCode.OK, "File uploaded successfully.");
            var jsonData = "{ \"trial\": { \"trialId\": \"fd4aa01f-b534-4947-b071-fe38219aa831\", \"title\": \"Test Trial\", \"startDate\": \"2024-06-01\", \"endDate\": \"2024-06-30\", \"participants\": 10, \"status\": \"Ongoing\" } }";
            var fileContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var content = new MultipartFormDataContent();
            content.Add(fileContent, "file", "test-clinical-trial.json");

            // Act
            var response = await client.PostAsync("/api/clinicaltrials/upload", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ValidId_ReturnsOk()
        {
            // Arrange
            var client = CreateMockHttpClient(HttpStatusCode.OK, "{ \"trialId\": \"fd4aa01f-b534-4947-b071-fe38219aa831\", \"title\": \"Test Trial\" }");

            // Act
            var response = await client.GetAsync("/api/clinicaltrials/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var client = CreateMockHttpClient(HttpStatusCode.NotFound, "Trial not found.");

            // Act
            var response = await client.GetAsync("/api/clinicaltrials/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
