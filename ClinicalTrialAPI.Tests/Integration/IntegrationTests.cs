using System.IO;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ClinicalTrialAPI.Tests.Integration
{
    public class IntegrationTests
    {
        [Fact]
        public async Task Upload_ValidFile_ReturnsOk()
        {
            var client = new HttpClient();
            var fileContent = new StringContent(File.ReadAllText("test-clinical-trial.json"));
            var content = new MultipartFormDataContent();
            content.Add(fileContent, "file", "test-clinical-trial.json");

            var response = await client.PostAsync("/api/clinicaltrials/upload", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ValidId_ReturnsOk()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("/api/clinicaltrials/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("/api/clinicaltrials/999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
