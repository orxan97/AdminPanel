using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.Common;

namespace WebApplication2.Context;

public class AppDbContext:IdentityDbContext<AppUser>
{

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{

	}

	public DbSet<Slider> Sliders { get; set; } = null!;
	public DbSet<Service> Services { get; set; } = null!;
	public DbSet<Category> Categories { get; set; } = null!;
	public DbSet<Product> Products { get; set; } = null!;

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var entiries = ChangeTracker.Entries<BaseEntity>();
		foreach (var entry in entiries)
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedAt = DateTime.UtcNow;
					entry.Entity.ModifiedAt = DateTime.UtcNow;

					break;
				case EntityState.Modified:
					entry.Entity.ModifiedAt = DateTime.UtcNow;
					break;
			}
		}
		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
		base.OnModelCreating(modelBuilder);
	}
}
