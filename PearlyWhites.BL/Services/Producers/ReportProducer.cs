using Microsoft.Extensions.Options;
using PearlyWhites.Caches.KafkaService;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models.Configurations;
using PearlyWhites.Models.Models.KafkaModels;

namespace PearlyWhites.BL.Services.Producers
{
    public class ReportProducer : KafkaProducer<Guid, KafkaReport> , IReportProducer
    {
        private readonly ITeethAndTreatmentRepository _teethAndTreatmentRepository;
        private readonly IOptions<KafkaConfiguration> _options;
        private readonly IReportRepository _reportRepository;
        public ReportProducer(IOptions<KafkaConfiguration> options, ITeethAndTreatmentRepository teethAndTreatmentRepository, IReportRepository reportRepository) : base(options)
        {
            _options = options;
            _teethAndTreatmentRepository = teethAndTreatmentRepository;
            _reportRepository = reportRepository;
        }

        public async Task<string> DailyReport(DateTime date)
        {
            var report = new KafkaReport()
            {
                Id = Guid.NewGuid()
            };
            var dayliTreatments = await _teethAndTreatmentRepository.GetTreatmentDayliReport(date);

            report.Date = date;
            report.Name = "";
            report.DailyTreatmentIds = dayliTreatments;

            var ifAlreadySend = await _reportRepository.CheckIfExists(report.Name, report.Date);
            if (ifAlreadySend)
            {
                return "Report already send";
            }

            await base.Produce( report, report.Id);
            return "Report succesfully send";
        }

    }
}
