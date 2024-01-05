using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Imaginarium.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : Controller
	{
		private ImaginariumContext _ImaginariumContext { get; set; } = null!;
		private static List<Users> currentPlayers = new List<Users>();			//список текущих игроков на сервере


		public UsersController(ImaginariumContext usersContext) 
		{
			_ImaginariumContext = usersContext;
			_ImaginariumContext.Database.EnsureCreated();
		}

		[HttpPost("autorize")]
		public async Task<IActionResult> Autorize(string sendName)
		{
			var user = _ImaginariumContext.Users.FirstOrDefault(u => u.name == sendName);
			//Если пользователь не найден
			if (user == null)
			{
				_ImaginariumContext.Users.Add(new Users { name = sendName });
				_ImaginariumContext.SaveChanges();
				var temp = _ImaginariumContext.Users.FirstOrDefault(u => u.name == sendName);
				currentPlayers.Add(temp);
				return Ok();
			}
			else if (user.name == sendName)
			{
				currentPlayers.Add(user);
				return Ok(user);
			}
			return BadRequest();
		}
		[HttpPost("sliceUser")]
		public async Task<IActionResult> sliceUser(Users user)
		{
			var userToDelete = _ImaginariumContext.Users.FirstOrDefault(u => u.id == user.id);
			if (userToDelete == null)
				return BadRequest();
			_ImaginariumContext.Users.Remove(userToDelete);
			currentPlayers.RemoveAll(u => u.name == user.name);
			await _ImaginariumContext.SaveChangesAsync();
			return Ok();
		}

		[HttpGet("getUsers")]
		public async Task<IActionResult> getAllUsers()
		{
			if(currentPlayers.Count > 0)
				return Ok(currentPlayers.ToList());
			return BadRequest();
		}

		[HttpGet("listUsers")]
		public async Task<IActionResult> listUsers()
		{
			return Ok(_ImaginariumContext.Users.ToList());
		}
	}
}
