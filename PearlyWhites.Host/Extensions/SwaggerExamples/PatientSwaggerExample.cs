using PearlyWhites.Models.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace PearlyWhites.Host.Extensions.SwaggerExamples
{
    public class PatientSwaggerExample : IMultipleExamplesProvider<PatientRequest>
    {
        private readonly List<ToothRequest> _toothRequests = new List<ToothRequest>();
        public IEnumerable<SwaggerExample<PatientRequest>> GetExamples()
        {
            for (int i = 1; i < 33; i++)
            {
                _toothRequests.Add(new ToothRequest { Name = $"Zub{i}", Position = $"{i}" });
            }

            yield return SwaggerExample.Create("example",
                new PatientRequest()
                {
                    Name = "Dummy Patient",
                    Age = 33,
                    Teeth = _toothRequests
                }
                );
        }
    }
}
