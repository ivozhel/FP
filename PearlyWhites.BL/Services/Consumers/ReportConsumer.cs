using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Options;
using PearlyWhites.Caches.KafkaService;
using PearlyWhites.DL.Repositories.Interfaces;
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
        private readonly ITreatmentsRepository _treatmentsRepository;
        private readonly IReportRepository _reportRepository;
        public ReportConsumer(IOptions<KafkaConfiguration> options, ITreatmentsRepository treatmentsRepository, IReportRepository reportRepository) : base(options)
        {
            _treatmentsRepository = treatmentsRepository;
            _reportRepository = reportRepository;
            _options = options;
            _reportTransformBlock = new TransformBlock<KafkaReport, Report>(async rep =>
            {
                var treatmentIds = rep.DailyTreatmentIds;
                var tasks = treatmentIds.Select(x => TreatmentPrices(x));
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

        private async Task<decimal> TreatmentPrices(int treatmentId)
        {
            var treatment = await _treatmentsRepository.GetTreatmentById(treatmentId);
            if (treatment is null)
            {
                return 0;
            }
            return treatment.Price;
        }
    }
}
