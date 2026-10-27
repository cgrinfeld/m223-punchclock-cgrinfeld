using System.ComponentModel.DataAnnotations;

namespace M223PunchclockDotnet.DTO
{
    public class EntryDto : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Check-in is required.")]
        public DateTime? CheckIn { get; set; }

        [Required(ErrorMessage = "Check-out is required.")]
        public DateTime? CheckOut { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CheckOut <= CheckIn)
            {
                yield return new ValidationResult(
                    "Check-out must be after check-in.",
                    new[] { nameof(CheckOut) });
            }
        }
    }
}