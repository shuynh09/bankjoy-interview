using BankingApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace BankingApi
{
    [Collection("Institution Test")]
    public class InstitutionControllerTests 
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public InstitutionControllerTests() {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task GetInstitutionListSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/institution");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<Institution>>(jsonString); 

            // Assert
            Assert.Single(data);
        }

        [Fact]
        public async Task PostInstitutionSuccess()
        {
            var payload = "{\"Name\" : \"Bank of Success\"}";
            // Act

            var response = await _client.PostAsync($"/api/institution", new StringContent(payload, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task PostInstitutionFailure()
        {
            var payload = "{\"Name\" : \"\"}";
            // Act
            var response = await _client.PostAsync($"/api/institution", new StringContent(payload, Encoding.UTF8, "application/json"));

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostTransferSuccess()
        {
            var payload = "{\"memberId\": 234789, \"accountId\": 23456,\"transferAmount\": 12.5, \"TransferMemberId\": 234790, \"TransferAccountId\": 23457 }";

            // Act
            var response = await _client.PostAsync($"/api/institution/transfer/{78923}", new StringContent(payload, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task PostTransferFailure()
        {
            var payload = "{\"memberId\": 234789, \"accountId\": 23456,\"transferAmount\": 15.5, \"TransferMemberId\": 234790, \"TransferAccountId\": 23457 }";

            // Act
            var response = await _client.PostAsync($"/api/institution/transfer/{78923}", new StringContent(payload, Encoding.UTF8, "application/json"));

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.Conflict, response.StatusCode);
        }
    }
}
