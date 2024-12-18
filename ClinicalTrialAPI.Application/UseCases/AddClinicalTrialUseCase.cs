using ClinicalTrialAPI.Domain.Repositories;

namespace ClinicalTrialAPI.Application.UseCases
{
    public class AddClinicalTrialUseCase
    {
        private readonly IClinicalTrialRepository _repository;

        public AddClinicalTrialUseCase(IClinicalTrialRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClinicalTrial> ExecuteAsync(ClinicalTrial clinicalTrial)
        {
            clinicalTrial.Duration = (clinicalTrial.EndDate - clinicalTrial.StartDate).Days;

            await _repository.AddAsync(clinicalTrial);
            return clinicalTrial;
        }
    }
}
