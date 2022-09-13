using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using WorldCitiesAPI.Controllers;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Data.Models;
using Xunit;

namespace WorldCitiesAPI.Tests;

public class SeedController_Tests
{
    /// <summary>
    /// Test the CreateDefaultUsers() method.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateDefaultUsers()
    {
        // Arrange
        // create option instances required by the ApplicationDbContext
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: "WorldCities").Options;
        IWebHostEnvironment mockEnv = Mock.Of<IWebHostEnvironment>();
        Mock<IConfiguration> mockConfiguration = new();
    }
}
