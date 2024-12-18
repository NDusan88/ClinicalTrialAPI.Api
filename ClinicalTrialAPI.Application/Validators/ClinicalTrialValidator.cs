using FluentValidation;

namespace ClinicalTrialAPI.Application.Validators
{
    public class ClinicalTrialValidator : AbstractValidator<ClinicalTrial>
    {
        public ClinicalTrialValidator()
        {
            RuleFor(x => x.TrialId.ToString())
                .NotEmpty().WithMessage("TrialId is required.")
                .Matches(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")
                .WithMessage("TrialId must be a valid GUID.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(3, 100).WithMessage("Title must be between 3 and 100 characters.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required.")
                .LessThan(x => x.EndDate).WithMessage("StartDate must be before EndDate.");

            RuleFor(x => x.EndDate)
                .Must((model, endDate) => endDate == null || endDate > model.StartDate)
                .WithMessage("EndDate must be after StartDate.");

            RuleFor(x => x.Participants)
                .NotEmpty().WithMessage("Participants is required.")
                .GreaterThanOrEqualTo(1).WithMessage("Participants must be greater than or equal to 1.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => new[] { "Not Started", "Ongoing", "Completed" }.Contains(status))
                .WithMessage("Status must be 'Not Started', 'Ongoing', or 'Completed'.");
        }
    }
}
