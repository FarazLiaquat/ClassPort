using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClassPort.Domain.Entities.Identity;
using ClassPort.Infrastructure.Identity.Services;
using ClassPort.Infrastructure.Persistence.DbContexts;

namespace ClassPort.Infrastructure.Identity;

public static class Startup
{
	/// <summary>
	///     Add custom identity user, roles, etc.
	/// </summary>
	public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
	{
		services.AddIdentity<ApplicationUser, ApplicationRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddClaimsPrincipalFactory<ApplicationClaimsPrincipalFactory>()
			.AddDefaultTokenProviders();
	}

	public static void Configure(IApplicationBuilder app, IConfiguration configuration)
	{
	}
}

