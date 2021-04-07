using JewelleryStore.Application.Exceptions;
using JewelleryStore.Model.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JewelleryStore.Application.User
{
    public class UserDetailsQuery : IRequest<UserMessage>
    {
        public int UserRno { get; set; }
    }

    public class UserDetailsQueryHandler : IRequestHandler<UserDetailsQuery, UserMessage>
    {
        private readonly IUserDataAccess _dataAccess;

        public UserDetailsQueryHandler(IUserDataAccess dataAccess)
            => _dataAccess = dataAccess;

        public async Task<UserMessage> Handle(UserDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _dataAccess.DetailsAsync(request.UserRno);

            if (result == null)
            {
                throw new NotFoundException(nameof(UserMessage), request.UserRno);
            }

            return result;
        }
    }
}
