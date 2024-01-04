using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imaginarium.Controllers
{
	[Table("Cards")]
	public class Card
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public string CardUrl { get; set; }
		public bool InGame { get; set; } //erjgejtrg
		public string CardName { get; set; }
		public Room? Room { get; set; }
	}
}
