using Microsoft.AspNetCore.Mvc;

namespace Imaginarium.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : Controller
	{
		private ImaginariumContext _ImaginariumContext { get; set; } = null!;


		public UsersController(ImaginariumContext usersContext) 
		{
			_ImaginariumContext = usersContext;
			_ImaginariumContext.Database.EnsureCreated();
		}

		[HttpGet("autorize")]
		public async Task<IActionResult> Get(string name, string password)
		{
			var user = _ImaginariumContext.Users.FirstOrDefault(u => u.name == name);
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
		[HttpGet("listCards")]
		public async Task<IActionResult> ListAll()
		{
			return Ok(_ImaginariumContext.Users.ToList());
		}

		[HttpPost("register")]
		public async Task<IActionResult> register(Users user)
		{
			//Проверка на наличие студента в бд
			if (_ImaginariumContext.Users.Contains(user))
			{
				return BadRequest();
			}
			_ImaginariumContext.Users.Add(user);
			_ImaginariumContext.SaveChanges();
			return Ok();
		}
	}
}
