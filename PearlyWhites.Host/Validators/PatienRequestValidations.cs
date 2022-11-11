using FluentValidation;
using PearlyWhites.Models.Models.Requests.Patient;

namespace PearlyWhites.Host.Validators
{
    public class PatienRequestValidations : AbstractValidator<PatientRequest>
    {
        public PatienRequestValidations()
        {
            RuleFor(x => x.Age).NotEmpty().GreaterThan(0).LessThan(200);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100).MinimumLength(2);
        }
    }
}
