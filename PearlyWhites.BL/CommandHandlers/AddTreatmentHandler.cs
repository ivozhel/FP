using AutoMapper;
using MediatR;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;
using PearlyWhites.Models.Models.Responses;
using PearlyWhites.Models.Models.Tools;

namespace PearlyWhites.BL.CommandHandlers
{
    public class AddTreatmentHandler : IRequestHandler<AddTreatmentCommand, BaseResponse<Treatment>>
    {
        private readonly IMapper _mapper;
        private readonly ITreatmentsRepository _treatmentsRepository;
        public AddTreatmentHandler(ITreatmentsRepository treatmentsRepository, IMapper mapper)
        {
            _treatmentsRepository = treatmentsRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Treatment>> Handle(AddTreatmentCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Treatment>();
            var checkIfExists = await _treatmentsRepository.CheckIfTreatmentExists(request.treatment);
            if (checkIfExists)
            {
                response.Message = ResponseMessages.AlreadyExists(typeof(Treatment).Name);
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return response;
            }

            var treatment = _mapper.Map<Treatment>(request.treatment);
            var created = await _treatmentsRepository.Create(treatment);

            if (created is null)
            {
                response.Message = ResponseMessages.SomethingWentWrong;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return response;
            }

            response.Message = ResponseMessages.Success;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Respone = created;
            return response;
        }
    }
}
