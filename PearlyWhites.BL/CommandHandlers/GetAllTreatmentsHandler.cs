using MediatR;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;
using PearlyWhites.Models.Models.Responses;

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
                response.Message = "Something went wrong try again";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return response;
            }

            if (count == 0)
            {
                response.Message = "There are no treatments found";
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return response;
            }

            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = "Treatments loaded";
            response.Respone = treatments;
            return response;
        }
    }
}
