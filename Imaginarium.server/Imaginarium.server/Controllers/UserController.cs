using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Imaginarium.server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : Controller
	{
		private static List<User> currentPlayers = new List<User>();		//список текущих игроков в сессии
		private static List<Card> cards = new List<Card>();          //список всех Карточек на сервере

		private static List<ScoreCards> currentCards = new List<ScoreCards>();  //список всех выбранных карточек

		private static bool isLiquid = true;						//позволяет замешивать карты один раз

		[HttpPost("autorize")]
		public async Task<IActionResult> Autorize(string sendName)
		{
			if (currentPlayers.Any(p => p.name == sendName))
			{
				return NoContent();
			}
			currentPlayers.Add(new User { name = sendName });
			return Ok(currentPlayers.FirstOrDefault(u => u.name == sendName));
		}

		[HttpPost("selectCard")]
		public async Task<IActionResult> SelectCard(int cardId, string name)
		{
			//не работает присваивание карточки на стороне клиента
			var newCard = new ScoreCards { card = currentPlayers.Find(p => p.name == name).cards.Find(c => c.id == cardId), isLeader = currentPlayers.Find(p => p.name == name).isLeader, score = 0 };
			currentCards.Add(newCard);
			//удаляет выбранную карточку из карточек пользователя
            var tmpCard = currentPlayers.Find(p => p.name == name).cards.Find(p => p.id == cardId);
			currentPlayers.Find(p => p.name == name).selectedCard = new Card(tmpCard);
            currentPlayers.Find(p => p.name == name).cards.RemoveAll(p => p.id == cardId);
            return Ok();
		}

		//[HttpGet("startGame")]
		//public async Task<IActionResult> StartGame()
		//{
		//	//Для того, чтобы замешало один раз, а не несколько!
		//	if (isLiquid == true)
		//	{
		//		Random rng = new Random();
		//		var shuffledCards = cards.OrderBy(x => rng.Next()).ToList();
		//		foreach (var player in currentPlayers)
		//		{
		//			player.cards = shuffledCards.Take(2).ToList();
		//			shuffledCards = shuffledCards.Skip(2).ToList();
		//		}
		//		isLiquid = false;
		//		/*			Console.WriteLine("Раздача карточек:");
		//					foreach (var player in currentPlayers)
		//					{
		//						Console.WriteLine($"{player.name}: {string.Join(", ", player.cards.Select(card => card.cardName))}");
		//					}*/
		//		return Ok(currentPlayers);
		//	}
		//	return Ok();
		//}


		[HttpGet("randomCards")]
        public async Task<IActionResult> RandomCards()
        {
            string prop = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName, @"imaginarium.client/ImaginImag/");
            Console.WriteLine(prop);
            if (Directory.Exists(prop))
            {
                // Получение списка файлов с расширением jpg, png, и т.д. (можете настроить под свои нужды)
                List<string> imageFiles = Directory.GetFiles(prop, "*.*", SearchOption.AllDirectories)
                                               .Where(s => s.EndsWith(".jpg") || s.EndsWith(".jpeg") || s.EndsWith(".png") || s.EndsWith(".gif"))
                                               .ToList();
                Console.WriteLine("Список изображений в папке:");
                //обрезаем путь к файлам и оставляем только их контент
                for (int i = 0; i < imageFiles.Count; i++)
                {
                    imageFiles[i] = imageFiles[i].Substring(prop.Length);  //обрезаем
                    Console.WriteLine($"img {i + 1}: {imageFiles[i]}");     //выводим кол-во изображений на экран
                    var newCard = new Card { cardUrl = @$"{prop}{imageFiles[i]}", id = i, cardName = imageFiles[i] };       //создаем экземпляр карточки
                    cards.Add(newCard);      //присваиваем экземпляр
                }
				//Для того, чтобы замешало один раз, а не несколько!
				if (isLiquid == true)
				{
					Random rng = new Random();
					var shuffledCards = cards.OrderBy(x => rng.Next()).ToList();
					foreach (var player in currentPlayers)
					{
						player.cards = shuffledCards.Take(10).ToList();
						shuffledCards = shuffledCards.Skip(10).ToList();
					}
					isLiquid = false;
					/*			Console.WriteLine("Раздача карточек:");
								foreach (var player in currentPlayers)
								{
									Console.WriteLine($"{player.name}: {string.Join(", ", player.cards.Select(card => card.cardName))}");
								}*/
					return Ok(currentPlayers);
				}
				return Ok();
			}
            Console.WriteLine("Указанная папка не существует.");
            return NoContent();
        }

		//обнуляет буль, чтобы карточки могли снова замешиваться
		[HttpPost("endGame")]
		public async Task<IActionResult> EndGame()
		{
			if (isLiquid == false)
				isLiquid = true;
			return Ok();
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

		[HttpGet("getUser")]
		public async Task<IActionResult> getUser(string user) => Ok(currentPlayers.Find(u => u.name == user));
	}
}
