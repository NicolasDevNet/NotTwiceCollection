namespace Assets.NotTwice.UP.ADAccess.Runtime.Wrappers
{
	public interface IFileAccessWrapper
	{
		void CreateDirectory(string path);
		bool DirectoryExists(string path);
		string[] DirectoryGetFiles(string path);
		void FileDelete(string path);
		bool FileExists(string path);
	}
}