using System;
using Microsoft.AspNetCore.SignalR;

namespace Imaginarium.server.Hubs
{
	public class UsersHub : Hub
	{
		//Определяем таску, которая будет прослушиваться с клиента и реализовывать какую-либо логику
		public async Task Send()
		{
			//Определяем ответ, который будте применяться ко всем юзерам
			await this.Clients.All.SendAsync("recieve", "hello");
		}
	}
}

