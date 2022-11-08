using AutoMapper;
using Microsoft.Extensions.Logging;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.DL.Repositories;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests.Patient;
using PearlyWhites.Models.Models.Requests.Tooth;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.BL.Services
{
    public class PatientService : IPatientService
    {
        private readonly IToothRepository _toothRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly ITeethAndTreatmentRepository _tethAndTreatmentRepository;
        private readonly ITreatmentsRepository _treatmentRepository;
        public PatientService(IPatientRepository patientRepository, IMapper mapper, IToothRepository toothRepository, ITeethAndTreatmentRepository tethAndTreatmentRepository, ITreatmentsRepository treatmentRepository)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _toothRepository = toothRepository;
            _tethAndTreatmentRepository = tethAndTreatmentRepository;
            _treatmentRepository = treatmentRepository;
        }

        public async Task<BaseResponse<Patient>> Create(PatientRequest patient)
        {
            var response = new BaseResponse<Patient>();
            var toBeCreated = _mapper.Map<Patient>(patient);

            var created = await _patientRepository.Create(toBeCreated);

            if (created is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.Message = "Something went wrong Patient was not created try again";
                return response;
            }

            var tasks = patient.Teeth.Select(x => CreateTeeth(x, created, response));

            var result = await Task.WhenAll(tasks);

            if (result.Any(x => x == false))
            {
                await DeletePatientById(created.Id);
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "Something went wrong Patient was not created try again";
                return response;
            }

            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message += "Patient created succsessfuly";
            response.Respone = created;
            return response;
        }

        public async Task<BaseResponse<bool>> DeletePatientById(int id)
        {
            var response = new BaseResponse<bool>();

            if (id <= 0)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "Id cannot be 0 or less";
                return response;
            }

            var patientToDelete = await _patientRepository.GetPatientById(id);
            if (patientToDelete is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.Message = "Patient not found";
                response.Respone = false;
                return response;
            }
            var isPatientTeethDeleted = await _toothRepository.DeletePatientTeeth(id);
            var isPatientDeleted = await _patientRepository.DeletePatientById(id);

            if (isPatientDeleted && isPatientTeethDeleted)
            {
                response.Respone = true;
                response.Message = "Succsessfully deleted patient";
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return response;
            }

            response.Message = "Something went wrong";
            response.Respone = false;
            response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            return response;
        }

        public async Task<BaseResponse<IEnumerable<Patient>>> GetAllPatients()
        {
            var response = new BaseResponse<IEnumerable<Patient>>();

            var patients = await _patientRepository.GetAllPatients();

            if (patients is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.Message = "Something went wrong";
                response.Respone = null;
                return response;
            }

            var tasks = patients.Select(x => GetTeethForPatient(x, response));
            await Task.WhenAll(tasks);

            response.Message += "Patients loaded";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Respone = patients;
            return response;
        }

        public async Task<BaseResponse<Patient>> GetPatientById(int id)
        {
            var response = new BaseResponse<Patient>();

            if (id <= 0)
            {
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.Message = "Id cannot be 0 or less";
                return response;
            }

            var patient = await _patientRepository.GetPatientById(id);
            if (patient is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.Message = "Patient not found";
                response.Respone = null;
                return response;
            }

            var teeth = await _toothRepository.GetPatientTeeth(patient.Id);
            if (teeth is not null)
            {
                patient.Teeth = teeth.ToList();
            }
            else
            {
                response.Message = $"Teeth can not be loaded for Patient with ID {patient.Id}";
            }

            response.Message += "Patient loaded";
            response.Respone = patient;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }

        public async Task<BaseResponse<Patient>> UpdatePatient(PatientUpdateRequest patientReq)
        {
            var response = new BaseResponse<Patient>();
            var patient = _mapper.Map<Patient>(patientReq);

            var toUp = await _patientRepository.GetPatientById(patient.Id);
            if (toUp is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.Message = "Patient not found";
                response.Respone = null;
                return response;
            }
            var updated = await _patientRepository.UpdatePatient(patient);
            if (updated is null)
            {
                response.Message = "Something went wrong";
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return response;
            }
            response.Message = "Patient updated";
            response.Respone = updated;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;

        }

        private async Task<bool> CreateTeeth(ToothRequest tooth, Patient created, BaseResponse<Patient> response)
        {
            var toothToCreate = _mapper.Map<Tooth>(tooth);
            toothToCreate.PatientId = created.Id;
            var createdTooth = await _toothRepository.Create(toothToCreate);
            if (createdTooth is null)
            {
                return false;
            }
            var tasks = tooth.TreatmentIds.Select(x => AddTreatments(response, x, createdTooth));
            await Task.WhenAll(tasks);

            created.Teeth.Add(createdTooth);
            return true;
        }

        private async Task AddTreatments(BaseResponse<Patient> response, int treatmentId, Tooth createdTooth)
        {
            var treatment = await _treatmentRepository.GetTreatmentById(treatmentId);
            if (treatment is null)
            {
                response.Message += $"Treatment with ID {treatmentId} dose not exist. Treatment did not add to tooth with ID {createdTooth.Id};";
                response.Message += Environment.NewLine;
                return;
            }
            createdTooth.Treatments.Add(treatment);
            await _tethAndTreatmentRepository.Create(treatmentId, createdTooth.Id);
        }

        private async Task GetTeethForPatient(Patient patient, BaseResponse<IEnumerable<Patient>> response)
        {
            var teeth = await _toothRepository.GetPatientTeeth(patient.Id);
            if (teeth is null)
            {
                response.Message += $"Teeth for Patient with id {patient.Id} not found; ";
                return;
            }
            patient.Teeth = teeth.ToList();
        }
    }
}
