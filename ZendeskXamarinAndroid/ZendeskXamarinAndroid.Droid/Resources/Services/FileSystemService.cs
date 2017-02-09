using System.IO;
using ZendeskXamarinAndroid.Core.Services;

namespace ZendeskXamarinAndroid.Droid
{
	public class FileSystemService : IFileSystem
	{
		public byte[] ReadAllByteS(string path)
		{
			return File.ReadAllBytes(path);
		}
	}
}