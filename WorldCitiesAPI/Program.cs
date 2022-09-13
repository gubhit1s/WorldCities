using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Data;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using WorldCitiesAPI.Data.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.MSSqlServer(connectionString: ctx.Configuration.GetConnectionString("DefaultConnection"),
        restrictedToMinimumLevel: LogEventLevel.Information,
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "LogEvents",
            AutoCreateSqlTable = true
        })
    .WriteTo.Console()
);

// Add services to the container.
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // options.JsonSerializerOptions.WriteIndented = true;
            // options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add ApplicationDbContext and SQL Server support
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        )
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.SignIn.RequiredConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphaNumeric = true;
    options.Password.RequireLength = 8;
});

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
