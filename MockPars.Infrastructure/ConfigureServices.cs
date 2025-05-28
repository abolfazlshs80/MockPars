using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MockPars.Domain.Interface;
using MockPars.Infrastructure.Context;
using MockPars.Infrastructure.Models.Jwt;
using MockPars.Infrastructure.Repositories;
using MockPars.Infrastructure.Service.Authebntication;
using MockPars.Infrastructure.Service.Jwt;
//using MockPars.Infrastructure.Repositories;

namespace MockPars.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region AppSetting Bind

        services.Configure<ConfigJwtDto>(options =>
            configuration.GetSection("Jwt").Bind(options));

        #endregion

        #region DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("LocalDb")));
        #endregion


        #region Repository

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDatabasesRepository, DatabasesRepository>();
        services.AddScoped<ITablesRepository, TablesRepository>();
        services.AddScoped<IColumnsRepository, ColumnsRepository>();
        services.AddScoped<IRecordDataRepository, RecordDataRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        #endregion

        #region Services

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        #endregion


        return services;
    }

}