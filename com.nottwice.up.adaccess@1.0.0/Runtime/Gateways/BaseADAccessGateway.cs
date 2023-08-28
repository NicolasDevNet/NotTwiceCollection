using Assets.NotTwice.UP.ADAccess.Runtime.Wrappers;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using MemoryPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NotTwice.UP.ADAccess.Runtime.Gateways
{
	public class BaseADAccessGateway<T> where T : class
	{
		private readonly IFileAccessWrapper _fileAccessWrapper;
		private readonly ILogger _logger;

		private readonly string _folderPath;

		#region Delegates

		public delegate UniTask<bool> CreateAsyncDelegate(FileStream fileStream, T data);
		public delegate UniTask<T> ReadAsyncDelegate(FileStream fileStream);

		protected CreateAsyncDelegate CreateAsyncDel;
		protected ReadAsyncDelegate ReadAsyncDel;

		#endregion

		public BaseADAccessGateway(string applicationDataPath,
		IFileAccessWrapper fileAccessWrapper,
		ILogger logger)
		{
			if (string.IsNullOrEmpty(applicationDataPath))
			{
				throw new ArgumentNullException(nameof(applicationDataPath), "The file storage path has not been provided.");
			}

			_folderPath = ZString.Format("{0}{1}/",
				applicationDataPath.Last().Equals("/") ? applicationDataPath : ZString.Concat(applicationDataPath, "/"),
				typeof(T).Name);

			_fileAccessWrapper = fileAccessWrapper;

			if (!_fileAccessWrapper.DirectoryExists(_folderPath))
			{
				_fileAccessWrapper.CreateDirectory(_folderPath);
			}

			_logger = logger;
		}

		#region Create

		public async UniTask<bool> CreateAsync(T data, int id)
		{
			var filePath = GetFilePath(id);

			if (_fileAccessWrapper.FileExists(filePath))
			{
				return false;
			}

			try
			{
				using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
				{
					return await CreateAsyncDel(fileStream, data);
				}
			}
			catch (Exception ex)
			{
				_logger.LogException(ex);
				return false;
			}
		}

		#endregion

		#region Read

		public async UniTask<T> ReadAsync(int id)
		{
			var filePath = GetFilePath(id);

			if (filePath == null)
			{
				return null;
			}

			try
			{
				using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				{
					return await ReadAsyncDel(fileStream);
				};
			}
			catch (Exception ex)
			{
				_logger.LogException(ex);
				return null;
			}
		}

		public async IAsyncEnumerable<(T data, int id)> ReadAllAsync()
		{
			var filePaths = _fileAccessWrapper.DirectoryGetFiles(_folderPath);

			if (filePaths.Length == 0)
				yield return default((T data, int id));

			foreach (var filePath in filePaths)
			{
				var id = GetFileIdWithName(filePath);
				yield return (await ReadAsync(id), id);
				await UniTask.Yield();
			}
		}

		#endregion

		#region Update

		public async UniTask<bool> UpdateAsync(T data, int id)
		{
			var filePath = GetFilePath(id);

			if (!_fileAccessWrapper.FileExists(filePath))
			{
				return false;
			}

			try
			{
				using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
				{
					fileStream.SetLength(0);
					return await CreateAsyncDel(fileStream, data);
				}
			}
			catch (Exception ex)
			{
				_logger.LogException(ex);
				return false;
			}
		}

		#endregion

		#region Delete

		public bool Delete(int id)
		{
			var filePath = GetFilePath(id);

			if (!_fileAccessWrapper.FileExists(filePath))
				return false;

			try
			{
				_fileAccessWrapper.FileDelete(filePath);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogException(ex);
				return false;
			}
		}

		#endregion

		#region Protected

		protected async UniTask<bool> WriteStreamAsync(StreamWriter streamWriter, T data)
		{
			byte[] content = MemoryPackSerializer.Serialize(data);
			await streamWriter.WriteAsync(Encoding.UTF8.GetString(content));
			return true;
		}

		protected async UniTask<T> ReadStreamAsync(StreamReader streamReader)
		{
			string content = await streamReader.ReadToEndAsync();
			return MemoryPackSerializer.Deserialize<T>(Encoding.UTF8.GetBytes(content));
		}

		#endregion

		#region Private

		private string GetFilePath(int id)
		{
			return ZString.Format("{0}{1}_{2}.json", _folderPath, typeof(T).Name, id);
		}

		private int GetFileIdWithName(string fileName)
		{
			var fileNameParts = fileName.Split('_');

			if (fileNameParts.Length != 2)
				throw new FileNotFoundException("The file collection has been modified.");

			var id = fileNameParts[1].Split(".")[0];

			if (int.TryParse(id, out int intId))
				return intId;

			throw new InvalidCastException("The file identifier was not recognized.");
		}

		#endregion
	}
}
