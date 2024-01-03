using Microsoft.AspNetCore.Mvc;

namespace Imaginarium.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : Controller
	{
		private UsersContext _context { get; set; } = null!;

		public UsersController(UsersContext context) 
		{
			_context = context;
			_context.Database.EnsureCreated();
		}

		[HttpGet("autorize")]
		public async Task<IActionResult> Get(string name, string password)
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
		[HttpGet("list")]
		public async Task<IActionResult> ListAll()
		{
			return Ok(_context.Users.ToList());
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
