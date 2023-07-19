using FluentValidation;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Application.Resources;
using System.Text.RegularExpressions;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer;

/// <summary>
/// Validator for the Create Manufacturer command.
/// </summary>
public class CreateManufacturerCommandValidator : AbstractValidator<CreateManufacturerCommand>
{
    /// <summary>
    /// Initializes a new instance of <see cref="CreateManufacturerCommandValidator"/>
    /// </summary>
    /// <param name="manufacturerRepository">Instance for working with manufacturers.</param>
    public CreateManufacturerCommandValidator(IManufacturerRepository manufacturerRepository)
    {
        _ = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(m => m.Name).NotEmpty()
            .NotNull()
            .Length(3, 50)
            .MustAsync(async (name, _) => !await manufacturerRepository.AnyAsync(c => c.Name == name))
            .WithMessage(_ => ValidationErrors.Manufacturer_Common_SameManufacturerAlreadyExist);

        When(
            m => !string.IsNullOrEmpty(m.Website),
            () =>
            {
                RuleFor(m => m.Website)
                    .MaximumLength(40)
                    .Matches(new Regex(@"^(https?://)?(www\.)?[a-z0-9]+\.[a-z]{2,6}(/[^\s]*)?$",
                        RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace))
                    .WithMessage(_ => ValidationErrors.Manufacturer_Common_ProvidedStringIsNotValidUrl)
                    .Matches(new Regex(@"^(?!.*\.(ru|su))",
                        RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace))
                    .WithMessage(_ => ValidationErrors.Manufacturer_Common_ProvidedUrlHasUnappropriateDomainName);
            });
    }
}