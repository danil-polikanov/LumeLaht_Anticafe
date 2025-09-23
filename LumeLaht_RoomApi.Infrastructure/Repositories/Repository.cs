using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using LumeLaht_RoomApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();      
        }
        public IQueryable<TEntity> GetQueryable() => _dbSet.AsQueryable();
        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken) =>
            await _dbSet.ToListAsync(cancellationToken);
        public async Task<TEntity?> GetByIdAsync(Guid id,CancellationToken cancellationToken) =>
            await _dbSet.FindAsync(id,cancellationToken).AsTask();

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate,CancellationToken cancellationToken) =>
           await _dbSet.AnyAsync(predicate, cancellationToken);
    }
}
