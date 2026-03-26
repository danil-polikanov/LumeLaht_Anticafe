using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Entities.Filters;
using LumeLaht_RoomApi.Core_.Interfaces;
using LumeLaht_RoomApi.Infrastructure.Data;
using LumeLaht_RoomApi.Infrastructure.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Infrastructure.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(AppDbContext context) : base(context) { }

        public override async Task<List<Room>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(r => r.Address)
                .Include(r => r.Images)
                .Include(r => r.RoomActivity).ThenInclude(ra => ra.Activity)
                .ToListAsync(cancellationToken);
        }
        public override async Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Include(r => r.Address)
                .Include(r => r.Images)
                .Include(r => r.RoomActivity).ThenInclude(ra => ra.Activity)
                .FirstOrDefaultAsync(r => r.RoomId == id, cancellationToken);
        }

        public async Task<PagedResult<Room>> GetFilteredRoomsAsync(
            FilterOptions filterOptions,
            CancellationToken cancellationToken)
        {
            var query = _dbSet
                .Include(r => r.Address)
                .Include(r => r.Images)
                .Include(r => r.RoomActivity).ThenInclude(ra => ra.Activity)
                .AsQueryable()
                .ApplySearch(filterOptions.Search)
                .ApplyStatusFilter(filterOptions)
                .ApplyAddressFilter(filterOptions)
                .ApplyPriceFilter(filterOptions)
                .ApplyCapacityFilter(filterOptions)
                .ApplyActivityFilter(filterOptions)
                .ApplySorting(filterOptions.SortOptions);

            // Пагинация
            var totalItems = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((filterOptions.Page - 1) * filterOptions.PageSize)
                .Take(filterOptions.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Room>
            {
                Items = items,
                pagination = new PaginationOptions
                {
                    CurrentPage = filterOptions.Page,
                    PageSize = filterOptions.PageSize,
                    TotalItems = totalItems
                }
            };
        }
    }
}
