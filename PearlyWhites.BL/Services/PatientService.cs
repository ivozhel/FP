using AutoMapper;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests;

namespace PearlyWhites.BL.Services
{
    public class PatientService : IPatientService
    {
        private readonly IToothRepository _toothRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        public PatientService(IPatientRepository patientRepository, IMapper mapper, IToothRepository toothRepository)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _toothRepository = toothRepository;
        }

        public async Task<Patient> Create(PatientRequest patient)
        {
            var teeth = new List<Tooth>();
            var toBeCreated = _mapper.Map<Patient>(patient);
            var created = await _patientRepository.Create(toBeCreated);
            foreach (var tooth in patient.Teeth)
            {
                var toothToCreate = _mapper.Map<Tooth>(tooth);
                toothToCreate.PatientId = created.Id;
                teeth.Add(await _toothRepository.Create(toothToCreate));
            }
            created.Teeth = teeth;
            return created;
        }

        public async Task DeletePatientById(int id)
        {
            var patientToDelete = await _patientRepository.GetPatientById(id);
            if (patientToDelete is null)
            {
                return;
            }
            await _toothRepository.DeletePatientTeeth(id);
            await _patientRepository.DeletePatientById(id);
        }

        public async Task<IEnumerable<Patient>> GetAllPatients()
        {
            var patients = await _patientRepository.GetAllPatients();

            foreach (var patient in patients)
            {
                var teeth = await _toothRepository.GetPatientTeeth(patient.Id);
                patient.Teeth = teeth.ToList();
            }
            return patients;
        }

        public async Task<Patient> GetPatientById(int id)
        {
            var patient = await _patientRepository.GetPatientById(id);
            var teeth = await _toothRepository.GetPatientTeeth(patient.Id);
            patient.Teeth = teeth.ToList();
            return patient;
        }

        public async Task<Patient> UpdatePatient(Patient patient)
        {
            return await _patientRepository.UpdatePatient(patient);
        }
    }
}
