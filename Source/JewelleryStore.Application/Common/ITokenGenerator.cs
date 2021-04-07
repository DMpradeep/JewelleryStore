using JewelleryStore.Model.Common;

namespace JewelleryStore.Application.Common
{
    public interface ITokenGenerator
    {
        public TokenMessage GenerateToken(int userRno, string username);
    }
}
