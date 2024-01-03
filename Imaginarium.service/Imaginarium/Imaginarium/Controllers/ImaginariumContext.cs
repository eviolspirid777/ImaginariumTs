using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Imaginarium.Controllers
{
	public class ImaginariumContext : DbContext
	{
		public DbSet<Users> Users { get; set; } = null!;
		public DbSet<Card> Cards { get; set; } = null!;

		public ImaginariumContext(DbContextOptions<ImaginariumContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
	}
}
