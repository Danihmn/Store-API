using FluentResults;

namespace Store.Domain.Validations;

public static class ValidationsExtension
{
    extension(List<IError> errors)
    {
        public void NotEmpty(string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value)) errors.Add(new Error(message));
        }

        public void NotEmptyIfProvided(string? value, string message)
        {
            if (value != null) errors.NotEmpty(value, message);
        }

        public void NotEmpty(Guid? value, string message)
        {
            if (value == Guid.Empty) errors.Add(new Error(message));
        }

        public void GreaterThanZero(decimal? value, string message)
        {
            if (value <= 0) errors.Add(new Error(message));
        }

        public void GreaterThanZero(int? value, string message)
        {
            if (value <= 0) errors.Add(new Error(message));
        }

        public void NotNegative(int? value, string message)
        {
            if (value < 0) errors.Add(new Error(message));
        }
    }

    public static void AddErrorsTo<T>(this Result<T> result, List<IError> errors)
    {
        if (result.IsFailed) errors.AddRange(result.Errors);
    }
}