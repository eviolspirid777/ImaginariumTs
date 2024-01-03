using Microsoft.EntityFrameworkCore;

namespace Imaginarium.Controllers
{
	public class UsersContext : DbContext
	{
		public DbSet<Users> Users { get; set; } = null!;

		public UsersContext(DbContextOptions<UsersContext> options) : base(options) 
		{
			Database.EnsureCreated();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Users>().HasData(
				new Users { id = 1, name = "Kirill", password = "123123" }
			);
		}

	}
}
