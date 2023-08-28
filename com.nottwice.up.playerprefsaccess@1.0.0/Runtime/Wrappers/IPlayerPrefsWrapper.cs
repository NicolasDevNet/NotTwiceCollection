namespace Assets.NotTwice.UP.PlayerPrefsAccess.Runtime.Wrappers
{
	public interface IPlayerPrefsWrapper
	{
		float GetFloat(string key, float defaultValue = 0);
		int GetInt(string key, int defaultValue = 0);
		string GetString(string key, string defaultValue = null);
		bool HasKey(string key);
		void Save();
		void SetFloat(string key, float value);
		void SetInt(string key, int value);
		void SetString(string key, string value);
	}
}