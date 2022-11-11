using FluentValidation;
using PearlyWhites.Models.Models.Requests.Visit;

namespace PearlyWhites.Host.Validators
{
    public class VisitRequestValidations : AbstractValidator<VisitRequest>
    {
        public VisitRequestValidations()
        {
            RuleFor(x => x.VisitTreatments).NotEmpty();
            RuleFor(x => x.PatientId).NotEmpty();
        }
    }
}
