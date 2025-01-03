﻿using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAllByPredicate(Expression<Func<T, bool>>? predicate = null);
        Task<IEnumerable<T>> GetAllByPredicatesAndIncludes(Expression<Func<T, bool>>[] predicates = null, Expression<Func<T, object>>[] includes = null);
        Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task ExecuteStoredProcedureAsync(string storedProcedureName, Dictionary<string, object> parameters);
        Task<List<T>> ExecuteStoredProcedureWithResultAsync(string storedProcedureName, Dictionary<string, object> parameters);
        public void CloseConnection();
    }
}
