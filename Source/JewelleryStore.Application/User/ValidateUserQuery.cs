using FluentValidation;
using JewelleryStore.Application.Common;
using JewelleryStore.Application.Exceptions;
using JewelleryStore.Model.Common;
using JewelleryStore.Model.Resources;
using JewelleryStore.Model.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JewelleryStore.Application.User
{
    public class ValidateUserQuery : UserAuthenticationMessage, IRequest<TokenMessage> { }

    public class ValidateUserQueryHandler : IRequestHandler<ValidateUserQuery, TokenMessage>
    {
        private readonly IUserDataAccess _dataAccess;
        private readonly AbstractValidator<UserAuthenticationMessage> _messageValidator;
        private readonly ITokenGenerator _tokenGenerator;

        public ValidateUserQueryHandler(AbstractValidator<UserAuthenticationMessage> messageValidator, IUserDataAccess dataAccess, ITokenGenerator tokenGenerator)
        {
            _dataAccess = dataAccess;
            _messageValidator = messageValidator;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<TokenMessage> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
        {
            await ValidateUserAuthenticationMessageRequest(request, cancellationToken);

            var message = await _dataAccess.GetUserAuthenticationMessageAsync(request.Id);
            var isValidUser = message != null &&
                (message.Id == request.Id && message.Password == request.Password);

            if (isValidUser)
            {
                return _tokenGenerator.GenerateToken(message.Rno, message.Id);
            }

            throw new UserInputException(ErrorMessage.InvalidUserIdPassword);
        }

        private async Task ValidateUserAuthenticationMessageRequest(ValidateUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _messageValidator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                throw new ValidationException(string.Format(ErrorMessage.ValidationError, CommonMessage.User), result.Errors);
            }
        }
    }
}
