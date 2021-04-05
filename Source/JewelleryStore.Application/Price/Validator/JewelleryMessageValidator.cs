using FluentValidation;
using JewelleryStore.Model.Jewellery;
using JewelleryStore.Model.Resources;

namespace JewelleryStore.Application.Price.Validator
{
    public class JewelleryMessageValidator : AbstractValidator<JewelleryMessage>
    {
        public JewelleryMessageValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ErrorMessage.InvalidInput);

            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ErrorMessage.InvalidInput);

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage(ErrorMessage.InvalidInput);
        }
    }
}
