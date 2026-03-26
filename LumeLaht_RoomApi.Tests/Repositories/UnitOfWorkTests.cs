using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Infrastructure.Data;
using LumeLaht_RoomApi.Infrastructure.Repositories;
using LumeLaht_RoomApi.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LumeLaht_RoomApi.Tests.Repositories
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var roomRepository = new RoomRepository(_context);
            var addressRepository = new Repository<Address>(_context);
            var activityRepository = new Repository<Activity>(_context);
            var roomImageRepository = new Repository<RoomImage>(_context);

            _unitOfWork = new UnitOfWork(
                _context,
                roomRepository,
                addressRepository,
                activityRepository,
                roomImageRepository
            );
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void UnitOfWork_ShouldInitializeAllRepositories()
        {
            // Assert
            Assert.NotNull(_unitOfWork.Rooms);
            Assert.NotNull(_unitOfWork.Addresses);
            Assert.NotNull(_unitOfWork.Activities);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldPersistChanges()
        {
            // Arrange
            var address = TestDataFactory.CreateAddress();
            await _context.Address.AddAsync(address);

            // Act
            var result = await _unitOfWork.SaveChangesAsync();

            // Assert
            Assert.Equal(1, result);
            var savedAddress = await _context.Address.FindAsync(address.AddressId);
            Assert.NotNull(savedAddress);
        }

        [Fact]
        public async Task SaveChangesAsync_WithMultipleChanges_ShouldReturnCorrectCount()
        {
            // Arrange
            var address1 = TestDataFactory.CreateAddress("Город 1");
            var address2 = TestDataFactory.CreateAddress("Город 2");
            var activity = TestDataFactory.CreateActivity();

            await _context.Address.AddRangeAsync(address1, address2);
            await _context.Activities.AddAsync(activity);

            // Act
            var result = await _unitOfWork.SaveChangesAsync();

            // Assert
            Assert.Equal(3, result);
        }
    }
}
