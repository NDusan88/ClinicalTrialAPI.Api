using ClinicalTrialAPI.Domain.Repositories;
using ClinicalTrialAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ClinicalTrialRepository : IClinicalTrialRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ClinicalTrialRepository> _logger;

    public ClinicalTrialRepository(AppDbContext context, ILogger<ClinicalTrialRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ClinicalTrial>> GetAllAsync()
    {
        try
        {
            return await _context.ClinicalTrials.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all clinical trials.");
            throw new ApplicationException("Unable to retrieve clinical trials. Please try again later.", ex);
        }
    }

    public async Task AddAsync(ClinicalTrial clinicalTrial)
    {
        try
        {
            await _context.ClinicalTrials.AddAsync(clinicalTrial);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "A database update error occurred while adding a clinical trial.");
            throw new ApplicationException("An error occurred while adding the clinical trial. Please try again later.", dbEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while adding a clinical trial.");
            throw new ApplicationException("An unexpected error occurred while adding the clinical trial. Please try again later.", ex);
        }
    }

    public async Task<ClinicalTrial> GetByIdAsync(Guid id)
    {
        try
        {
            var trial = await _context.ClinicalTrials.FirstOrDefaultAsync(trial => trial.TrialId == id);
            if (trial == null)
            {
                _logger.LogWarning("Clinical trial with ID {TrialId} was not found.", id);
                throw new KeyNotFoundException("Clinical trial not found.");
            }
            return trial;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the clinical trial with ID {TrialId}.", id);
            throw new ApplicationException("Unable to retrieve the clinical trial. Please try again later.", ex);
        }
    }

    public async Task<List<ClinicalTrial>> GetByStatusAsync(string status)
    {
        try
        {
            return await _context.ClinicalTrials
                .Where(trial => trial.Status == status)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving clinical trials with status {Status}.", status);
            throw new ApplicationException("Unable to retrieve clinical trials by status. Please try again later.", ex);
        }
    }
}
