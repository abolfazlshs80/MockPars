

using Microsoft.Extensions.DependencyInjection;
using MockPars.Application.Services.Implementation;
using MockPars.Application.Services.Interfaces;

namespace MockPars.Application
{
 

   


    public static class ApplicationServiceRegistration
    {
        
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            #region Service
       
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<ITableService, TableService>();
            services.AddScoped<IColumnService, ColumnService>();
            services.AddScoped<IRecordDataService, RecordDataService>();
            services.AddScoped<IFakeService, FakeService>();

            #endregion

       
            return services;
         
        }
    }
}


    


        
   


