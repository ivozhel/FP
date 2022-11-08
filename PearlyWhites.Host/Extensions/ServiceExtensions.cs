using PearlyWhites.BL.Services;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.BL.Services.Producers;
using PearlyWhites.DL.Repositories;
using PearlyWhites.DL.Repositories.Interfaces;

namespace PearlyWhites.Host.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepos(this IServiceCollection services)
        {
            services.AddSingleton<IPatientRepository, PatientRepository>();
            services.AddSingleton<IToothRepository, ToothRepository>();
            services.AddSingleton<ITreatmentsRepository, TreatmentsRepository>();
            services.AddSingleton<ITeethAndTreatmentRepository, TeethAndTreatmentRepository>();
            services.AddSingleton<IReportRepository, ReportRepository>();
            return services;
        }
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IPatientService, PatientService>();
            services.AddSingleton<IToothService, ToothService>();
            services.AddSingleton<IReportProducer, ReportProducer>();
            services.AddSingleton<IReportService, ReportService>();

            return services;
        }
    }
}
