using UnityEngine;

namespace Assets.NotTwice.UP.PlayerPrefsAccess.Runtime.Wrappers
{
	public class PlayerPrefsWrapper : IPlayerPrefsWrapper
	{
		public string GetString(string key, string defaultValue = null)
		{
			return PlayerPrefs.GetString(key, defaultValue);
		}

		public void SetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}

		public int GetInt(string key, int defaultValue = 0)
		{
			return PlayerPrefs.GetInt(key, defaultValue);
		}

		public void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}

		public float GetFloat(string key, float defaultValue = 0)
		{
			return PlayerPrefs.GetFloat(key, defaultValue);
		}

		public void SetFloat(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
		}

		public bool HasKey(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		public void Save()
		{
			PlayerPrefs.Save();
		}
	}
}
