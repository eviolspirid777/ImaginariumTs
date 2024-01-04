using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imaginarium.Controllers
{
	public class Users
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public string name { get; set; }
		public string password { get; set; }
		public Room? Room { get; set; }
	}
}
