namespace ClinicalTrialAPI.Domain.Repositories
{
    public interface IClinicalTrialRepository
    {
        Task AddAsync(ClinicalTrial clinicalTrial);
        Task<List<ClinicalTrial>> GetAllAsync();
        Task<ClinicalTrial> GetByIdAsync(Guid id);
        Task<List<ClinicalTrial>> GetByStatusAsync(string status);

    }
}
