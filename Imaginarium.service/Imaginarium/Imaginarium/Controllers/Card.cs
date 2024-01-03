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
        public string cardUrl { get; set; }
        public string cardName { get; set; }
        public bool inGame { get; set; }
    }
}
