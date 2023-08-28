using Assets.NotTwice.UP.PlayerPrefsAccess.Runtime.Wrappers;
using MemoryPack;
using System.Text;
using UnityEngine;

namespace Assets.NotTwice.UP.PlayerPrefsAccess.Runtime.Gateways
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public class PlayerPrefsAccessGateway : IPlayerPrefsAccessGateway
	{
		private readonly IPlayerPrefsWrapper _playerPrefsWrapper;

		public PlayerPrefsAccessGateway(IPlayerPrefsWrapper playerPrefsWrapper)
		{
			_playerPrefsWrapper = playerPrefsWrapper;
		}

		#region Get

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public T Get<T>() where T : class
		{
			var key = typeof(T).Name;

			return Get<T>(key);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public T Get<T>(string key) where T : class
		{
			if (!_playerPrefsWrapper.HasKey(key))
				return null;

			return MemoryPackSerializer.Deserialize<T>(Encoding.UTF8.GetBytes(_playerPrefsWrapper.GetString(key)));
		}

		#endregion

		#region Set

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Set<T>(T value) where T : class
		{
			var key = typeof(T).Name;

			Set(key, value);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Set<T>(string key, T value) where T : class
		{
			_playerPrefsWrapper.SetString(key, Encoding.UTF8.GetString(MemoryPackSerializer.Serialize(value)));

			_playerPrefsWrapper.Save();
		}

		#endregion

	}
}
