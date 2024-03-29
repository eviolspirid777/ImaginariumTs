﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Imaginarium.server
{
	public class User
	{
		public int id { get; set; }
		public string name { get; set; }
		public bool isReady { get; set; }
		public bool? isLeader { get; set; } = false;
		public int? score { get; set; } = 0;
        public List<Card>? cards { get; set; } = null;
		public Card? selectedCard { get; set; }
	}
}
