using AutoMapper;
using MediatR;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;

namespace PearlyWhites.BL.CommandHandlers
{
    public class AddTreatmentHandler : IRequestHandler<AddTreatmentCommand, Treatment>
    {
        private readonly IMapper _mapper;
        private readonly ITreatmentsRepository _treatmentsRepository;
        public AddTreatmentHandler(ITreatmentsRepository treatmentsRepository, IMapper mapper)
        {
            _treatmentsRepository = treatmentsRepository;
            _mapper = mapper;
        }
        public async Task<Treatment> Handle(AddTreatmentCommand request, CancellationToken cancellationToken)
        {            
            var treatmentToAdd = _mapper.Map<Treatment>(request.treatment);
            var treatment = await _treatmentsRepository.Create(treatmentToAdd);
            return treatment;
        }
    }
}
