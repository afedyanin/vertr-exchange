namespace Vertr.OrderMatching.Domain.ValueObjects
{
    public class ValidationResult
    {
        public bool IsValid { get; }

        public string[] ValidationErrors { get; }
        public ValidationResult(bool isValid) : this(isValid, Array.Empty<string>())
        {
        }

        public ValidationResult(
            bool isValid,
            string[] validationErrors)
        {
            IsValid = isValid;
            ValidationErrors = validationErrors;
        }
    }
}
