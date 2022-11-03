using AutoMapper;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests;

namespace PearlyWhites.Host.AutoMapper
{
    public class AutoMappings : Profile
    {
        public AutoMappings()
        {
            CreateMap<PatientRequest, Patient>();
            CreateMap<ToothRequest, Tooth>();
            CreateMap<TreatmentRequest, Treatment>();
        }
    }
}
