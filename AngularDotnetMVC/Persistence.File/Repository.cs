using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Logging;
using Persistence.File;

namespace Persistence
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly string filePath;

        private readonly Settings _settings;
        private readonly ILogger _logger;

        public Repository(Settings settings, ILogger<Repository<TEntity>> logger)
        {

            this._settings = settings;
            this._logger = logger;

            //if (string.IsNullOrWhiteSpace(_settings.StoragePath))
            //    _settings.StoragePath = Path.Combine(Directory.GetCurrentDirectory(), "data");

            filePath = Path.Combine(_settings.FolderPath, Path.ChangeExtension(typeof(TEntity).Name.ToLower(), ".json"));

        }

        public async Task<IQueryable<TEntity>> GetAllAsync()
        {
            try
            {
                using FileStream openStream = System.IO.File.OpenRead(filePath);

                var users = await JsonSerializer.DeserializeAsync<List<TEntity>>(openStream);

                return users.AsQueryable();
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException || ex is FileNotFoundException)
            {
                _logger.LogWarning(ex, "Thrown exception");

                return Enumerable.Empty<TEntity>().AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Thrown exception");

                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _ = entity ?? throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");

            try
            {
                var allRecords = (await this.GetAllAsync()).ToList();
                allRecords.Add(entity);

                await SaveToFileAsync(allRecords);

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Thrown exception");

                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        private async Task SaveToFileAsync(List<TEntity> allRecords)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            // We should use stream to write large size contents
            FileStream createStream = System.IO.File.Create(filePath);

            await JsonSerializer.SerializeAsync(createStream, allRecords);
            await createStream.DisposeAsync();

        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }
    }
}

