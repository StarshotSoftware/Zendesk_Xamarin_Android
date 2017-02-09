namespace ZendeskXamarinAndroid.Core.Entities
{
	public class ZendeskUserAccess
	{
		public string oAuthToken { get; set; }
		public string user { get; set; }
		public string password { get; set; }
		public string locale { get; set; }

		public ZendeskUserAccess()
		{
			password = "";
			locale = "en-us";
		}
	}
}

