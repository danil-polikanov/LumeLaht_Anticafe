using LumeLaht_RoomApi.Core_.Interfaces;
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
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public Task<TEntity?> GetByIdAsync(int id) =>
            _dbSet.FindAsync(id).AsTask();

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate) =>
            _dbSet.AnyAsync(predicate);

    }
}
