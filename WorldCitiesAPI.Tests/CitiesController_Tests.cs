using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WorldCitiesAPI.Controllers;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Data.Models;

namespace WorldCitiesAPI.Tests;

public class CitiesController_Tests
{
    /// <summary>
    /// Test the GetCity() method.
    /// </summary>
    [Fact]
    public async Task GetCity()
    {
        // Arrange
        // todo: define the required assets.

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "WorldCities").Options;

        using ApplicationDbContext context = new ApplicationDbContext(options);
        context.Add(new City()
        {
            Id = 1,
            CountryId = 1,
            Lat = 1,
            Lon = 1,
            Name = "TestCity1"
        });
        context.SaveChanges();

        CitiesController controller = new(context);
        City? city_existing = null;
        City? city_notExisting = null;

        // Act
        // todo: invoke the test.

        city_existing = (await controller.GetCity(1)).Value;
        city_notExisting = (await controller.GetCity(2)).Value;

        // Assert
        // todo: verify that the conditions are met.

        Assert.NotNull(city_existing);
        Assert.Null(city_notExisting);
    }

}
