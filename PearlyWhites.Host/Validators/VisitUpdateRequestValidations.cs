using FluentValidation;
using PearlyWhites.Models.Models.Requests.Visit;

namespace PearlyWhites.Host.Validators
{
    public class VisitUpdateRequestValidations : AbstractValidator<VisitUpdateRequest>
    {
        public VisitUpdateRequestValidations()
        {
            RuleFor(x => x.VisitTreatments).NotEmpty();
            RuleFor(x => x.PatientId).NotEmpty().GreaterThan(0);
        }
    }
}
