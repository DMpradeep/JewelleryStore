using Dapper;
using JewelleryStore.Application.User;
using JewelleryStore.Model.Configuration;
using JewelleryStore.Model.User;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace JewelleryStore.DataAccess
{
    public class UserDataAccess : BaseDataAccess, IUserDataAccess
    {
        public UserDataAccess(IOptions<DbSetting> config) : base(config) { }

        public async Task<UserMessage> DetailsAsync(int rno)
        {
            var sql = $"Select * from users where rno = {DBVar(nameof(rno))}";

            using (var connection = GetDbConnection())
            {
                var result = await connection.QueryAsync<UserMessage>(sql, new { rno });
                return result.FirstOrDefault();
            }
        }

        public async Task<UserAuthenticationMessage> GetUserAuthenticationMessageAsync(string userId)
        {
            var sql = $"Select * from users where id = {DBVar(nameof(userId))}";

            using (var connection = GetDbConnection())
            {
                var result = await connection.QueryAsync<UserAuthenticationMessage>(sql, new { userId });
                return result.FirstOrDefault();
            }
        }
    }
}
