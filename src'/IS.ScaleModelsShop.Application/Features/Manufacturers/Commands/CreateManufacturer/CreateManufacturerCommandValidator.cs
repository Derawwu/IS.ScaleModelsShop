using FluentValidation;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Application.Resources;

namespace IS.ScaleModelsShop.Application.Features.Manufacturers.Commands.CreateManufacturer
{
    public class CreateManufacturerCommandValidator : AbstractValidator<CreateManufacturerCommand>
    {
        public CreateManufacturerCommandValidator(IManufacturerRepository manufacturerRepository)
        {
            _ = manufacturerRepository ?? throw new ArgumentNullException(nameof(manufacturerRepository));

            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(m => m.Name).NotEmpty()
            .NotNull()
            .Length(3, 50)
            .MustAsync(async (name, _) => !await manufacturerRepository.AnyAsync(c => c.Name == name))
            .WithMessage(_ => ValidationErrors.Manufacturer_Common_SameManufacturerAlreadyExist);
        }
    }
}