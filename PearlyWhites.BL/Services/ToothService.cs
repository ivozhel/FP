using AutoMapper;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.DL.Repositories;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests.Tooth;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.BL.Services
{
    public class ToothService : IToothService
    {
        private readonly IToothRepository _toothRepository;
        private readonly ITreatmentsRepository _treatmentsRepository;
        private readonly ITeethAndTreatmentRepository _teethAndTreatmentRepository;
        private readonly IMapper _mapper;

        public ToothService(IToothRepository toothRepository, ITreatmentsRepository treatmentsRepository, ITeethAndTreatmentRepository teethAndTreatmentRepository, IMapper mapper)
        {
            _toothRepository = toothRepository;
            _treatmentsRepository = treatmentsRepository;
            _teethAndTreatmentRepository = teethAndTreatmentRepository;
            _mapper = mapper;
        }
        public async Task<BaseResponse<Tooth>> GetToothById(int id)
        {
            var response = new BaseResponse<Tooth>();

            if (id <= 0)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "Id cannot be 0 or less";
                return response;
            }
            var tooth = await _toothRepository.GetToothById(id);

            if (tooth is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.Message = "Tooth not found";
                response.Respone = null;
                return response;
            }

            var treatmentIds = await _teethAndTreatmentRepository.GetTreatmentIdsForTooth(tooth.Id);

            if (treatmentIds is null)
                response.Message = "Tooth have no treatments";
            else
            {
                var tasks = treatmentIds.Select(x => GetTreatments(response, x, tooth));
                await Task.WhenAll(tasks);
            }

            response.Message += "Tooth found";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Respone = tooth;
            return response;

        }

        public async Task<BaseResponse<Tooth>> UpdateTooth(ToothUpdateRequest toothReq)
        {
            var response = new BaseResponse<Tooth>();
            var tooth = _mapper.Map<Tooth>(toothReq);

            var toUp = await _toothRepository.GetToothById(tooth.Id);
            if (toUp is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.Message = "Tooth not found";
                response.Respone = null;
                return response;
            }

            var updated = await _toothRepository.UpdateTooth(tooth);
            if (updated is null)
            {
                response.Message = "Something went wrong";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return response;
            }

            var tasks = toothReq.TreatmentIds.Select(x => CreateTeethTreatments(response, x, updated));
            await Task.WhenAll(tasks);

            response.Message += "Tooth updated";
            response.Respone = toUp;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;

        }

        private async Task GetTreatments(BaseResponse<Tooth> response, int treatmentId, Tooth tooth)
        {
            var treatment = await _treatmentsRepository.GetTreatmentById(treatmentId);
            if (treatment is null)
                response.Message += $"Treatment with id {treatment} not found ";
            else
                tooth.Treatments.Add(treatment);
        }

        private async Task CreateTeethTreatments(BaseResponse<Tooth> response,int treatmentId, Tooth updated)
        {
            var toothTreatment = await _teethAndTreatmentRepository.Create(treatmentId, updated.Id);
            if (!toothTreatment)
            {
                response.Message += $"Treatment with ID {treatmentId} not found";
            }
        }
    }
}
