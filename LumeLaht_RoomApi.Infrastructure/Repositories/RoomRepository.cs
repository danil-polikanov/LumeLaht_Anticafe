using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Entities.Filters;
using LumeLaht_RoomApi.Core_.Interfaces;
using LumeLaht_RoomApi.Infrastructure.Data;
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
                .AsQueryable();

            // Общий поиск
            if (!string.IsNullOrWhiteSpace(filterOptions.Search))
                query = query.Where(r => r.Name.Contains(filterOptions.Search) ||
                                       r.Description.Contains(filterOptions.Search));

            // Специфичные фильтры
            if (filterOptions.Filters.TryGetValue("Status", out var status) 
                && status is string statusStr
                && !string.IsNullOrWhiteSpace(statusStr))
                query = query.Where(r => r.Status == statusStr);
            if (filterOptions.Filters.TryGetValue("City", out var city) 
                && city is string citySrt
                && !string.IsNullOrWhiteSpace(citySrt))
                query = query.Where(r => r.Address.City == citySrt);
            if (filterOptions.Filters.TryGetValue("Region", out var region) 
                && region is string regionStr
                && !string.IsNullOrWhiteSpace(regionStr))
                query = query.Where(r => r.Address.Region == regionStr);
            if (filterOptions.Filters.TryGetValue("MinPrice", out var minPrice) && minPrice is double minPriceVal)
                query = query.Where(r => r.PricePerHour >= minPriceVal);
            if (filterOptions.Filters.TryGetValue("MaxPrice", out var maxPrice) && maxPrice is double maxPriceVal)
                query = query.Where(r => r.PricePerHour <= maxPriceVal);
            if (filterOptions.Filters.TryGetValue("ActivityIds", out var activityIds) &&
                activityIds is List<Guid> activityIdsList && activityIdsList.Any())
                query = query.Where(r => r.RoomActivity.Any(ra => activityIdsList.Contains(ra.ActivityId)));

            // Сортировка
            query = filterOptions.SortOptions.SortBy switch
            {
                "price" => filterOptions.SortOptions.SortOrder == "desc"
                    ? query.OrderByDescending(r => r.PricePerHour)
                    : query.OrderBy(r => r.PricePerHour),
                "name" => filterOptions.SortOptions.SortOrder == "desc"
                    ? query.OrderByDescending(r => r.Name)
                    : query.OrderBy(r => r.Name),
                _ => query.OrderBy(r => r.Name)
            };
            // Пагинация
            var totalItems = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((filterOptions.Page - 1) * filterOptions.PageSize)
                .Take(filterOptions.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Room>
            {
                Items = items,
                paggination = new PagginationOptions
                {
                    CurrentPage = filterOptions.Page,
                    PageSize = filterOptions.PageSize,
                    TotalItems = totalItems

                }
            };
        }
    }
}
