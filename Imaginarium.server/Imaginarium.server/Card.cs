using System.ComponentModel.DataAnnotations;

namespace Imaginarium.server
{
	public class Card
	{
		[Key]
		public int id { get; set; }
        public string cardUrl { get; set; }
		public string? cardName { get; set; }

		// Пустой конструктор
		public Card()
		{
		}

		// Конструктор для создания копии Card
		public Card(Card other)
		{
			id = other.id;
			cardUrl = other.cardUrl;
			cardName = other.cardName;
		}
	}
}
