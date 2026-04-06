using RoomService.Core.Entities;
using RoomService.Core.Entities.Filters;
using RoomService.Core.Interfaces;
using RoomService.Infrastructure.Data;
using RoomService.Infrastructure.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace RoomService.Infrastructure.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(RoomDbContext context) : base(context) { }

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
