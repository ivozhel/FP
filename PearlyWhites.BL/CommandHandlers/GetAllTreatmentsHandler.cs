using MediatR;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;
using PearlyWhites.Models.Models.Responses;
using PearlyWhites.Models.Models.Tools;

namespace PearlyWhites.BL.CommandHandlers
{
    public class GetAllTreatmentsHandler : IRequestHandler<GetAllTretmentsCommand, BaseResponse<IEnumerable<Treatment>>>
    {
        private readonly ITreatmentsRepository _tretmentsRepository;

        public GetAllTreatmentsHandler(ITreatmentsRepository tretmentsRepository)
        {
            _tretmentsRepository = tretmentsRepository;
        }

        public async Task<BaseResponse<IEnumerable<Treatment>>> Handle(GetAllTretmentsCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<Treatment>>();

            var treatments = await _tretmentsRepository.GetAllTreatments();
            var count = treatments.Count();

            if (treatments is null)
            {
                response.Message = ResponseMessages.SomethingWentWrong;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return response;
            }

            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = ResponseMessages.Success;
            response.Respone = treatments;
            return response;
        }
    }
}
