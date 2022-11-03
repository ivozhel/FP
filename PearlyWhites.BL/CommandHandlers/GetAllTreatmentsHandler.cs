using MediatR;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;

namespace PearlyWhites.BL.CommandHandlers
{
    public class GetAllTreatmentsHandler : IRequestHandler<GetAllTretmentsCommand, IEnumerable<Treatment>>
    {
        private readonly ITreatmentsRepository _tretmentsRepository;

        public GetAllTreatmentsHandler(ITreatmentsRepository tretmentsRepository)
        {
            _tretmentsRepository = tretmentsRepository;
        }

        public async Task<IEnumerable<Treatment>> Handle(GetAllTretmentsCommand request, CancellationToken cancellationToken)
        {
            var result = await _tretmentsRepository.GetAllTreatments();
            return result;
        }
    }
}
