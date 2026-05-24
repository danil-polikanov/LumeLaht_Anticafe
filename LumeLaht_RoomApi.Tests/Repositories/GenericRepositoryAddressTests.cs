using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Infrastructure.Data;
using LumeLaht_RoomApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LumeLaht_RoomApi.Tests.Helpers.TestDataFactory;
using Xunit;

namespace LumeLaht_RoomApi.Tests.Repositories
{
    public class GenericRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly Repository<Address> _repository;

        public GenericRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new Repository<Address>(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region GetAllAsync Tests

        [Fact]
        public async Task GetAllAsync_WhenEmpty_ReturnsEmptyList()
        {
            // Act
            var result = await _repository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_WithMultipleEntities_ReturnsAllEntities()
        {
            // Arrange
            var addresses = new List<Address>
            {
                CreateAddress("Narva"),
                CreateAddress("Tallinn"),
                CreateAddress("Tartu")
            };
            await _context.Address.AddRangeAsync(addresses);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, a => a.City == "Narva");
            Assert.Contains(result, a => a.City == "Tallinn");
            Assert.Contains(result, a => a.City == "Tartu");
        }

        [Fact]
        public async Task GetAllAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await _repository.GetAllAsync(cts.Token)
            );
        }

        #endregion

        #region GetByIdAsync Tests

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ReturnsNull()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdAsync(nonExistentId, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingId_ReturnsEntity()
        {
            // Arrange
            var address = CreateAddress("Narva");
            await _context.Address.AddAsync(address);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(address.AddressId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(address.AddressId, result.AddressId);
            Assert.Equal(address.City, result.City);
            Assert.Equal(address.AddressName, result.AddressName);
        }

        [Fact]
        public async Task GetByIdAsync_WithEmptyGuid_ReturnsNull()
        {
            // Arrange
            var emptyGuid = Guid.Empty;

            // Act
            var result = await _repository.GetByIdAsync(emptyGuid, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region AddAsync Tests

        [Fact]
        public async Task AddAsync_WithValidEntity_AddsAndReturnsEntity()
        {
            // Arrange
            var address = CreateAddress("Tallinn");

            // Act
            var result = await _repository.AddAsync(address, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(address.AddressId, result.AddressId);

            var savedAddress = await _context.Address.FindAsync(address.AddressId);
            Assert.NotNull(savedAddress);
            Assert.Equal(address.City, savedAddress.City);
        }

        [Fact]
        public async Task AddAsync_AutomaticallyCallsSaveChanges()
        {
            // Arrange
            var address = CreateAddress("Tallinn");

            // Act
            await _repository.AddAsync(address, CancellationToken.None);

            // Assert
            var count = await _context.Address.CountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task AddAsync_WithNullEntity_ThrowsException()
        {
            // Arrange
            Address nullAddress = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _repository.AddAsync(nullAddress, CancellationToken.None)
            );
        }

        [Fact]
        public async Task AddAsync_WithMultipleEntities_AddsAll()
        {
            // Arrange
            var address1 = CreateAddress("Город 1");
            var address2 = CreateAddress("Город 2");

            // Act
            await _repository.AddAsync(address1, CancellationToken.None);
            await _repository.AddAsync(address2, CancellationToken.None);

            // Assert
            var allAddresses = await _context.Address.ToListAsync();
            Assert.Equal(2, allAddresses.Count);
        }

        #endregion

        #region UpdateAsync Tests

        [Fact]
        public async Task UpdateAsync_WithExistingEntity_UpdatesEntity()
        {
            // Arrange
            var address = CreateAddress("Старый город");
            await _context.Address.AddAsync(address);
            await _context.SaveChangesAsync();

            address.City = "Новый город";
            address.AddressName = "Новый адрес";

            // Act
            await _repository.UpdateAsync(address, CancellationToken.None);

            // Assert
            var updatedAddress = await _context.Address.FindAsync(address.AddressId);
            Assert.NotNull(updatedAddress);
            Assert.Equal("Новый город", updatedAddress.City);
            Assert.Equal("Новый адрес", updatedAddress.AddressName);
        }

        [Fact]
        public async Task UpdateAsync_AutomaticallyCallsSaveChanges()
        {
            // Arrange
            var address = CreateAddress("Тест");
            await _context.Address.AddAsync(address);
            await _context.SaveChangesAsync();

            address.City = "Обновлено";

            // Act
            await _repository.UpdateAsync(address, CancellationToken.None);

            // Assert
            _context.ChangeTracker.Clear();
            var verifyAddress = await _context.Address.FindAsync(address.AddressId);
            Assert.Equal("Обновлено", verifyAddress.City);
        }

        [Fact]
        public async Task UpdateAsync_WithNullEntity_ThrowsException()
        {
            // Arrange
            Address nullAddress = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _repository.UpdateAsync(nullAddress, CancellationToken.None)
            );
        }

        [Fact]
        public async Task UpdateAsync_WithDetachedEntity_AttachesAndUpdates()
        {
            // Arrange
            var address = CreateAddress("Тест");
            await _context.Address.AddAsync(address);
            await _context.SaveChangesAsync();

            _context.Entry(address).State = EntityState.Detached;

            address.City = "Изменено";

            // Act
            await _repository.UpdateAsync(address, CancellationToken.None);

            // Assert
            var updatedAddress = await _context.Address.AsNoTracking()
                .FirstOrDefaultAsync(a => a.AddressId == address.AddressId);
            Assert.Equal("Изменено", updatedAddress.City);
        }

        #endregion

        #region DeleteAsync Tests

        [Fact]
        public async Task DeleteAsync_WithExistingId_DeletesEntity()
        {
            // Arrange
            var address = CreateAddress("Удаляемый город");
            await _context.Address.AddAsync(address);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(address.AddressId, CancellationToken.None);

            // Assert
            var deletedAddress = await _context.Address.FindAsync(address.AddressId);
            Assert.Null(deletedAddress);

            var count = await _context.Address.CountAsync();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistentId_DoesNotThrowException()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act & Assert
            await _repository.DeleteAsync(nonExistentId, CancellationToken.None);

            var count = await _context.Address.CountAsync();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task DeleteAsync_AutomaticallyCallsSaveChanges()
        {
            // Arrange
            var address = CreateAddress("Тест");
            await _context.Address.AddAsync(address);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(address.AddressId, CancellationToken.None);

            // Assert
            var count = await _context.Address.CountAsync();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task DeleteAsync_WithMultipleEntities_DeletesOnlySpecified()
        {
            // Arrange
            var address1 = CreateAddress("Город 1");
            var address2 = CreateAddress("Город 2");
            var address3 = CreateAddress("Город 3");
            await _context.Address.AddRangeAsync(address1, address2, address3);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(address2.AddressId, CancellationToken.None);

            // Assert
            var remainingAddresses = await _context.Address.ToListAsync();
            Assert.Equal(2, remainingAddresses.Count);
            Assert.DoesNotContain(remainingAddresses, a => a.AddressId == address2.AddressId);
            Assert.Contains(remainingAddresses, a => a.AddressId == address1.AddressId);
            Assert.Contains(remainingAddresses, a => a.AddressId == address3.AddressId);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public async Task CompleteWorkflow_AddUpdateDelete_WorksCorrectly()
        {
            // Arrange & Act
            var address = CreateAddress("Исходный");
            var added = await _repository.AddAsync(address, CancellationToken.None);
            Assert.Equal(1, await _context.Address.CountAsync());

            // Act
            added.City = "Обновленный";
            await _repository.UpdateAsync(added, CancellationToken.None);
            var updated = await _repository.GetByIdAsync(added.AddressId, CancellationToken.None);
            Assert.Equal("Обновленный", updated.City);

            // Act
            await _repository.DeleteAsync(added.AddressId, CancellationToken.None);
            var deleted = await _repository.GetByIdAsync(added.AddressId, CancellationToken.None);
            Assert.Null(deleted);
            Assert.Equal(0, await _context.Address.CountAsync());
        }

        [Fact]
        public async Task ConcurrentOperations_WorkCorrectly()
        {
            // Arrange
            var addresses = Enumerable.Range(1, 10)
                .Select(i => CreateAddress($"Город {i}"))
                .ToList();

            // Act
            var tasks = addresses.Select(a => _repository.AddAsync(a, CancellationToken.None));
            await Task.WhenAll(tasks);

            // Assert
            var result = await _repository.GetAllAsync(CancellationToken.None);
            Assert.Equal(10, result.Count);
        }

        #endregion
    }
}
