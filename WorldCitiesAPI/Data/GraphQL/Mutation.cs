using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Data.Models;

namespace WorldCitiesAPI.Data.GraphQL;

public class Mutation {
	
	/// <summary>
	/// Add a new city
	/// </summary>
	[Serial]
	[Authorize(Roles = new[] { "RegisteredUser" })]
	public async Task<City> AddCity([Service] ApplicationDbContext context, CityDTO cityDTO) {
		City city = new City() {
			Name = cityDTO.Name,
			Lat = cityDTO.Lat,
			Lon = cityDTO.Lon,
			CountryId = cityDTO.CountryId
		};
		context.Cities.Add(city);
		await context.SaveChangesAsync();
		return city;
	}
	
	/// <summary>
	/// Update an existing city
	/// </summary>
	[Serial]
	[Authorize(Roles = new[] { "RegisteredUser" })]
	public async Task<City> UpdateCity([Service] ApplicationDbContext context, CityDTO cityDTO) {
		City? city = await context.Cities.Where(c => c.Id == cityDTO.Id).FirstOrDefaultAsync();
		if (city == null) throw new NotSupportedException();
		city.Name = cityDTO.Name;
		city.Lat = cityDTO.Lat;
		city.Lon = cityDTO.Lon;
		city.CountryId = cityDTO.CountryId;
		context.Cities.Update(city);
		await context.SaveChangesAsync();
		return city;
	}
	
	/// <summary>
	/// Delete a city.
	/// </summary>
	[Serial]
	[Authorize(Roles = new[] { "Administrator" })]
	public async Task DeleteCity([Service] ApplicationDbContext context, int id) {
		City? city = await context.Cities.Where(c => c.Id == id).FirstOrDefaultAsync();
		if (city != null) {
			context.Cities.Remove(city);
			await context.SaveChangesAsync();
		}
	}
	
	/// <summary>
	/// Add a new country.
	/// </summary>
	[Serial]
	[Authorize(Roles = new[] { "Administrator" })]
	public async Task<Country> AddCountry([Service] ApplicationDbContext context, CountryDTO countryDTO) {
		Country country = new Country() {
			Name = countryDTO.Name,
			ISO2 = countryDTO.ISO2,
			ISO3 = countryDTO.ISO3
		};
		context.Countries.Add(country);
		await context.SaveChangesAsync();
		return country;
	}
	
	/// <summary>
	/// Update an existing country
	/// </summary>
	[Serial]
	[Authorize(Roles = new[] { "Administrator" })]
	public async Task<Country> UpdateCountry([Service] ApplicationDbContext context, CountryDTO countryDTO) {
		Country? country = await context.Countries.Where(c => c.Id == countryDTO.Id).FirstOrDefaultAsync();
		if (country == null) throw new NotSupportedException();
		country.Name = countryDTO.Name;
		country.ISO2 = countryDTO.ISO2;
		country.ISO3 = countryDTO.ISO3;
		context.Countries.Update(country);
		await context.SaveChangesAsync();
		return country;
	}
	
	/// <summary>
	/// Delete a country
	/// </summary>
	[Serial]
	[Authorize(Roles = new[] { "Administrator" })]
	public async Task DeleteCountry([Service] ApplicationDbContext context, int id) {
		Country? country = await context.Countries.Where(c => c.Id == id).FirstOrDefaultAsync();
		if (country != null) {
			context.Countries.Remove(country);
			await context.SaveChangesAsync();
		}
	}
}
