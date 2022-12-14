using MediatR;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.Models.Models.MediatRCommands.Treatments
{
    public record UpdateTreatmentCommand(Treatment treatment) : IRequest<BaseResponse<Treatment>>
    {
    }
}
