using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace PearlyWhites.Host.Extensions.SwaggerExamples
{
    public class PatientToUpdateSwaggerExample : IMultipleExamplesProvider<Patient>
    {
        public IEnumerable<SwaggerExample<Patient>> GetExamples()
        {
            yield return SwaggerExample.Create("example",
                new Patient()
                {
                    Name = null,
                    Age = null
                });
        }
    }
}
