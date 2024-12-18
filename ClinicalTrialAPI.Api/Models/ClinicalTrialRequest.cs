using System.ComponentModel.DataAnnotations;

namespace ClinicalTrialAPI.Api.Models
{
    public class ClinicalTrialRequest
    {
        [Required(ErrorMessage = "The trial field is required.")]
        public ClinicalTrial Trial { get; set; }
    }

}
