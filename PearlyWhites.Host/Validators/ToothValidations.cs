using FluentValidation;
using PearlyWhites.Models.Models.Requests;

namespace PearlyWhites.Host.Validators
{
    public class ToothValidations : AbstractValidator<ToothRequest>
    {
        public ToothValidations()
        {
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x => x.Position).NotEmpty();
            //RuleFor(x => x.TreatmentIds).Empty();

        }
    }
}
