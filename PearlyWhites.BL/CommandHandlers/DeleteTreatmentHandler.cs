using MediatR;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;

namespace PearlyWhites.BL.CommandHandlers
{
    public class DeleteTreatmentHandler : IRequestHandler<DeleteTreatmentCommand, bool>
    {
        private readonly ITreatmentsRepository _treatmentsRepository;

        public DeleteTreatmentHandler(ITreatmentsRepository treatmentsRepository)
        {
            _treatmentsRepository = treatmentsRepository;
        }

        public async Task<bool> Handle(DeleteTreatmentCommand request, CancellationToken cancellationToken)
        {
            return await _treatmentsRepository.DeleteTreatmentById(request.treatmentId);
        }
    }
}
