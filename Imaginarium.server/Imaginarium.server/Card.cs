using System.ComponentModel.DataAnnotations;

namespace Imaginarium.server
{
	public class Card
	{
		[Key]
		public int id { get; set; }
        public string cardUrl { get; set; }
		public string? cardName { get; set; }
    }
}
