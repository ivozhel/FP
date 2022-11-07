using MediatR;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.Models.Models.MediatRCommands.Treatments
{
    public record GetAllTretmentsCommand : IRequest<BaseResponse<IEnumerable<Treatment>>>
    {
    }
}
