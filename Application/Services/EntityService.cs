using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EntityService<T, TDto> : IEntityService<T, TDto> where T : class where TDto : class
    {
        private readonly IRepository<T> repository;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public EntityService(IRepository<T> repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<TDto> GetByIdAsync(int id)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException("The entity was not found.");
            }
            return mapper.Map<TDto>(entity);
        }

        public async Task<List<TDto>> GetAllAsync()
        {
            var entities = await repository.GetAllAsync();
            return mapper.Map<List<TDto>>(entities);
        }

        public async Task<TDto> AddAsync(TDto dto)
        {
            var entity = mapper.Map<T>(dto);
            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<TDto>(entity);
        }
        public async Task UpdateAsync(int id, TDto dto)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException("The entity was not found.");
            }
            mapper.Map(dto, entity);
            repository.Update(entity);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException("The entity was not found.");
            }
            repository.Delete(entity);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
    
