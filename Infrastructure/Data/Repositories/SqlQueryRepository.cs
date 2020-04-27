using ApplicationCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class SqlQueryRepository<T> : ISqlQueryRepository<T> where T: class
    {
        protected readonly AppDbContext _dbContext;

        public SqlQueryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int RunSqlCommand(string query, params object[] parameters)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = _dbContext.Database.ExecuteSqlRaw(query, parameters);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    return result;
                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<int> RunSqlCommandAsync(string query, params object[] parameters)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = await _dbContext.Database.ExecuteSqlRawAsync(query, parameters);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    return result;
                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
