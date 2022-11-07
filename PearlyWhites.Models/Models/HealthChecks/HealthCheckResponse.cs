namespace PearlyWhites.Models.Models.HealtChecks
{
    public class HealthCheckResponse
    {
        public string Status { get; init; }

        public IEnumerable<IndividualHealthCheckResponse> HealtChecks { get; init; }

        public TimeSpan HealthCheckDuration { get; init; }
    }
}
