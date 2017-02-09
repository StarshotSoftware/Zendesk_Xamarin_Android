using System.IO;
using System.Threading.Tasks;

namespace ZendeskXamarinAndroid.Core.Services
{ 
	public interface IMessageImageUploadService
	{
		Task<Stream> GetImageFromLibrary();
		Task<Stream> GetImageFromCamera();
		string ResizeImage(Stream imageData, float width, float height);
	}
}
