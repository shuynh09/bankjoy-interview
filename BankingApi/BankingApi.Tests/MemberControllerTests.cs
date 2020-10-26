using BankingApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace BankingApi
{
    [Collection("Member Tests")]
    public class MemberControllerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public MemberControllerTests()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task GetMemberListSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/member");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<MemberModel>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            // Assert
            Assert.Equal(3, data.Count());
        }

        [Fact]
        public async Task GetMemberSuccess()
        {
            // Act
            var response = await _client.GetAsync($"/api/member/{234789}");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<MemberModel>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            // Assert
            Assert.Equal("John", data.GivenName);
            Assert.Equal("Doe", data.Surname);
            Assert.Equal(78923, data.InstitutionId);
            Assert.NotNull(data.Accounts);
            Assert.Equal(1, data.Accounts.Count);
            Assert.Equal(23456, data.Accounts[0].AccountId);
            Assert.Equal(12.5, data.Accounts[0].Balance);
        }

        [Fact]
        public async Task PostMemberSuccess()
        {
            var payload = "{\"givenName\": \"John\",\"surname\": \"Doe\",\"institutionId\": 78923,\"accounts\": [{\"accountId\": 23456, \"balance\": 12.5} ] }";
            // Act
            var response = await _client.PostAsync($"/api/member", new StringContent(payload, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task PostMemberFailure()
        {
            var payload = "{\"givenName\": \"John\",\"surname\": \"Doe\",\"institutionId\": 78922,\"accounts\": [{\"accountId\": 23456, \"balance\": 12.5} ] }";
            // Act
            var response = await _client.PostAsync($"/api/member", new StringContent(payload, Encoding.UTF8, "application/json"));
            
            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task UpdateMemberSuccess()
        {
            var payload = "{\"memberId\": 234791, \"givenName\": \"Bob\",\"surname\": \"Smith\" }";

            // Act
            var response = await _client.PutAsync($"/api/member/{234791}", new StringContent(payload, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task UpdateMemberFailure()
        {
            var payload = "{ \"givenName\": \"Bob\",\"surname\": \"Smith\" }";

            // Act
            var response = await _client.PutAsync($"/api/member/{234789}", new StringContent(payload, Encoding.UTF8, "application/json"));

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task DeleteMemberSuccess()
        {
            // Act
            var response = await _client.DeleteAsync($"/api/member/{234790}");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteMemberFailure()
        {
            // Act
            var response = await _client.DeleteAsync($"/api/member/{23478999}");

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateBalanceSuccess()
        {
            var payload = "{\"memberId\": 234789, \"accountId\": 23456,\"balance\": 15.5 }";

            // Act
            var response = await _client.PutAsync($"/api/member/updatebalance/{234789}", new StringContent(payload, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }


    }
}
