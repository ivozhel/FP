using MediatR;

namespace PearlyWhites.Models.Models.MediatRCommands.Treatments
{
    public record GetByIdTreatmentCommand(int treatmentId) : IRequest<Treatment>
    {

    }
}
