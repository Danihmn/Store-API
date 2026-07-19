using FluentResults;

namespace Store.Domain.Validations;

public static class ValidationsExtension
{
    extension(List<IError> errors)
    {
        public void NotEmpty(string? value, string code, string message)
        {
            if (string.IsNullOrWhiteSpace(value)) errors.Add(new Abstractions.Error(code, message));
        }

        public void NotEmptyIfProvided(string? value, string code, string message)
        {
            if (value != null) errors.NotEmpty(value, code, message);
        }

        public void NotEmpty(Guid value, string code, string message)
        {
            if (value == Guid.Empty) errors.Add(new Abstractions.Error(code, message));
        }

        public void NotEmptyIfProvided(Guid? value, string code, string message)
        {
            if (value != null) errors.NotEmpty(value.Value, code, message);
        }

        public void GreaterThanZero(decimal value, string code, string message)
        {
            if (value <= 0) errors.Add(new Abstractions.Error(code, message));
        }

        public void GreaterThanZero(decimal? value, string code, string message)
        {
            if (value != null) errors.GreaterThanZero(value.Value, code, message);
        }

        public void GreaterThanZero(int value, string code, string message)
        {
            if (value <= 0) errors.Add(new Abstractions.Error(code, message));
        }

        public void NotNegative(int? value, string code, string message)
        {
            if (value < 0) errors.Add(new Abstractions.Error(code, message));
        }
    }

    public static void AddErrorsTo<T>(this Result<T> result, List<IError> errors)
    {
        if (result.IsFailed) errors.AddRange(result.Errors);
    }
}