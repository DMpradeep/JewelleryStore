using FluentValidation;
using JewelleryStore.Model.Resources;
using JewelleryStore.Model.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JewelleryStore.Application.User
{
    public class ValidateUserQuery : UserAuthenticationMessage, IRequest<(bool isValidUser, int userRno)> { }

    public class ValidateUserQueryHandler : IRequestHandler<ValidateUserQuery, (bool isValidUser, int userRno)>
    {
        private readonly IUserDataAccess _dataAccess;
        private readonly AbstractValidator<UserAuthenticationMessage> _messageValidator;

        public ValidateUserQueryHandler(IUserDataAccess dataAccess, AbstractValidator<UserAuthenticationMessage> messageValidator)
        {
            _dataAccess = dataAccess;
            _messageValidator = messageValidator;
        }

        public async Task<(bool isValidUser, int userRno)> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
        {
            await ValidateUserAuthenticationMessageRequest(request, cancellationToken);

            var message = await _dataAccess.GetUserAuthenticationMessageAsync(request.Id);

            if (message != null)
            {
                return (message.Id == request.Id && message.Password == request.Password, message.Rno);
            }

            return (false, 0);
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
