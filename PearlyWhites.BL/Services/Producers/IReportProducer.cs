namespace PearlyWhites.BL.Services.Producers
{
    public interface IReportProducer
    {
        public Task<string> DailyReport(DateTime date);
    }
}
