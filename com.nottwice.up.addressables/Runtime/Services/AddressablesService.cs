using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityAddressables = UnityEngine.AddressableAssets.Addressables;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace Assets.NotTwice.UP.Addressables.Runtime.Services
{
	public class AddressablesService : IAddressablesService
	{
		public async UniTask<(SceneInstance result, AsyncOperationHandle<SceneInstance> operation)> LoadSceneAsync(object address, LoadSceneMode loadSceneMode)
		{
			return await GetAsyncResult(UnityAddressables.LoadSceneAsync(address, loadSceneMode, true));
		}

		public async UniTask<(SceneInstance result, AsyncOperationHandle<SceneInstance> operation)> LoadSceneAsync(IResourceLocator resourceLocator)
		{
			return await GetAsyncResult(UnityAddressables.LoadSceneAsync(resourceLocator));
		}

		public async UniTask<(T result, AsyncOperationHandle<T> operation)> LoadAssetAsync<T>(object address) where T : class
		{
			return await GetAsyncResult(UnityAddressables.LoadAssetAsync<T>(address));
		}

		public async UniTask<(IList<T> result, AsyncOperationHandle<IList<T>> operation)> LoadAssetsAsync<T>(string address, Action<T> callback) where T : class
		{
			return await GetAsyncResult(UnityAddressables.LoadAssetsAsync(address, callback, UnityAddressables.MergeMode.Union, true));
		}

		public async UniTask<(IList<T> result, AsyncOperationHandle<IList<T>> operation)> LoadAssetsAsync<T>(IList<IResourceLocation> keys, Action<T> callback) where T : class
		{
			return await GetAsyncResult(UnityAddressables.LoadAssetsAsync(keys, callback));
		}

		public async UniTask<(IResourceLocator result, AsyncOperationHandle<IResourceLocator> operation)> InitializeAsync()
		{
			return await GetAsyncResult(UnityAddressables.InitializeAsync());
		}

		public async UniTask<(IList<IResourceLocation> result, AsyncOperationHandle<IList<IResourceLocation>> operation)> LoadResourceLocationsAsync(IList<object> keys, Type type = null)
		{
			return await GetAsyncResult(UnityAddressables.LoadResourceLocationsAsync(keys, type));
		}

		public async UniTask<(IList<IResourceLocation> result, AsyncOperationHandle<IList<IResourceLocation>> operation)> LoadResourceLocationsAsync(object address, Type type = null)
		{
			return await GetAsyncResult(UnityAddressables.LoadResourceLocationsAsync(address, type));
		}

		public async UniTask<(IList<IResourceLocation> result, AsyncOperationHandle<IList<IResourceLocation>> operation)> LoadResourceLocationsAsync<T>(object address)
		{
			return await LoadResourceLocationsAsync(address, typeof(T));
		}

		public async UniTask<(IList<IResourceLocation> result, AsyncOperationHandle<IList<IResourceLocation>> operation)> LoadResourceLocationsAsync<T>(IList<object> keys)
		{
			return await LoadResourceLocationsAsync(keys, typeof(T));
		}

		public void Release<T>(T resource)
		{
			UnityAddressables.Release(resource);
		}

		public void Release<T>(AsyncOperationHandle<T> operation)
		{
			UnityAddressables.Release(operation);
		}

		public void Release(AsyncOperationHandle operation)
		{
			UnityAddressables.Release(operation);
		}

		private static async UniTask<(T result, AsyncOperationHandle<T> operation)> GetAsyncResult<T>(AsyncOperationHandle<T> operation, CancellationToken cancellationToken = default)
		{
			await UniTask.WaitUntil(() => operation.IsDone, cancellationToken: cancellationToken);
			return (operation.Result, operation);
		}
	}
}
