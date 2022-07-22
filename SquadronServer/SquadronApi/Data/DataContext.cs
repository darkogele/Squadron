using Microsoft.EntityFrameworkCore;
using SquadronApi.Entities;

namespace SquadronApi.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options) { }

    public DbSet<UploadedFileLine> FileLines => Set<UploadedFileLine>();
    public DbSet<UploadedFile> File => Set<UploadedFile>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}