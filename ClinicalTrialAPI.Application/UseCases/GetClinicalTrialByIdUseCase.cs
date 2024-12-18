using ClinicalTrialAPI.Domain.Repositories;

public class GetClinicalTrialByIdUseCase
{
    private readonly IClinicalTrialRepository _repository;

    public GetClinicalTrialByIdUseCase(IClinicalTrialRepository repository)
    {
        _repository = repository;
    }

    public async Task<ClinicalTrial> ExecuteAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
}
