using AutoMapper;
using Moq;
using PearlyWhites.BL.Services;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Host.AutoMapper;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests.Patient;

namespace PearlyWhites.Tests
{
    public class PatientServiceTests
    {
        private IList<Patient> _patients = new List<Patient>()
        {
            new Patient()
            {
                Id = 1,
                Name = "Dummy 1",
                Age = 30,
                Teeth = new List<Tooth>()
            },

            new Patient()
            {
                Id = 2,
                Name = "Dummy 2",
                Age = 30,
                Teeth = new List<Tooth>()
            },

            new Patient()
            {
                Id = 3,
                Name = "Dummy 3",
                Age = 30,
                Teeth = new List<Tooth>()
            }
        };

        private readonly IMapper _mapper;
        private readonly Mock<IToothRepository> _toothRepository;
        private readonly Mock<IPatientRepository> _patientRepository;
        private readonly Mock<ITeethAndTreatmentRepository> _teethAndTreatmentRepository;
        private readonly Mock<ITreatmentsRepository> _treatmentRepository;

        public PatientServiceTests()
        {
            var mockMapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMappings());
            });

            _mapper = mockMapConfig.CreateMapper();
            _toothRepository = new Mock<IToothRepository>();
            _patientRepository = new Mock<IPatientRepository>();
            _teethAndTreatmentRepository = new Mock<ITeethAndTreatmentRepository>();
            _treatmentRepository = new Mock<ITreatmentsRepository>();
        }

        [Fact]
        public async Task Patients_GetAll_CountCheck()
        {
            //setup
            var expectedCount = _patients.Count;
            _patientRepository.Setup(x => x.GetAllPatients()).ReturnsAsync(_patients);

            //inject 
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var result = await service.GetAllPatients();

            //assert 
            var response = result.Respone;
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(expectedCount, response.Count());
            Assert.Equal(200,(int)result.StatusCode);

        }
        [Fact]
        public async Task Patients_GetAll_Error()
        {
            //setup
            _patientRepository.Setup(x => x.GetAllPatients()).ReturnsAsync((IEnumerable<Patient>)null);

            //inject 
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var result = await service.GetAllPatients();

            //assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);

        }
        [Fact]
        public async Task Patient_GetById()
        {
            //setup
            var patientId = _patients.First().Id;            
            var patient = _patients.FirstOrDefault(x => x.Id == patientId);
            _patientRepository.Setup(x => x.GetPatientById(patientId)).ReturnsAsync(patient);

            //inject
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var result = await service.GetPatientById(patientId);

            //assert
            Assert.NotNull(result);
            Assert.Equal(patient.Id, result.Respone.Id);
            Assert.Equal(patient.Name, result.Respone.Name);
            Assert.Equal(patient.Age, result.Respone.Age);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public async Task Patient_GetById_NotFund()
        {
            //setup
            var patientId = _patients.Last().Id + 1;
            _patientRepository.Setup(x => x.GetPatientById(patientId)).ReturnsAsync((Patient)null);

            //inject
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var result = await service.GetPatientById(patientId);

            //assert
            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public async Task Patient_GetById_BadRequest()
        {
            //setup

            //inject
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var result = await service.GetPatientById(0);

            //assert
            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public async Task Patient_Create()
        {
            //setup
            var patientId = _patients.Last().Id + 1;
            var patient = new Patient()
            {
                Id = patientId,
                Name = "Test",
                Age = 31
            };

            _patientRepository.Setup(x => x.Create(It.IsAny<Patient>())).ReturnsAsync(patient);

            //inject
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var patientReq = new PatientRequest()
            {
                Name = patient.Name,
                Age = patient.Age ?? 0
            };

            var result = await service.Create(patientReq);

            //assert
            Assert.NotNull(result);
            Assert.Equal(patient.Name, result.Respone.Name);
            Assert.Equal(patient.Age, result.Respone.Age);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);

        }
        [Fact]
        public async Task Patient_Create_Error()
        {
            //setup
            _patientRepository.Setup(x => x.Create(It.IsAny<Patient>())).ReturnsAsync((Patient) null);

            //inject
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var patientReq = new PatientRequest();

            var result = await service.Create(patientReq);

            //assert
            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
        }
        [Fact]
        public async Task Patient_Delete()
        {
            //setup
            var deleteId = _patients.First().Id;
            _patientRepository.Setup(x=>x.DeletePatientById(It.IsAny<int>())).ReturnsAsync(true);
            _patientRepository.Setup(x => x.GetPatientById(It.IsAny<int>())).ReturnsAsync(new Patient());
            _toothRepository.Setup(x => x.DeletePatientTeeth(It.IsAny<int>())).ReturnsAsync(true);

            //inject
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var result = await service.DeletePatientById(deleteId);


            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.True(result.Respone);

        }
        [Fact]
        public async Task Patient_Delete_NotFound()
        {
            //setup
            var deleteId = _patients.First().Id;
            _patientRepository.Setup(x => x.DeletePatientById(It.IsAny<int>())).ReturnsAsync(true);
            _patientRepository.Setup(x => x.GetPatientById(It.IsAny<int>())).ReturnsAsync((Patient)null);

            //inject
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var result = await service.DeletePatientById(deleteId);


            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, result.StatusCode);
            Assert.False(result.Respone);

        }
        [Fact]
        public async Task Patient_Delete_BadRequest()
        {
            //setup
            var deleteId = 0;

            //inject
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var result = await service.DeletePatientById(deleteId);


            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
            Assert.False(result.Respone);

        }
        [Fact]
        public async Task Patient_Delete_Error()
        {
            //setup
            var deleteId = _patients.First().Id;
            _patientRepository.Setup(x => x.DeletePatientById(It.IsAny<int>())).ReturnsAsync(true);
            _patientRepository.Setup(x => x.GetPatientById(It.IsAny<int>())).ReturnsAsync(new Patient());
            _toothRepository.Setup(x => x.DeletePatientTeeth(It.IsAny<int>())).ReturnsAsync(false);

            //inject
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var result = await service.DeletePatientById(deleteId);
  
            //assert
            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.False(result.Respone);
        }
        [Fact]
        public async Task Patient_Update()
        {
            //setup
            var expectedID = 3;
            var patientRequest = new PatientUpdateRequest()
            {
                Name = "Test",
                Age = 20
            };
            
            _patientRepository.Setup(x => x.GetPatientById(It.IsAny<int>())).ReturnsAsync(() => _patients.FirstOrDefault(x => x.Id == expectedID));

            _patientRepository.Setup(x => x.UpdatePatient(It.IsAny<Patient>())).ReturnsAsync(new Patient() { Name = patientRequest.Name, Age = patientRequest.Age});

            //infect 
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var result = await service.UpdatePatient(patientRequest);
            
            //assert
            Assert.NotNull(result);
            Assert.Equal(patientRequest.Name, result.Respone.Name);
            Assert.Equal(patientRequest.Age, result.Respone.Age);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public async Task Patient_Update_NotFound()
        {
            //setup

            _patientRepository.Setup(x => x.GetPatientById(It.IsAny<int>())).ReturnsAsync((Patient)null);

            //infect 
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var result = await service.UpdatePatient(new PatientUpdateRequest() { Id = It.IsAny<int>() });

            //assert
            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public async Task Patient_Update_Error()
        {
            //setup
            _patientRepository.Setup(x => x.GetPatientById(It.IsAny<int>())).ReturnsAsync(new Patient());
            _patientRepository.Setup(x => x.UpdatePatient(It.IsAny<Patient>())).ReturnsAsync((Patient)null);

            //infect 
            var service = new PatientService(_patientRepository.Object, _mapper, _toothRepository.Object, _teethAndTreatmentRepository.Object, _treatmentRepository.Object);

            //act
            var result = await service.UpdatePatient(new PatientUpdateRequest() { Id = It.IsAny<int>() });

            //assert
            Assert.NotNull(result);
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);
        }
    }
}