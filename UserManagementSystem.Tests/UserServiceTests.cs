using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using UserManagementSystem.DAL;
using UserManagementSystem.DAL.Data;
using UserManagementSystem.DAL.Entities;
using UserManagementSystem.DAL.Repositories;
using Xunit;

namespace UserManagementSystem.Tests
{
    public class UserServiceTests
    {
        private async Task<AppDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new AppDbContext(options);
            await dbContext.Database.EnsureCreatedAsync();

            dbContext.Users.AddRange(new List<User>
            {
                new User { Id = 1, FirstName = "Alice" },
                new User { Id = 2, FirstName = "Bob" }
            });

            await dbContext.SaveChangesAsync();
            return dbContext;
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnCorrectUserCount()
        {
            var dbContext = await GetDatabaseContext();
            var userRepository = new UserRepository(dbContext);

            var users = await userRepository.GetAllUsersAsync();

            users.Count().Should().Be(2);
        }

        [Fact]
        public async Task AddUser_ShouldIncreaseUserCount()
        {
            var dbContext = await GetDatabaseContext();
            var userRepository = new UserRepository(dbContext);

            await userRepository.AddUserAsync(new User { Id = 3, FirstName = "Charlie" });
            var users = await userRepository.GetAllUsersAsync();

            users.Count().Should().Be(3);
        }
    }
}
