using ClinicalTrialAPI.Domain.Repositories;

namespace ClinicalTrialAPI.Application.UseCases
{
    public class GetAllClinicalTrialsByStatusUseCase
    {
        private readonly IClinicalTrialRepository _repository;

        public GetAllClinicalTrialsByStatusUseCase(IClinicalTrialRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ClinicalTrial>> ExecuteAsync(string? status)
        {
            if (string.IsNullOrEmpty(status))
                return await _repository.GetAllAsync();

            return await _repository.GetByStatusAsync(status);
        }
    }
}
