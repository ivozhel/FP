using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Options;
using PearlyWhites.Caches.KafkaService;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.DL.Repositories.MongoRepos.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Configurations;
using PearlyWhites.Models.Models.KafkaModels;

namespace PearlyWhites.BL.Services.Consumers
{
    public class ReportConsumer : KafkaConsumer<Guid, KafkaReport>
    {
        private IOptions<KafkaConfiguration> _options;
        private readonly TransformBlock<KafkaReport, Report> _reportTransformBlock;
        private readonly ActionBlock<Report> actionBlock;
        private readonly IVisitRepository _visitRepository;
        private readonly IReportRepository _reportRepository;
        public ReportConsumer(IOptions<KafkaConfiguration> options, IVisitRepository visitRepository, IReportRepository reportRepository) : base(options)
        {
            _visitRepository = visitRepository;
            _reportRepository = reportRepository;
            _options = options;
            _reportTransformBlock = new TransformBlock<KafkaReport, Report>(async rep =>
            {
                var visitIds = rep.DailyVisitIds;
                var tasks = visitIds.Select(x => GetVisitPaidAmount(x));
                var prices = await Task.WhenAll(tasks);
                var total = prices.Sum();

                var report = new Report()
                {
                    Name = rep.Name,
                    Total = total,
                    Date = rep.Date
                };
                return report;

            });
            actionBlock = new ActionBlock<Report>(async rep =>
            {
                await _reportRepository.AddReport(rep);
            });
            _reportTransformBlock.LinkTo(actionBlock);
        }

        public override Task Consume(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = base._consumer.Consume();
                    _reportTransformBlock.Post(result.Value);
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        private async Task<decimal> GetVisitPaidAmount(Guid visitId)
        {
            var visit = await _visitRepository.GetVisitById(visitId);
            return visit.Paid;
        }

    }
}
