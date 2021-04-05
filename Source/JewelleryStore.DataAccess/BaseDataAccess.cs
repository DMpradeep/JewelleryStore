using JewelleryStore.Model.Configuration;
using Microsoft.Extensions.Options;
using System.Data.Common;
using System.Data.SQLite;

namespace JewelleryStore.DataAccess
{
    public abstract class BaseDataAccess
    {
        private readonly IOptions<DbSetting> _config;

        public BaseDataAccess(IOptions<DbSetting> config) => _config = config;

        protected DbConnection GetDbConnection() => new SQLiteConnection(_config.Value.ConnectionString);

        protected static string DBVar(string variableName) => $"@{variableName}";
    }
}
