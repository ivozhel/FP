using Microsoft.Extensions.Options;
using PearlyWhites.Caches.KafkaService;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.DL.Repositories.MongoRepos.Interfaces;
using PearlyWhites.Models.Models.Configurations;
using PearlyWhites.Models.Models.KafkaModels;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.BL.Services.Producers
{
    public class ReportProducer : KafkaProducer<Guid, KafkaReport>, IReportProducer
    {
        private readonly IVisitRepository _visitRepository;
        private readonly IOptions<KafkaConfiguration> _options;
        private readonly IReportRepository _reportRepository;
        public ReportProducer(IOptions<KafkaConfiguration> options, IReportRepository reportRepository, IVisitRepository visitRepository) : base(options)
        {
            _options = options;
            _reportRepository = reportRepository;
            _visitRepository = visitRepository;
        }

        public async Task<BaseResponse<string>> DailyReport(DateTime date, string address)
        {
            var response = new BaseResponse<string>();
            var report = new KafkaReport()
            {
                Id = Guid.NewGuid()
            };

            var dayVisits = await _visitRepository.GetAllOnDate(date, address);


            report.Date = date;
            report.Name = address;
            report.DailyVisitIds = dayVisits.Select(x => x.Id);

            var ifAlreadySend = await _reportRepository.CheckIfExists(report.Name, report.Date);
            if (ifAlreadySend)
            {
                response.Respone = "Report already send";
                response.StatusCode = System.Net.HttpStatusCode.Forbidden;
                return response;
            }

            await base.Produce(report, report.Id);
            response.Respone = "Report succesfully send";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

    }
}
