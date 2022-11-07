using MediatR;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.BL.CommandHandlers
{
    public class DeleteTreatmentHandler : IRequestHandler<DeleteTreatmentCommand, BaseResponse<bool>>
    {
        private readonly ITreatmentsRepository _treatmentsRepository;

        public DeleteTreatmentHandler(ITreatmentsRepository treatmentsRepository)
        {
            _treatmentsRepository = treatmentsRepository;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteTreatmentCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            var toDel = await _treatmentsRepository.GetTreatmentById(request.treatmentId);
            if (toDel is null)
            {
                response.Message = "Treatment with this id dose not exist";
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return response;
            }

            var isDeleted = await _treatmentsRepository.DeleteTreatmentById(request.treatmentId);
            if (isDeleted)
            {
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Message = "Succsesfully deleted treatment";
                return response;
            }

            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            response.Message = "Something went wrong try again";
            return response;
        }
    }
}
