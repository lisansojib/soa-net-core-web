using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    /// <summary>
    /// Use this repository to call sql query, stored procedures and functions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISqlQueryRepository<T> where T : class
    {
        int RunSqlCommand(string query, params object[] parameters);

        Task<int> RunSqlCommandAsync(string query, params object[] parameters);
    }
}
