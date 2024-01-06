using Microsoft.AspNetCore.Mvc;

namespace Imaginarium.server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : Controller
	{
		private static List<User> currentPlayers = new List<User>();          //список текущих игроков на сервере
		
		[HttpPost("autorize")]
		public async Task<IActionResult> Autorize(string sendName)
		{
			currentPlayers.Add(new User { name = sendName });
			return Ok(currentPlayers.FirstOrDefault(u => u.name == sendName));
		}

		[HttpPost("sliceUser")]
		public async Task<IActionResult> SliceUser(string name)
		{
			var userToDelete = currentPlayers.FirstOrDefault(u => u.name == name);
			if (userToDelete == null)
				return BadRequest();
			currentPlayers.Remove(userToDelete);
			return Ok();
		}

		[HttpGet("checkState")]

		public async Task<IActionResult> CheckState(string name)
		{
			return Ok(currentPlayers.FirstOrDefault(u => u.name == name).isReady);
		}

		[HttpPost("switchReady")]
		public async Task<IActionResult> SwitchReady(string name)
		{
			var userToSwitch = currentPlayers.FirstOrDefault(u => u.name == name);
			if (userToSwitch != null)
			{
				userToSwitch.isReady = !userToSwitch.isReady;
				return Ok();
			}
			return BadRequest();
		}

		[HttpGet("getUsers")]
		public async Task<IActionResult> getAllUsers()
		{
			if (currentPlayers.Count > 0)
				return Ok(currentPlayers.ToList());
			return BadRequest();
		}
	}
}
