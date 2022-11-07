using FluentValidation;
using PearlyWhites.Models.Models;

namespace PearlyWhites.Host.Validators
{
    public class PatientValidations : AbstractValidator<Patient>
    {
        public PatientValidations()
        {
            RuleFor(x => x.Age).GreaterThan(0).LessThan(200);
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        }
    }
}
