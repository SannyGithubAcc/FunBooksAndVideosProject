namespace Application.Interfaces
{
    public interface IEntityService<T, TDto> where T : class where TDto : class
    {
        Task<TDto> GetByIdAsync(int id);
        Task<List<TDto>> GetAllAsync();
        Task<TDto> AddAsync(TDto dto);
        Task UpdateAsync(int id, TDto dto);
        Task DeleteAsync(int id);
    }
}
