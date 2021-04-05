using FluentValidation;
using JewelleryStore.Application.Exceptions;
using JewelleryStore.Model.Resources;
using JewelleryStore.Model.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JewelleryStore.Application.User
{
    public class ValidateUserQuery : UserAuthenticationMessage, IRequest<int> { }

    public class ValidateUserQueryHandler : IRequestHandler<ValidateUserQuery, int>
    {
        private readonly IUserDataAccess _dataAccess;
        private readonly AbstractValidator<UserAuthenticationMessage> _messageValidator;

        public ValidateUserQueryHandler(IUserDataAccess dataAccess, AbstractValidator<UserAuthenticationMessage> messageValidator)
        {
            _dataAccess = dataAccess;
            _messageValidator = messageValidator;
        }

        public async Task<int> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
        {
            await ValidateUserAuthenticationMessageRequest(request, cancellationToken);

            var message = await _dataAccess.GetUserAuthenticationMessageAsync(request.Id);
            if (message == null)
            {
                throw new NotFoundException(nameof(UserAuthenticationMessage), request.Id);
            }

            //compare hashes and move to helper class
            if (message.Password != request.Password)
            {
                throw new UserInputException("Invalid password");
            }

            return message.Rno;
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
