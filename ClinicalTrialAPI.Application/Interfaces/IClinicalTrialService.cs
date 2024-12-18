namespace ClinicalTrialAPI.Application.Interfaces
{
    public interface IClinicalTrialService
    {
        Task<ClinicalTrial> AddClinicalTrialAsync(ClinicalTrial clinicalTrial);
        Task<ClinicalTrial> GetByIdAsync(Guid id);
        Task<List<ClinicalTrial>> GetAllByStatusAsync(string? status);

    }
}
