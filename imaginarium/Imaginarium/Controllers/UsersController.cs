using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Imaginarium.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		public UsersContext _context;
		public UsersController(UsersContext context) 
		{
			_context = context;
		}
		[HttpGet("autorize")]
		public async Task<IActionResult> Getter(string name, string password)
		{
			var user = _context.Users.FirstOrDefault(u => u.name == name);
			//Если пользователь не найден
			if (user == null)
			{
				return NotFound();
			}
			if (user.password == password)
			{
				return Ok(user);
			}
			return BadRequest();
		}

		[HttpPost("register")]
		public async Task<IActionResult> register(Users user)
		{
			//Проверка на наличие студента в бд
			if (_context.Users.Contains(user))
			{
				return BadRequest();
			}
			_context.Users.Add(user);
			 _context.SaveChanges();
			return Ok();
		}
	}
}
