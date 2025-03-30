using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using UserManagementSystem.API;
using UserManagementSystem.DAL.Entities;
using Xunit;

namespace UserManagementSystem.Tests
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UserControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetUsers_ShouldReturnSuccess()
        {
            var response = await _client.GetAsync("/api/users");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreateUser_ShouldReturnSuccess()
        {
            var newUser = new { Name = "David" };
            var content = new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/users", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
