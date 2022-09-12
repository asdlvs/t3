namespace Inmeta.Moving.Services
{
    public interface IService<T>
    {
        public Task<T> CreateAsync(T item);

        public Task<IEnumerable<T>> GetAsync();

        public Task<T?> GetByIdAsync(int id);

        public Task<T> UpdateAsync(int id, T item);

        public Task DeleteAsync(int id);
    }
}
