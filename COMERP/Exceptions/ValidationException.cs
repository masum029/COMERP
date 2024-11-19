﻿using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;


namespace COMERP.Exceptions
{
    public class ValidatException : Exception
    {
        public ValidatException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidatException(string message)
            : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidatException(IEnumerable<ValidationFailure> failures)
            : base(CreateErrorMessage(failures))
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidatException(IEnumerable<IdentityError> errors)
            : base(CreateErrorMessage(errors))
        {
            Errors = errors
                .GroupBy(e => e.Code, e => e.Description)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidatException(string message, IEnumerable<ValidationFailure> failures)
            : base($"{message}: {CreateErrorMessage(failures)}")
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidatException(string message, IEnumerable<IdentityError> errors)
            : base($"{message}: {CreateErrorMessage(errors)}")
        {
            Errors = errors
                .GroupBy(e => e.Code, e => e.Description)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public IDictionary<string, string[]> Errors { get; }

        private static string CreateErrorMessage(IEnumerable<ValidationFailure> failures)
        {
            var errorMessages = failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}");
            return string.Join("; ", errorMessages);
        }

        private static string CreateErrorMessage(IEnumerable<IdentityError> errors)
        {
            var errorMessages = errors.Select(e => $"{e.Code}: {e.Description}");
            return string.Join("; ", errorMessages);
        }
    }
}
