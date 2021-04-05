using FluentValidation;
using JewelleryStore.Model.Resources;
using JewelleryStore.Model.User;

namespace JewelleryStore.Application.User.Validator
{
    public class UserAuthenticationMessageValidator : AbstractValidator<UserAuthenticationMessage>
    {
        public UserAuthenticationMessageValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(ErrorMessage.MissingInfo);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ErrorMessage.MissingInfo);
        }
    }
}
