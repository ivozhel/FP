using PearlyWhites.BL.Services;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.DL.Repositories;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models.Requests;

namespace PearlyWhites.Host.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepos(this IServiceCollection services)
        {
            services.AddSingleton<IPatientRepository, PatientRepository>();
            services.AddSingleton<IToothRepository, ToothRepository>();
            services.AddSingleton<ITreatmentsRepository, TreatmentsRepository>();
            return services;
        }
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IPatientService, PatientService>();

            return services;
        }
    }
}
