using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Persistence.File;

public class CustomDbSet : DbSet<User>
{
    public override IEntityType EntityType => throw new NotImplementedException();

}

public class MyDbContext : DbContext
{
    private readonly string storagePath;

    public virtual DbSet<User> Users { get; set; }

    /// <summary>
    /// To create MyDbContext, please specific the folder path where we store all json files
    /// </summary>
    /// <param name="storagePath"></param>
    public MyDbContext(Settings settings)
    {
        this.storagePath = settings.FolderPath;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.ChangeTracker.DetectChanges();

        var latestRecords = this.ChangeTracker
            .Entries<User>()
            .Where(p => p.State != EntityState.Deleted)
            .Select(p => p.Entity)
            .ToArray();


        string fileName = Path.Combine(storagePath, Path.ChangeExtension(nameof(Users), ".json"));

        // We should use stream to write large size contents
        using FileStream createStream = System.IO.File.Create(fileName);
        await JsonSerializer.SerializeAsync(createStream, latestRecords, cancellationToken: cancellationToken);
        await createStream.DisposeAsync();

        this.ChangeTracker.AcceptAllChanges();

        using FileStream openStream = System.IO.File.OpenRead(fileName);
        var users = await JsonSerializer.DeserializeAsync<List<User>>(openStream);

        return latestRecords.Length;
    }


}

