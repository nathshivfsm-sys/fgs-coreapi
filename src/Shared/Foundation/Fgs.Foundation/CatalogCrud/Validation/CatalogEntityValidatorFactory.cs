using System.Linq.Expressions;
using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;

namespace Fgs.Foundation.CatalogCrud.Validation;

public static class CatalogEntityValidatorFactory
{
    public static void ApplyCreateRules<TCommand, TPayload>(
        AbstractValidator<TCommand> validator,
        CatalogEntityDescriptor descriptor,
        IEntityReadRepository readRepository,
        Expression<Func<TCommand, TPayload>> payloadSelector)
        where TPayload : class
    {
        foreach (var column in descriptor.WritableColumns.Where(c => c.IsRequired))
        {
            validator.RuleFor(payloadSelector)
                .Must(payload => HasRequiredValue(payload, column.PropertyName))
                .WithMessage($"{column.PropertyName} is required.");
        }

        ApplyLengthRules(validator, descriptor, payloadSelector);
        ApplyUniqueRules(validator, descriptor, readRepository, payloadSelector, excludeIdSelector: null);
    }

    public static void ApplyUpdateRules<TCommand, TPayload>(
        AbstractValidator<TCommand> validator,
        CatalogEntityDescriptor descriptor,
        IEntityReadRepository readRepository,
        Expression<Func<TCommand, TPayload>> payloadSelector,
        Expression<Func<TCommand, string>> idSelector)
        where TPayload : class
    {
        ApplyLengthRules(validator, descriptor, payloadSelector);
        ApplyUniqueRules(validator, descriptor, readRepository, payloadSelector, idSelector);
    }

    public static void ApplyPatchRules<TCommand, TPayload>(
        AbstractValidator<TCommand> validator,
        CatalogEntityDescriptor descriptor,
        Expression<Func<TCommand, TPayload>> payloadSelector)
        where TPayload : class =>
        ApplyLengthRules(validator, descriptor, payloadSelector);

    private static void ApplyUniqueRules<TCommand, TPayload>(
        AbstractValidator<TCommand> validator,
        CatalogEntityDescriptor descriptor,
        IEntityReadRepository readRepository,
        Expression<Func<TCommand, TPayload>> payloadSelector,
        Expression<Func<TCommand, string>>? excludeIdSelector)
        where TPayload : class
    {
        foreach (var uniqueKey in descriptor.UniqueKeys)
        {
            validator.RuleFor(command => command)
                .MustAsync(async (command, cancellationToken) =>
                {
                    var payload = payloadSelector.Compile()(command);
                    if (payload is null)
                    {
                        return true;
                    }

                    var values = uniqueKey.PropertyNames.ToDictionary(
                        propertyName => propertyName,
                        propertyName => GetPropertyValue(payload, propertyName),
                        StringComparer.Ordinal);

                    if (values.Values.Any(value => value is null))
                    {
                        return true;
                    }

                    var excludeId = excludeIdSelector?.Compile()(command);
                    return !await readRepository.ExistsAsync(descriptor, values, excludeId, cancellationToken);
                })
                .WithMessage($"A {descriptor.EntityName} with the same {string.Join(", ", uniqueKey.PropertyNames)} already exists.");
        }
    }

    private static void ApplyLengthRules<TCommand, TPayload>(
        AbstractValidator<TCommand> validator,
        CatalogEntityDescriptor descriptor,
        Expression<Func<TCommand, TPayload>> payloadSelector)
        where TPayload : class
    {
        foreach (var column in descriptor.WritableColumns.Where(c => c.MaxLength.HasValue))
        {
            validator.RuleFor(payloadSelector)
                .Must(payload => IsWithinMaxLength(payload, column.PropertyName, column.MaxLength!.Value))
                .WithMessage($"{column.PropertyName} must be {column.MaxLength} characters or fewer.");
        }
    }

    private static bool HasRequiredValue(object? payload, string propertyName)
    {
        var value = GetPropertyValue(payload, propertyName);
        return value switch
        {
            null => false,
            string text => !string.IsNullOrWhiteSpace(text),
            _ => true
        };
    }

    private static bool IsWithinMaxLength(object? payload, string propertyName, int maxLength)
    {
        var value = GetPropertyValue(payload, propertyName)?.ToString();
        return value is null || value.Length <= maxLength;
    }

    private static object? GetPropertyValue(object? payload, string propertyName) =>
        payload?.GetType().GetProperty(propertyName)?.GetValue(payload);
}
