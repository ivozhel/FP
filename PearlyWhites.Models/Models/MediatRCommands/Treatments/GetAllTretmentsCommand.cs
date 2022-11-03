using MediatR;

namespace PearlyWhites.Models.Models.MediatRCommands.Treatments
{
    public record GetAllTretmentsCommand : IRequest<IEnumerable<Treatment>>
    {
    }
}
