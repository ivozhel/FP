using MediatR;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;
using PearlyWhites.Models.Models.Responses;
using PearlyWhites.Models.Models.Tools;

namespace PearlyWhites.BL.CommandHandlers
{
    public class GetByIdTreatmentHandler : IRequestHandler<GetByIdTreatmentCommand, BaseResponse<Treatment>>
    {
        private readonly ITreatmentsRepository _treatmentsRepository;

        public GetByIdTreatmentHandler(ITreatmentsRepository treatmentsRepository)
        {
            _treatmentsRepository = treatmentsRepository;
        }

        public async Task<BaseResponse<Treatment>> Handle(GetByIdTreatmentCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Treatment>();
            if (request.treatmentId <= 0)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = ResponseMessages.FalseId;
                return response;
            }

            var treatment = await _treatmentsRepository.GetTreatmentById(request.treatmentId);

            if (treatment is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.Message = ResponseMessages.NotFound(typeof(Treatment).Name);
                return response;
            }

            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = ResponseMessages.Success;
            response.Respone = treatment;
            return response;
        }
    }
}
