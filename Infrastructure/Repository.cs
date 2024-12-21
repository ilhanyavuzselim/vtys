using Infrastructure.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly RestorantDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(RestorantDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            // Her Include parametresini ekle
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByPredicate(Expression<Func<T, bool>>? predicate)
        {
            IQueryable<T> query = _dbSet;

            // Apply the predicate if it's provided
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync();
        }


        // GetByIdAsync ile ilişkili verileri yüklemek
        public async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            // Her Include parametresini ekle
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Attach(entity);
            }
            entry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }
        }

        public async Task ExecuteStoredProcedureAsync(string storedProcedureName, Dictionary<string, object> parameters)
        {
            var sqlCommand = GenerateSqlCommand(storedProcedureName, parameters);
            var sqlParameters = parameters.Select(p => new NpgsqlParameter($"@{p.Key}", p.Value ?? DBNull.Value)).ToArray();
            await _context.Database.ExecuteSqlRawAsync(sqlCommand, sqlParameters);
        }

        public async Task<List<T>> ExecuteStoredProcedureWithResultAsync(string storedProcedureName, Dictionary<string, object> parameters) 
        {
            var sqlCommand = GenerateSqlCommand(storedProcedureName, parameters);
            var sqlParameters = parameters.Select(p => new NpgsqlParameter($"@{p.Key}", p.Value ?? DBNull.Value)).ToArray();
            return await _context.Set<T>().FromSqlRaw(sqlCommand, sqlParameters).ToListAsync();
        }

        private string GenerateSqlCommand(string storedProcedureName, Dictionary<string, object> parameters)
        {
            var parameterList = string.Join(", ", parameters.Keys.Select(k => $"@{k}"));
            return $"CALL {storedProcedureName}({parameterList})";
        }

        public void CloseConnection()
        {
            if (_context.Database.GetDbConnection().State == System.Data.ConnectionState.Open)
            {
                _context.Database.GetDbConnection().Close();
            }
        }


    }
}
