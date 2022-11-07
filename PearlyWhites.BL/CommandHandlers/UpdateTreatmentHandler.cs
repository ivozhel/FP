using MediatR;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;
using PearlyWhites.Models.Models.Responses;

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
                response.Message = $"Treatment with id {request.treatment.Id} dose not exist";
                return response;
            }

            var updated = await _treatmentsRepository.UpdateTreatment(request.treatment);
            if (updated is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "Something went wrong try again";
                return response;
            }

            response.Message = "Treatment succsessfully updated";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Respone = updated;
            return response;

        }
    }
}
