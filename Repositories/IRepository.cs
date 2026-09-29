using MessageHub.Models;

namespace MessageHub.Repositories;

public interface IRepository<T> where T : class, IFirestoreEntity
{
    Task<T?> GetByIdAsync(string Id);
    Task<IReadOnlyList<T>> GetAllAsync(int limit = 100);
    Task<T> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(string Id);

}