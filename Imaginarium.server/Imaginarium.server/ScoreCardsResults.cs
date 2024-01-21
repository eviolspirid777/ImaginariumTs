using System;
namespace Imaginarium.server
{
	public class ScoreCardsResults
	{
        public int score { get; set; }
        public bool? isLeader { get; set; }
        public Card? card { get; set; }
        public string? [] name { get; set; }
        public string owner { get; set; }
    }
}

