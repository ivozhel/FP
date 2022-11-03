using MediatR;
using PearlyWhites.Models.Models.Requests;

namespace PearlyWhites.Models.Models.MediatRCommands.Treatments
{
    public record AddTreatmentCommand(TreatmentRequest treatment) : IRequest<Treatment>
    {
    }
}
