using MediatR;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;

namespace PearlyWhites.BL.CommandHandlers
{
    public class UpdateTreatmentHandler : IRequestHandler<UpdateTreatmentCommand, Treatment>
    {
        private readonly ITreatmentsRepository _treatmentsRepository;

        public UpdateTreatmentHandler(ITreatmentsRepository treatmentsRepository)
        {
            _treatmentsRepository = treatmentsRepository;
        }

        public async Task<Treatment> Handle(UpdateTreatmentCommand request, CancellationToken cancellationToken)
        {
            return await _treatmentsRepository.UpdateTreatment(request.treatment);
        }
    }
}
