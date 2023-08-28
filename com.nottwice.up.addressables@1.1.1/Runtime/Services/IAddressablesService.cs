using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Assets.NotTwice.UP.Addressables.Runtime.Services
{
	public interface IAddressablesService
	{
		UniTask<(IResourceLocator result, AsyncOperationHandle<IResourceLocator> operation)> InitializeAsync();
		UniTask<(T result, AsyncOperationHandle<T> operation)> LoadAssetAsync<T>(object address) where T : class;
		UniTask<(IList<T> result, AsyncOperationHandle<IList<T>> operation)> LoadAssetsAsync<T>(IList<IResourceLocation> keys, Action<T> callback) where T : class;
		UniTask<(IList<T> result, AsyncOperationHandle<IList<T>> operation)> LoadAssetsAsync<T>(string address, Action<T> callback) where T : class;
		UniTask<(IList<IResourceLocation> result, AsyncOperationHandle<IList<IResourceLocation>> operation)> LoadResourceLocationsAsync(IList<object> keys, Type type = null);
		UniTask<(IList<IResourceLocation> result, AsyncOperationHandle<IList<IResourceLocation>> operation)> LoadResourceLocationsAsync(object address, Type type = null);
		UniTask<(IList<IResourceLocation> result, AsyncOperationHandle<IList<IResourceLocation>> operation)> LoadResourceLocationsAsync<T>(object address);
		UniTask<(IList<IResourceLocation> result, AsyncOperationHandle<IList<IResourceLocation>> operation)> LoadResourceLocationsAsync<T>(IList<object> keys);
		UniTask<(SceneInstance result, AsyncOperationHandle<SceneInstance> operation)> LoadSceneAsync(IResourceLocator resourceLocator);
		UniTask<(SceneInstance result, AsyncOperationHandle<SceneInstance> operation)> LoadSceneAsync(object address, LoadSceneMode loadSceneMode);
		void Release(AsyncOperationHandle operation);
		void Release<T>(AsyncOperationHandle<T> operation);
		void Release<T>(T resource);
	}
}