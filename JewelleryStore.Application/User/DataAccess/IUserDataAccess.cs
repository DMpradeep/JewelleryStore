using JewelleryStore.Model.User;
using System.Threading.Tasks;

namespace JewelleryStore.Application.User
{
    public interface IUserDataAccess
    {
        Task<UserMessage> DetailsAsync(int rno);

        Task<UserAuthenticationMessage> GetUserAuthenticationMessageAsync(string userId);
    }
}
