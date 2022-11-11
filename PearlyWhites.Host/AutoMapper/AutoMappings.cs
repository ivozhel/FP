using AutoMapper;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Mongo;
using PearlyWhites.Models.Models.Requests.Patient;
using PearlyWhites.Models.Models.Requests.Tooth;
using PearlyWhites.Models.Models.Requests.Visit;

namespace PearlyWhites.Host.AutoMapper
{
    public class AutoMappings : Profile
    {
        public AutoMappings()
        {
            CreateMap<PatientRequest, Patient>();
            CreateMap<PatientUpdateRequest, Patient>();
            CreateMap<ToothRequest, Tooth>();
            CreateMap<ToothUpdateRequest, Tooth>();
            CreateMap<TreatmentRequest, Treatment>();
            CreateMap<VisitTreatmentRequest, VisitTreatment>();
            CreateMap<Visit, VisitRequest>();
            CreateMap<Visit, VisitUpdateRequest>();
        }
    }
}
