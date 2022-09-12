namespace Inmeta.Moving.Services
{
    public interface ISearchingService<T>
    {
        public Task<IEnumerable<T>> FindByText(string text);
    }
}
