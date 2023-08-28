using Zenject;
using System.IO;
using UnityEngine;
using Assets.NotTwice.UP.ADAccess.Runtime.Wrappers;
using Cysharp.Threading.Tasks;
using MemoryPack;
using System.Text;

namespace Assets.NotTwice.UP.ADAccess.Runtime.Gateways
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public class ADAccessGateway<T> : BaseADAccessGateway<T>, IApplicationDataAccessGateway<T>
		where T : class
	{
		public ADAccessGateway([Inject(Id = "applicationDataPath")] string applicationDataPath,
		IFileAccessWrapper fileAccessWrapper,
		ILogger logger) : base(applicationDataPath, fileAccessWrapper, logger)
		{
			CreateAsyncDel = FileCreateAsync;
			ReadAsyncDel = FileReadAsync;
		}

		private async UniTask<bool> FileCreateAsync(FileStream fileStream, T data)
		{
			using (StreamWriter streamWriter = new StreamWriter(fileStream))
			{
				return await WriteStreamAsync(streamWriter, data);
			}
		}

		private async UniTask<T> FileReadAsync(FileStream fileStream)
		{
			using (StreamReader streamReader = new StreamReader(fileStream))
			{
				return await ReadStreamAsync(streamReader);
			}
		}
	}
}
