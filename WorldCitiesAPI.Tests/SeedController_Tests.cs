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
using XUnit;

namespace WorldCitiesAPI.Tests;
public class SeedController_Tests {
    
    /// <summary>
    /// Test the CreateDefaultUsers() method
    /// </summary>
    [Fact]
    public async Task CreateDefaultUsers() {
        // Arrange
        // Create the option instances required by the ApplicationDbContext
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "WorldCiies").Options;
            
        // Create a IWebHostEnvironment mock instance
        var mockEnv = Mock.Of<IWebHostEnvironment>();
        
        // Create an IConfiguration mock instance
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "DefaultPasswords:RegisteredUser")]).Returns("M0ckP$$word");
		mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "DefaultPasswords:Administrator")]).Returns("M0ckP$$word");
        
        // Create a ApplicationDbContext instance using the in-memory Db
        using var context = new ApplicationDbContext(options);
		
		// Create a RoleManager Instance
		var roleManager = IdentityHelper.GetRoleManager(new RoleStore<IdentityRole>(context));
		var userManager = IdentityHelper.GetUserManager(new UserStore<ApplicationUser>(context));
		
		// Create a SeedController instance
		var controller = new SeedController(context, roleManager, userManager, mockEnv, mockConfiguration.Object);
		
		// Define the variables for the users we want to test
		ApplicationUser user_Admin = null!;
		ApplicationUser user_User = null!;
		ApplicationUser user_NotExisting = null!;
		
		// Act
		// Execute the SeedController's CreateDefaultUsers() method to create default users (and roles)
		await controller.CreateDefaultUsers();
		user_Admin = await userManager.FindByEmailAsync("admin@email.com");
		user_User = await userManager.FindByEmailAsync("user@email.com");
		user_NotExisting = await userManager.FindByEmailAsync("notexisting@email.com");
		
		// Assert
		Assert.NotNull(user_Admin);
		Assert.NotNull(user_User);
		Assert.NotNull(user_NotExisting);
	}
}
