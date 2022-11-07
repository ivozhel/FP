using MediatR;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.Models.Models.MediatRCommands.Treatments
{
    public record GetByIdTreatmentCommand(int treatmentId) : IRequest<BaseResponse<Treatment>>
    {

    }
}
