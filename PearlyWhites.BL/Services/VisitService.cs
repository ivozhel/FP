using AutoMapper;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.DL.Repositories.MongoRepos.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Mongo;
using PearlyWhites.Models.Models.Requests.Visit;
using PearlyWhites.Models.Models.Responses;
using PearlyWhites.Models.Models.Tools;

namespace PearlyWhites.BL.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _visitRepository;
        private readonly ITreatmentsRepository _treatmentsRepository;
        private readonly ITeethRepository _teethRepository;
        private readonly IMapper _mapper;

        public VisitService(IVisitRepository visitRepository, IMapper mapper, ITreatmentsRepository treatmentsRepository, ITeethRepository teethRepository)
        {
            _visitRepository = visitRepository;
            _mapper = mapper;
            _treatmentsRepository = treatmentsRepository;
            _teethRepository = teethRepository;
        }

        public async Task<BaseResponse<Visit>> Create(VisitRequest visit, string clinicName)
        {
            var response = new BaseResponse<Visit>();

            var toCreate = new Visit();
            toCreate.Id = Guid.NewGuid();
            toCreate.PatientId = visit.PatientId;
            toCreate.ClinicName = clinicName;
            toCreate.VisitDate = DateTime.Now;
            var visitTreatmentTasks = visit.VisitTreatments.Select(x => GetVisitTreatments(x));
            var visitTreatments = await Task.WhenAll(visitTreatmentTasks);
            toCreate.VisitTreatments = visitTreatments.ToList();

            var created = await _visitRepository.Create(toCreate);
            if (created is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.Message = ResponseMessages.SomethingWentWrong;
                return response;
            }

            var updateTeethTasks = created.VisitTreatments.Select(x => UpdatePatientTeeth(x));
            await Task.WhenAll();

            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message += ResponseMessages.Successfull(nameof(Create), typeof(Visit).Name);
            response.Respone = created;
            return response;

        }

        public Task Delete(Guid id)
        {
            return _visitRepository.Delete(id);
        }

        public async Task<BaseResponse<IEnumerable<Visit>>> GetAll()
        {
            var response = new BaseResponse<IEnumerable<Visit>>();
            var visits = await _visitRepository.GetAll();
            if (visits is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.Message = ResponseMessages.SomethingWentWrong;
                return response;
            }
            response.Message += ResponseMessages.Success;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Respone = visits;
            return response;

        }

        public async Task<BaseResponse<IEnumerable<Visit>>> GetPatientVisits(int patientId)
        {
            var response = new BaseResponse<IEnumerable<Visit>>();
            if (patientId <= 0)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = ResponseMessages.FalseId;
                return response;
            }

            var visits = await _visitRepository.GetPatientVisits(patientId);
            if (visits is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.Message = ResponseMessages.SomethingWentWrong;
                return response;
            }
            response.Message += ResponseMessages.Success;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Respone = visits;
            return response;
        }

        public async Task<BaseResponse<Visit>> GetVisitById(Guid id)
        {
            var response = new BaseResponse<Visit>();
            var visits = await _visitRepository.GetVisitById(id);
            if (visits is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.Message = ResponseMessages.SomethingWentWrong;
                return response;
            }
            response.Message += ResponseMessages.Success;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Respone = visits;
            return response;
        }

        public async Task<BaseResponse<decimal>> PayVisit(Guid visitId, decimal money)
        {
            var response = new BaseResponse<decimal>();
            var visit = await _visitRepository.GetVisitById(visitId);
            if (visit is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.Message = ResponseMessages.SomethingWentWrong;
                return response;
            }

            var toPay = visit.VisitTreatments.Select(x => x.Treatments.Sum(y => y.Price)).Sum();

            if (toPay > money)
            {
                visit.Paid += money;
                var payParial = await _visitRepository.PayVisit(visit);
                if (payParial is null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    response.Message = ResponseMessages.SomethingWentWrong;
                    return response;
                }
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Message = $"You made partial pay for visit with id {visit.Id} you have to pay {toPay - money} $ more";
                response.Respone = 0;
                return response;
            }

            visit.Paid = toPay;
            var pay = await _visitRepository.PayVisit(visit);
            if (pay is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.Message = ResponseMessages.SomethingWentWrong;
                return response;
            }
            response.Message = $"You paid succssesfully";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Respone = money - toPay;
            return response;
        }

        public async Task<BaseResponse<Visit>> UpdateVisit(VisitUpdateRequest visit)
        {
            var response = new BaseResponse<Visit>();
            var toUp = await _visitRepository.GetVisitById(visit.Id);
            if (toUp is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.Message = ResponseMessages.SomethingWentWrong;
                return response;
            }
            if (toUp.Paid == toUp.VisitTreatments.Sum(x => x.Treatments.Sum(y => y.Price)))
            {
                response.StatusCode = System.Net.HttpStatusCode.Forbidden;
                response.Message = ResponseMessages.SomethingWentWrong;
                return response;
            }


            var taskTreatments = visit.VisitTreatments.Select(x => GetVisitTreatments(x));
            var resultTreatments = await Task.WhenAll(taskTreatments);
            toUp.VisitTreatments = resultTreatments.ToList();
            toUp.PatientId = visit.PatientId;

            var updated = await _visitRepository.UpdateVisit(toUp);
            if (updated is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.Message = ResponseMessages.SomethingWentWrong;
                return response;
            }

            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = ResponseMessages.Success;
            response.Respone = updated;
            return response;


        }

        private async Task<VisitTreatment> GetVisitTreatments(VisitTreatmentRequest visitTreatmentRequest)
        {
            var treatmentsTask = visitTreatmentRequest.TreatmentId.Select(x => GetTreatments(x));
            var treatments = await Task.WhenAll(treatmentsTask);

            if (visitTreatmentRequest.ToothId is not null)
            {
                var tooth = await _teethRepository.GetToothById((Guid)visitTreatmentRequest.ToothId);
                return new VisitTreatment() { Tooth = tooth, Treatments = treatments.ToList() };
            }
            return new VisitTreatment() { Treatments = treatments.ToList() };
        }
        private async Task<Treatment> GetTreatments(int treatmentId)
        {
            return await _treatmentsRepository.GetTreatmentById(treatmentId);
        }
        private async Task UpdatePatientTeeth(VisitTreatment visitTreatment)
        {
            var tooth = await _teethRepository.GetToothById(visitTreatment.Tooth.Id);
            tooth.Treatments.AddRange(visitTreatment.Treatments);
            await _teethRepository.UpdateTooth(tooth);
        }
    }
}
