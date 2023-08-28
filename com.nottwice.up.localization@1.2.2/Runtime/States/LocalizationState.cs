using Assets.NotTwice.UP.Addressables.Runtime.Services;
using Assets.NotTwice.UP.Localization.Runtime.Enums;
using Assets.NotTwice.UP.Localization.Runtime.ScriptableObjects;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Assets.NotTwice.UP.Localization.Runtime.States
{
	public static partial class ApplicationState
	{
		public static class LocalizationState
		{
			/// <summary>
			/// Current language of the application as reactive property
			/// </summary>
			public static ReactiveProperty<Language> CurrentLanguage { get; set; }

			/// <summary>
			/// Old language of the application for rollback
			/// </summary>
			public static Language PreviousLanguage { get; set; } = Language.None;

			#region Resources Handlers

			private static AsyncOperationHandle<LocalizationConfiguration> _localizationConfigurationHandle;

			private static AsyncOperationHandle<IList<LocalizationResource>> _sharedResourcesHandle;

			private static AsyncOperationHandle<IList<LocalizationResource>> _sceneResourcesHandle;

			#endregion

			#region Get resources

			/// <summary>
			/// Translation system configuration method
			/// </summary>
			public static LocalizationConfiguration GetLocalizationConfiguration()
			{
				if(_localizationConfigurationHandle.IsValid())
				{
					return _localizationConfigurationHandle.Result;
				}

				return null;
			}

			/// <summary>
			/// Method for asynchronously loading the translation system configuration with the addressable system
			/// </summary>
			public static async UniTask<LocalizationConfiguration> GetLocalizationConfigurationAsync(IAddressablesService addressablesService)
			{
				if (_localizationConfigurationHandle.IsValid())
				{
					return _localizationConfigurationHandle.Result;
				}

				var taskResult = await addressablesService.LoadAssetAsync<LocalizationConfiguration>(Constants.Addressables.LocalizationConfigurationLabel);

				_localizationConfigurationHandle = taskResult.operation;

				return _localizationConfigurationHandle.Result;
			}

			/// <summary>
			/// Method for obtaining shared translation resources
			/// </summary>
			public static IList<LocalizationResource> GetSharedResources()
			{
				if (_sharedResourcesHandle.IsDone)
				{
					return _sharedResourcesHandle.Result;
				}

				return null;
			}

			/// <summary>
			/// Method for asynchronous loading of translation resources shared with the addressable system
			/// </summary>
			public static async UniTask<IList<LocalizationResource>> GetSharedResourcesAsync(IAddressablesService addressablesService)
			{
				if (_sharedResourcesHandle.IsValid())
				{
					return _sharedResourcesHandle.Result;
				}

				var taskResult = await addressablesService.LoadResourceLocationsAsync(Constants.Addressables.SharedResourcesLabel)
								.ContinueWith(async (locationsResult) => await addressablesService.LoadAssetsAsync<LocalizationResource>(locationsResult.result,
								(resource) => Debug.Log("Shared resource loaded: " + resource.name)));

				_sharedResourcesHandle = taskResult.operation;

				return _sharedResourcesHandle.Result;
			}

			/// <summary>
			/// Method for obtaining translation resources for a scene
			/// </summary>
			public static IList<LocalizationResource> GetSceneResources()
			{
				if (_sceneResourcesHandle.IsDone)
				{
					return _sceneResourcesHandle.Result;
				}

				return null;
			}

			/// <summary>
			/// Method for asynchronous loading of translation resources for a scene with the addressable system
			/// </summary>
			public static async UniTask<IList<LocalizationResource>> GetSceneResourcesAsync(IAddressablesService addressablesService)
			{
				if (_sceneResourcesHandle.IsValid())
				{
					return _sceneResourcesHandle.Result;
				}

				var taskResult = await addressablesService.LoadResourceLocationsAsync(ZString.Format(Constants.Addressables.SceneResourcesLabel, SceneManager.GetActiveScene().name))
					.ContinueWith(async (locationsResult) => await addressablesService.LoadAssetsAsync<LocalizationResource>(locationsResult.result,
					(resource) => Debug.Log("Scene resource loaded: " + resource.name)));

				_sceneResourcesHandle = taskResult.operation;

				return _sceneResourcesHandle.Result;
			}

			#endregion

			/// <summary>
			/// Method for releasing translation resources that are no longer used for the scene
			/// </summary>
			public static void ReleaseSceneResources(IAddressablesService addressablesService)
			{
				addressablesService.Release(_sceneResourcesHandle);
			}

			/// <summary>
			/// Method for setting the user's current language from these preferences or from the system
			/// </summary>
			public static void SetCurrentLanguage(LocalizationConfiguration localizationConfiguration)
			{
				if (PlayerPrefs.HasKey(localizationConfiguration.LanguagePrefsKey))
				{
					//Get language from playerprefs
					CurrentLanguage = new ReactiveProperty<Language>((Language)PlayerPrefs.GetInt(localizationConfiguration.LanguagePrefsKey));
				}
				else
				{
					//Get system language
					CurrentLanguage = new ReactiveProperty<Language>(LanguageConverter.ConvertFromSystemLanguage(Application.systemLanguage));
					PlayerPrefs.SetInt(localizationConfiguration.LanguagePrefsKey, (int)CurrentLanguage.Value);
					PlayerPrefs.Save();
				}

				CurrentLanguage.Skip(1).Subscribe(value => PreviousLanguage = value);
			}
		}
	}
}
