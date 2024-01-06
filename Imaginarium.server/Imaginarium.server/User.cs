using System.ComponentModel.DataAnnotations.Schema;

namespace Imaginarium.server
{
	public class User
	{
		public int id { get; set; }
		public string name { get; set; }
		public bool isReady { get; set; }
	}
}
