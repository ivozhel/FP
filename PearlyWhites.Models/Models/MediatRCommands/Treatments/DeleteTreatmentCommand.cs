using MediatR;

namespace PearlyWhites.Models.Models.MediatRCommands.Treatments
{
    public record DeleteTreatmentCommand(int treatmentId) : IRequest<bool>
    {
    }
}
