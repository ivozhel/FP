using MediatR;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.Models.Models.MediatRCommands.Treatments
{
    public record DeleteTreatmentCommand(int treatmentId) : IRequest<BaseResponse<bool>>
    {
    }
}
