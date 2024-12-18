using ClinicalTrialAPI.Application.Interfaces;
using ClinicalTrialAPI.Application.UseCases;

namespace ClinicalTrialAPI.Application.Services
{
    public class ClinicalTrialService : IClinicalTrialService
    {
        private readonly AddClinicalTrialUseCase _addUseCase;
        private readonly GetClinicalTrialByIdUseCase _getByIdUseCase;
        private readonly GetAllClinicalTrialsByStatusUseCase _getAllByStatusUseCase;

        public ClinicalTrialService(
            AddClinicalTrialUseCase addUseCase,
            GetClinicalTrialByIdUseCase getByIdUseCase,
            GetAllClinicalTrialsByStatusUseCase getAllByStatusUseCase)
        {
            _addUseCase = addUseCase;
            _getByIdUseCase = getByIdUseCase;
            _getAllByStatusUseCase = getAllByStatusUseCase;
        }

        public async Task<ClinicalTrial> AddClinicalTrialAsync(ClinicalTrial clinicalTrial)
        {
            return await _addUseCase.ExecuteAsync(clinicalTrial);
        }

        public async Task<ClinicalTrial> GetByIdAsync(Guid id)
        {
            return await _getByIdUseCase.ExecuteAsync(id);
        }

        public async Task<List<ClinicalTrial>> GetAllByStatusAsync(string? status)
        {
            return await _getAllByStatusUseCase.ExecuteAsync(status);
        }
    }
}
