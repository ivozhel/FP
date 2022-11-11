using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PearlyWhites.BL.Services.Consumers;
using PearlyWhites.Caches.KafkaService;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.DL.Repositories.MongoRepos.Interfaces;
using PearlyWhites.Models.Models.Configurations;
using PearlyWhites.Models.Models.KafkaModels;

namespace PearlyWhites.BL.Services.HostedServices
{
    public class ReportConsumeHostedService : IHostedService
    {
        private readonly KafkaConsumer<Guid, KafkaReport> _reportConsumer;
        private readonly IOptions<KafkaConfiguration> _options;
        private readonly IVisitRepository _visitRepository;
        private readonly IReportRepository _reportRepository;
        public ReportConsumeHostedService(IOptions<KafkaConfiguration> options, IVisitRepository visitRepository, IReportRepository reportRepository)
        {
            _visitRepository = visitRepository;
            _reportRepository = reportRepository;
            _options = options;
            _reportConsumer = new ReportConsumer(_options, _visitRepository, _reportRepository);
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
