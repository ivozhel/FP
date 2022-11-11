using FluentValidation;
using PearlyWhites.Models.Models.Requests.Visit;

namespace PearlyWhites.Host.Validators
{
    public class TreatmentValidations : AbstractValidator<TreatmentRequest>
    {
        public TreatmentValidations()
        {
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
