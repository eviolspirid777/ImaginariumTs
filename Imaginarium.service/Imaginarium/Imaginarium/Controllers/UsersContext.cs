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
	}
}
