using System;
using ZendeskXamarinAndroid.Core;

namespace ZendeskXamarinAndroid.Utils
{
	public class ResourceStrings
	{

		#region Threadsafe singleton methods
		private static volatile ResourceStrings instance;
		private static object syncRoot = new Object();
		public static ResourceStrings Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
						{
							instance = new ResourceStrings();
						}
					}
				}
				return instance;
			}
		}
		#endregion

		private ResourceStrings()
		{
		}

		public string GetString(string value)
		{
			var result = StringResources.ResourceManager.GetString(value);
			return result;
		}

	}
}

