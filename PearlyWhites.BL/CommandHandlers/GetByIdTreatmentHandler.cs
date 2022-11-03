using MediatR;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;

namespace PearlyWhites.BL.CommandHandlers
{
    public class GetByIdTreatmentHandler : IRequestHandler<GetByIdTreatmentCommand, Treatment>
    {
        private readonly ITreatmentsRepository _treatmentsRepository;

        public GetByIdTreatmentHandler(ITreatmentsRepository treatmentsRepository)
        {
            _treatmentsRepository = treatmentsRepository;
        }

        public Task<Treatment> Handle(GetByIdTreatmentCommand request, CancellationToken cancellationToken)
        {
            return _treatmentsRepository.GetTreatmentById(request.treatmentId);
        }
    }
}
