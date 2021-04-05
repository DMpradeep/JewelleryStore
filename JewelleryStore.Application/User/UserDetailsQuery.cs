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
        private readonly UserDetailsProvider _userDetailsProvider;

        public UserDetailsQueryHandler(UserDetailsProvider userDetailsProvider)
            => _userDetailsProvider = userDetailsProvider;

        public async Task<UserMessage> Handle(UserDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _userDetailsProvider.DetailsAsync(request.UserRno);

            if (result == null)
            {
                throw new NotFoundException(nameof(UserMessage), request.UserRno);
            }

            return result;
        }
    }

    public class UserDetailsProvider
    {
        private readonly IUserDataAccess _dataAccess;

        public UserDetailsProvider(IUserDataAccess dataAccess) => _dataAccess = dataAccess;

        public async Task<UserMessage> DetailsAsync(int userRno) => await _dataAccess.DetailsAsync(userRno);
    }
}
