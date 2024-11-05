using FluentValidation;
using MediatR;

namespace Filer.Storage.Shared.Behaviors;

public class ValidationBehavior<TRequest, IResult>
    (IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, IResult>
    where TRequest : IRequest<IResult>
{
    public async Task<IResult> Handle(TRequest request,
        RequestHandlerDelegate<IResult> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = validators.Select(v => v.Validate(context)).ToList();
        var groupedValidationFailures = validationResults.SelectMany(v => v.Errors)
            .GroupBy(e => e.PropertyName)
            .Select(g => new {
                PropertyName = g.Key,
                ValidationFailures = g.Select(v => new { v.ErrorMessage })
            }).ToList();

        if (groupedValidationFailures.Count != 0)
        {
            var validationProblemsDictionary = new Dictionary<string, string[]>();
            foreach (var group in groupedValidationFailures)
            {
                var errorMessages = group.ValidationFailures.Select(v => v.ErrorMessage);
                validationProblemsDictionary.Add(group.PropertyName, errorMessages.ToArray());
            }

            return (IResult)Results.ValidationProblem(validationProblemsDictionary);
        }

        return await next();
    }
}