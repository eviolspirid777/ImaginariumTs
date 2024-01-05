﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Imaginarium.Controllers
{
	public class Users
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public string name { get; set; }
		[NotMapped]
		public bool isReady { get; set; } = false;
		public Room? Room { get; set; }
	}
}
