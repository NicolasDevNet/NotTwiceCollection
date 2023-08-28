using System.IO;

namespace Assets.NotTwice.UP.ADAccess.Runtime.Wrappers
{
	public class FileAccessWrapper : IFileAccessWrapper
	{
		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		public void CreateDirectory(string path)
		{
			Directory.CreateDirectory(path);
		}

		public string[] DirectoryGetFiles(string path)
		{
			return Directory.GetFiles(path);
		}

		public void FileDelete(string path)
		{
			File.Delete(path);
		}
	}
}
