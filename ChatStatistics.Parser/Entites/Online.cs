using System.Collections.Generic;

namespace ChatStatistics.Parser.Entites
{
	class Online
	{
		public string type { get; set; }
		public int chat_id { get; set; }
		public string title { get; set; }
		public int admin_id { get; set; }
		public List<User> users { get; set; }
	}
}
