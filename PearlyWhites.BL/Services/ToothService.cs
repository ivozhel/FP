using AutoMapper;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests.Tooth;
using PearlyWhites.Models.Models.Responses;
using PearlyWhites.Models.Models.Tools;

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
                response.Message = ResponseMessages.FalseId;
                return response;
            }
            var tooth = await _toothRepository.GetToothById(id);

            if (tooth is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.Message = ResponseMessages.NotFound(typeof(Tooth).Name);
                response.Respone = null;
                return response;
            }

            var treatmentIds = await _teethAndTreatmentRepository.GetTreatmentIdsForTooth(tooth.Id);

            if (treatmentIds is null)
                response.Message = ResponseMessages.DoseNotHave(typeof(Tooth).Name, typeof(Treatment).Name);
            else
            {
                var tasks = treatmentIds.Select(x => GetTreatments(response, x, tooth));
                await Task.WhenAll(tasks);
            }

            response.Message += ResponseMessages.Success;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Respone = tooth;
            return response;

        }

        public async Task<BaseResponse<Tooth>> Update(ToothUpdateRequest toothReq, string clinicName)
        {
            var response = new BaseResponse<Tooth>();
            var tooth = _mapper.Map<Tooth>(toothReq);

            var toUp = await _toothRepository.GetToothById(tooth.Id);
            if (toUp is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.Message = ResponseMessages.NotFound(typeof(Tooth).Name);
                response.Respone = null;
                return response;
            }

            var updated = await _toothRepository.UpdateTooth(tooth);
            if (updated is null)
            {
                response.Message = ResponseMessages.SomethingWentWrong;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return response;
            }

            var tasks = toothReq.TreatmentIds.Select(x => CreateTeethTreatments(response, x, updated, clinicName));
            await Task.WhenAll(tasks);

            response.Message += ResponseMessages.Success;
            response.Respone = toUp;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;

        }

        private async Task GetTreatments(BaseResponse<Tooth> response, int treatmentId, Tooth tooth)
        {
            var treatment = await _treatmentsRepository.GetTreatmentById(treatmentId);
            if (treatment is null)
                response.Message += ResponseMessages.NotFound(typeof(Treatment).Name);
            else
                tooth.Treatments.Add(treatment);
        }

        private async Task CreateTeethTreatments(BaseResponse<Tooth> response,int treatmentId, Tooth updated, string clinicName)
        {
            var toothTreatment = await _teethAndTreatmentRepository.Create(treatmentId, updated.Id, clinicName);
            if (!toothTreatment)
            {
                response.Message += ResponseMessages.NotFound(typeof(Treatment).Name);
            }
        }
    }
}
