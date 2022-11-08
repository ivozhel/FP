using MediatR;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;
using PearlyWhites.Models.Models.Responses;
using PearlyWhites.Models.Models.Tools;

namespace PearlyWhites.BL.CommandHandlers
{
    public class UpdateTreatmentHandler : IRequestHandler<UpdateTreatmentCommand, BaseResponse<Treatment>>
    {
        private readonly ITreatmentsRepository _treatmentsRepository;

        public UpdateTreatmentHandler(ITreatmentsRepository treatmentsRepository)
        {
            _treatmentsRepository = treatmentsRepository;
        }

        public async Task<BaseResponse<Treatment>> Handle(UpdateTreatmentCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Treatment>();
            var toUp = await _treatmentsRepository.GetTreatmentById(request.treatment.Id);
            
            if (toUp is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.Message = ResponseMessages.NotFound(typeof(Treatment).Name);
                return response;
            }

            var updated = await _treatmentsRepository.UpdateTreatment(request.treatment);
            if (updated is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.Message = ResponseMessages.SomethingWentWrong;
                return response;
            }

            response.Message = ResponseMessages.Success;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Respone = updated;
            return response;

        }
    }
}
