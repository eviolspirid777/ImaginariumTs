namespace Imaginarium.Controllers
{
	public class Room
	{
		public int RoomId { get; set; }
		public List<Users>? Users { get; set; }
		public List<Card>? Cards { get; set; }
	}
}
