using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PearlyWhites.BL.Services.Consumers;
using PearlyWhites.Caches.KafkaService;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models.Configurations;
using PearlyWhites.Models.Models.KafkaModels;

namespace PearlyWhites.BL.Services.HostedServices
{
    public class ReportConsumeHostedService : IHostedService
    {
        private readonly KafkaConsumer<Guid, KafkaReport> _reportConsumer;
        private readonly IOptions<KafkaConfiguration> _options;
        private readonly ITreatmentsRepository _treatmentsRepository;
        private readonly IReportRepository _reportRepository;
        public ReportConsumeHostedService(IOptions<KafkaConfiguration> options, ITreatmentsRepository treatmentsRepository, IReportRepository reportRepository)
        {
            _treatmentsRepository = treatmentsRepository;
            _reportRepository = reportRepository;
            _options = options;
            _reportConsumer = new ReportConsumer(_options, _treatmentsRepository, _reportRepository);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _reportConsumer.Consume(cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
