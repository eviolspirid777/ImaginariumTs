using Microsoft.AspNetCore.Mvc;

namespace Imaginarium.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CardsController : Controller
	{
		private ImaginariumContext _ImaginariumContext { get; set; } = null!;
		
		public CardsController(ImaginariumContext cardsContext)
		{
			_ImaginariumContext = cardsContext;
			_ImaginariumContext.Database.EnsureCreated();
		}

		[HttpGet("listCards")]
		public async Task<IActionResult> ListCards()
		{
			return Ok(_ImaginariumContext.Cards.ToList());
		}
	}
}
