using Assets.NotTwice.UP.Addressables.Runtime.Services;
using Assets.NotTwice.UP.SceneManagement.Runtime.ScriptableObjects;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.NotTwice.UP.SceneManagement.Runtime.States
{
	public static partial class ApplicationState
	{
		public static class SceneManagementState
		{
			private static AsyncOperationHandle<IList<SceneConfiguration>> _scenesConfigurationsHandle;

			public static IList<SceneConfiguration> GetSceneConfigurations()
			{
				if (_scenesConfigurationsHandle.IsDone)
				{
					return _scenesConfigurationsHandle.Result;
				}

				return null;
			}

			public static async UniTask<IList<SceneConfiguration>> GetSceneConfigurationsAsync(IAddressablesService addressablesService)
			{
				if(_scenesConfigurationsHandle.IsValid())
				{
					return _scenesConfigurationsHandle.Result;
				}

				var taskScenesResult = await addressablesService.LoadResourceLocationsAsync(Constants.Addressables.ScenesConfigurationsLabel)
						.ContinueWith(async (locationsResult) => await addressablesService.LoadAssetsAsync<SceneConfiguration>(locationsResult.result,
						(resource) => UnityEngine.Debug.Log("Scene configuration loaded: " + resource.name)));

				_scenesConfigurationsHandle = taskScenesResult.operation;

				return _scenesConfigurationsHandle.Result;
			}
		}
	}
}
