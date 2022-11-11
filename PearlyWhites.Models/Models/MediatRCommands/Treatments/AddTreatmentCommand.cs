using MediatR;
using PearlyWhites.Models.Models.Requests.Visit;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.Models.Models.MediatRCommands.Treatments
{
    public record AddTreatmentCommand(TreatmentRequest treatment) : IRequest<BaseResponse<Treatment>>
    {
    }
}
