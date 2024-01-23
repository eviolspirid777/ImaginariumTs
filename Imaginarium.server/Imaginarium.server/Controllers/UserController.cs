using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Imaginarium.server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : Controller
	{
		private static List<User> currentPlayers = new List<User>();        //список текущих игроков в сессии
		private static List<Card> cards = new List<Card>();          //список всех Карточек на сервере

		private static List<ScoreCardsResults> currentCards = new List<ScoreCardsResults>();  //список всех выбранных карточек

		private static string codeWord = "";

		private static bool isLiquid = true;                        //позволяет замешивать карты один раз
		private static bool isStart = false;                            //запрещает стартовать игру, если сессия уже началась
		private static bool isFetchedScore = true;

		private void NextAdmin()
		{
			if (currentPlayers.Count > 0)
			{
				User currentAdmin = currentPlayers.FirstOrDefault(player => player.isLeader == true)!;
				if (currentAdmin == null || currentPlayers.IndexOf(currentAdmin) == currentPlayers.Count - 1)
				{
					currentPlayers[currentPlayers.Count - 1].isLeader = false;
					currentPlayers[0].isLeader = true;
				}
				else
				{
					int currentIndex = currentPlayers.IndexOf(currentAdmin);
					currentPlayers[currentIndex].isLeader = false;
					currentPlayers[currentIndex + 1].isLeader = true;
				}
				if (currentAdmin != null)
				{
					currentAdmin.isLeader = false;
				}
			}
		}

		[HttpPost("autorize")]
		public async Task<IActionResult> Autorize(string sendName)
		{
			if (isStart == false)
			{
				if (currentPlayers.Any(p => p.name == sendName))
				{
					return NoContent();
				}
				currentPlayers.Add(new User { name = sendName });
				return Ok(currentPlayers.FirstOrDefault(u => u.name == sendName));
			}
			return NotFound();
		}

		[HttpPost("playersReady")]
		public async Task<IActionResult> PlayersReady()
		{
			if (isLiquid == false)
			{
				currentPlayers.ForEach(p => p.isReady = false);
				NextAdmin();
				isLiquid = true; //Запрещает срабатывание этой функции дважды
				codeWord = "";  //чистим кодовое слово
				currentCards = new List<ScoreCardsResults>(); //Чистим прошлый список и создаем новый
			}
			return Ok();
		}

		[HttpPost("selectUserCard")]
		public async Task<IActionResult> SelectUserCard(string authorName, string cardName)
		{
			var card  = currentCards.Find(c => c.card!.cardName == cardName);
			card.score += 1;
			card.name?.Add(authorName);
			currentPlayers.Find(p => p.name ==  authorName).isReady = true;
			currentPlayers.Find(p => p.isLeader == true).isReady = true;
			return Ok();
		}

		[HttpPost("fetchScore")]
		public async Task<IActionResult> FetchScore()
		{
			if(isFetchedScore == true)
			{
				isFetchedScore = false;

				var leader = currentCards.Find(p => p.isLeader == true);
				bool allPlayersGuessedLeader = currentCards.Where(p => p.isLeader != true).All(p => p.card == leader.card);
				if (allPlayersGuessedLeader)
				{
					// Все игроки угадали карточку ведущего
					leader.score -= 3;// Ведущий идет на 3 хода назад
									  // Остальные игроки остаются на месте
					foreach (var player in currentPlayers)
					{
						if (player.isLeader == false)
							player.score += 3; // Все остальные игроки получают по 3 очка
					}
				}
				else
				{
					// Никто не угадал карточку ведущего
					leader.score -= 2; // Ведущий идет на 2 хода назад
									   // Очки получают игроки, чьи карточки угадали
					foreach (var player in currentPlayers)
					{
						if (player.selectedCard != null && player.selectedCard != leader.card)
							player.score += 3; // Игрок получает 3 очка за угаданную карточку
					}
				}
				// Очки для ведущего
				leader.score += 3 + currentPlayers.Count(player => player.selectedCard == leader.card);
				// Очки для остальных игроков за угаданные карточки
				foreach (var player in currentPlayers)
				{
					if (player.selectedCard != null && player.selectedCard != leader.card)
						player.score += 1; // Игрок получает 1 очко за каждого угадавшего его игрока
				}
			}
			return Ok();
		}

		[HttpPost("selectCard")]
		public async Task<IActionResult> SelectCard(int cardId, string name)
		{
			//не работает присваивание карточки на стороне клиента
			var newCard = new ScoreCardsResults { card = currentPlayers.Find(p => p.name == name)!.cards!.Find(c => c.id == cardId)!, isLeader = currentPlayers.Find(p => p.name == name).isLeader, score = 0, owner=name };
			currentCards.Add(newCard);
			currentPlayers.Find(p => p.name == name)!.isReady = true;
			//удаляет выбранную карточку из карточек пользователя
			var tmpCard = currentPlayers.Find(p => p.name == name)!.cards!.Find(p => p.id == cardId);
			currentPlayers.Find(p => p.name == name)!.selectedCard = new Card(tmpCard!);
			currentPlayers.Find(p => p.name == name)!.cards!.RemoveAll(p => p.id == cardId);
			return Ok();
		}

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
					isStart = true;
					NextAdmin();
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
			return Ok(currentPlayers.FirstOrDefault(u => u.name == name)!.isReady);
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

		[HttpPost("postWord")]
		public async Task<IActionResult> PostWord(string word)
		{
			if (word != "")
			{
				codeWord = word;
				Console.WriteLine("Code word set to: " + codeWord);
				return Ok();
			}
			return NoContent();
		}

		[HttpGet("getUsers")]
		public async Task<IActionResult> getAllUsers()
		{
			if (currentPlayers.Count > 0)
				return Ok(currentPlayers.ToList());
			return BadRequest();
		}
		[HttpGet("getCurrentCards")]
		public async Task<IActionResult> GetCurrentCards() => Ok(currentCards.ToList());

		[HttpGet("getUser")]
		public async Task<IActionResult> GetUser(string user) => Ok(currentPlayers.Find(u => u.name == user));

		[HttpGet("getWord")]
		public async Task<IActionResult> GetWord()
		{
			//return Ok( new { name = "Bob", codeWord = codeWord});
			return Ok(codeWord);
		}
		[HttpPost("unReady")]
		public async Task<IActionResult> Unready()
		{
			currentPlayers.ForEach(p => p.isReady = false);
			return Ok();
		}
	}
}
