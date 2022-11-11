using PearlyWhites.BL.Services;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.BL.Services.Producers;
using PearlyWhites.DL.Repositories;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.DL.Repositories.MongoRepos;
using PearlyWhites.DL.Repositories.MongoRepos.Interfaces;

namespace PearlyWhites.Host.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepos(this IServiceCollection services)
        {
            services.AddSingleton<IPatientRepository, PatientRepository>();
            services.AddSingleton<ITeethRepository, TeethRepository>();
            services.AddSingleton<ITreatmentsRepository, TreatmentsRepository>();
            services.AddSingleton<IReportRepository, ReportRepository>();
            services.AddSingleton<IVisitRepository, VisitRepository>();
            return services;
        }
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IPatientService, PatientService>();
            services.AddSingleton<IReportProducer, ReportProducer>();
            services.AddSingleton<IReportService, ReportService>();
            services.AddSingleton<IVisitService, VisitService>();

            return services;
        }
    }
}
