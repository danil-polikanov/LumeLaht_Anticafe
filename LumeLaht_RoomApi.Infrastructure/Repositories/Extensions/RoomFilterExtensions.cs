using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Entities.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Infrastructure.Repositories.Extensions
{
    public static class RoomFilterExtensions
    {
        public static IQueryable<Room> ApplySearch(this IQueryable<Room> query, string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return query;

            return query.Where(r =>
                r.Name.Contains(search) ||
                r.Description.Contains(search));
        }

        public static IQueryable<Room> ApplyStatusFilter(this IQueryable<Room> query, FilterOptions options)
        {
            if (options.Filters.TryGetValue("Status", out var status)
                && status is string statusStr
                && !string.IsNullOrWhiteSpace(statusStr))
            {
                query = query.Where(r => r.Status == statusStr);
            }
            return query;
        }

        public static IQueryable<Room> ApplyAddressFilter(this IQueryable<Room> query, FilterOptions options)
        {
            if (options.Filters.TryGetValue("City", out var city)
                && city is string cityStr
                && !string.IsNullOrWhiteSpace(cityStr))
            {
                query = query.Where(r => r.Address.City.Contains(cityStr));
            }

            if (options.Filters.TryGetValue("Region", out var region)
                && region is string regionStr
                && !string.IsNullOrWhiteSpace(regionStr))
            {
                query = query.Where(r => r.Address.Region.Contains(regionStr));
            }

            return query;
        }

        public static IQueryable<Room> ApplyPriceFilter(this IQueryable<Room> query, FilterOptions options)
        {
            if (options.Filters.TryGetValue("MinPrice", out var minPrice)
                && minPrice is double minPriceVal)
            {
                query = query.Where(r => r.PricePerHour >= minPriceVal);
            }

            if (options.Filters.TryGetValue("MaxPrice", out var maxPrice)
                && maxPrice is double maxPriceVal)
            {
                query = query.Where(r => r.PricePerHour <= maxPriceVal);
            }

            return query;
        }

        public static IQueryable<Room> ApplyActivityFilter(this IQueryable<Room> query, FilterOptions options)
        {
            if (options.Filters.TryGetValue("ActivitiesIds", out var activityIds)
                && activityIds is List<Guid> ids && ids.Any())
            {
                query = query.Where(r =>
                    ids.All(id => r.RoomActivity.Any(ra => ra.ActivityId == id)));    
            }

            return query;
        }

        public static IQueryable<Room> ApplySorting(this IQueryable<Room> query, SortOptions sortOptions)
        {
            return sortOptions.Field switch
            {
                "pricePerHour" => sortOptions.Direction == "desc"
                    ? query.OrderByDescending(r => r.PricePerHour)
                    : query.OrderBy(r => r.PricePerHour),
                "name" => sortOptions.Direction == "desc"
                    ? query.OrderByDescending(r => r.Name)
                    : query.OrderBy(r => r.Name),
                "city" => sortOptions.Direction == "desc"
            ? query.OrderByDescending(r => EF.Functions.Collate(r.Address.City, "Estonian_CI_AS"))
            : query.OrderBy(r => EF.Functions.Collate(r.Address.City, "Estonian_CI_AS")),
                _ => query.OrderBy(r => r.Name)
            };
        }
    }
}
