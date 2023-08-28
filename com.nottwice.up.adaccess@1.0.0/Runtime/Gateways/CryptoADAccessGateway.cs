using Zenject;
using System.Security.Cryptography;
using System;
using System.IO;
using UnityEngine;
using Assets.NotTwice.UP.PlayerPrefsAccess.Runtime.Wrappers;
using Assets.NotTwice.UP.ADAccess.Runtime.Wrappers;
using Cysharp.Threading.Tasks;

namespace Assets.NotTwice.UP.ADAccess.Runtime.Gateways
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public class CryptoADAccessGateway<T> : BaseADAccessGateway<T>, IApplicationDataAccessGateway<T>
		where T : class
	{
		private readonly IPlayerPrefsWrapper _playerPrefsWrapper;

		private const string _encryptionKeyName = "EncryptionKey";

		public CryptoADAccessGateway([Inject(Id = "applicationDataPath")] string applicationDataPath,
		IPlayerPrefsWrapper playerPrefsWrapper,
		IFileAccessWrapper fileAccessWrapper,
		ILogger logger) : base(applicationDataPath, fileAccessWrapper, logger)
		{
			_playerPrefsWrapper = playerPrefsWrapper;

			CreateAsyncDel = CryptoCreateAsync;
			ReadAsyncDel = CryptoReadAsync;
		}

		private async UniTask<bool> CryptoCreateAsync(FileStream fileStream, T data)
		{
			Aes aes = Aes.Create();
			SetEncryptionKey(aes.Key);

			var inputIV = aes.IV;

			fileStream.Write(inputIV, 0, inputIV.Length);

			using (CryptoStream cryptoStream = new CryptoStream(fileStream,
				aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
			{
				using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
				{
					return await WriteStreamAsync(streamWriter, data);
				}
			}
		}

		private async UniTask<T> CryptoReadAsync(FileStream fileStream)
		{
			var encryptionKey = GetEncryptionKey();

			if (encryptionKey == null)
			{
				return null;
			}

			Aes aes = Aes.Create();
			var outputIV = new byte[aes.IV.Length];

			fileStream.Read(outputIV, 0, outputIV.Length);

			using (CryptoStream cryptoStream = new CryptoStream(fileStream,
				aes.CreateDecryptor(encryptionKey, outputIV), CryptoStreamMode.Read))
			{
				using (StreamReader streamReader = new StreamReader(cryptoStream))
				{
					return await ReadStreamAsync(streamReader);
				}
			}
		}

		#region Private

		private byte[] GetEncryptionKey()
		{
			if (_playerPrefsWrapper.HasKey(_encryptionKeyName))
				return Convert.FromBase64String(_playerPrefsWrapper.GetString(_encryptionKeyName));

			return null;
		}

		private void SetEncryptionKey(byte[] key)
		{
			if (!_playerPrefsWrapper.HasKey(_encryptionKeyName))
				_playerPrefsWrapper.SetString(_encryptionKeyName, Convert.ToBase64String(key));
		}

		#endregion
	}
}
