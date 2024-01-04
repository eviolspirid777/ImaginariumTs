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
			//_ImaginariumContext.Cards.Add(new Card { id = 1, CardName = "MyCard", CardUrl = "vk/ctylg.png", InGame = false });
			//_ImaginariumContext.SaveChanges();
			return Ok(_ImaginariumContext.Cards.ToList());
		}

		
	}
}
