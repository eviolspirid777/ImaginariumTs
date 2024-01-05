using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Imaginarium.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RoomController : Controller
	{
		private ImaginariumContext _context { get; set; } = null!;

		public RoomController(ImaginariumContext context)
		{
			_context = context;
		}

		[HttpPost("postRoom")]
		public async Task<IActionResult> postRoom()
		{
			var room = new Room();
			room.Cards = new List<Card>();
			room.Users = new List<Users>();
			room.Cards.Add(new Card { CardName = "HisCard2", CardUrl = "my/url3", InGame = false, id = 3 });
			var usersList = new List<Users>() { new Users { id = 3, name = "Him" }, new Users { id = 4, name = "Him2" } };
			room.Users.AddRange(usersList);
			_context.Rooms.Add(room);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpGet("listRooms")]
		public async Task<IActionResult> getRooms()
		{
			var rooms = _context.Rooms
				.Include(r => r.Users)
				.Include(r => r.Cards)
				.ToList();

			var result = rooms.Select(r => new
			{
				id = r.RoomId,
				Users = r.Users.Select(u => new Users
				{
					id = u.id,
					name = u.name
				}),
				Cards = r.Cards.Select(c => new Card
				{
					id = c.id,
					CardUrl = c.CardUrl,
					InGame = c.InGame,
					CardName = c.CardName
				})
			});

			return Ok(result);
		}
	}
}
