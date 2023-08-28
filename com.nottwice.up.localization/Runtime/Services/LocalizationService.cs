using Assets.NotTwice.UP.Localization.Runtime.Enums;
using Assets.NotTwice.UP.Localization.Runtime.ScriptableObjects;
using Assets.NotTwice.UP.PlayerPrefsAccess.Runtime.Wrappers;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using static Assets.NotTwice.UP.Localization.Runtime.States.ApplicationState;

namespace Assets.NotTwice.UP.Localization.Runtime.Services
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public class LocalizationService : ILocalizationService
	{
		private LocalizationConfiguration _configuration;
		private IList<LocalizationResource> _sharedResources;
		private IList<LocalizationResource> _sceneResources;

		private readonly IPlayerPrefsWrapper _playerPrefsWrapper;

		public LocalizationService(IList<LocalizationResource> sceneResources, IPlayerPrefsWrapper playerPrefsWrapper)
		{
			_playerPrefsWrapper = playerPrefsWrapper;
			_configuration = LocalizationState.GetLocalizationConfiguration();
			_sharedResources = LocalizationState.GetSharedResources();
			_sceneResources = sceneResources;
		}

		#region Public

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public string GetKeyFromValue(Language language, string value)
		{
			//First search the shared translation resources
			var translation = FindTranslationPairByValue(language, _sharedResources, value);

			if (translation != null)
				return translation.Value;

			//Then in those of the scene
			translation = FindTranslationPairByValue(language, _sceneResources, value);

			//If no value is found either, then the default value is returned
			return translation?.Key ?? Constants.Errors.MissingKeyForValue;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public string TranslateWithKey(Language language, string key)
		{
			//First search the shared translation resources
			var translation = FindTranslationPair(language, _sharedResources, key);

			if (translation != null)
				return translation.Value;

			//Then in those of the scene
			translation = FindTranslationPair(language, _sceneResources, key);

			//If no value is found either, then the default value is returned
			return translation?.Value ?? _configuration.MissingTranslationText;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public string TranslateWithKey(string key)
		{
			//First search the shared translation resources
			var translation = FindTranslationPair(_sharedResources, key);

			if (translation != null)
				return translation.Value;

			//Then in those of the scene
			translation = FindTranslationPair(_sceneResources, key);

			//If no value is found either, then the default value is returned
			return translation?.Value ?? _configuration.MissingTranslationText;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void ChangePlayerPrefLanguage(int value)
		{
			_playerPrefsWrapper.SetInt(_configuration.LanguagePrefsKey, value);
			_playerPrefsWrapper.Save();
		}

		#endregion

		#region Private

		private TranslationPair FindTranslationPair(Language language ,IList<LocalizationResource> resources, string key)
		{
			if (resources.Any(p => language.Equals(p.Language)))
			{
				return resources.First(p => language.Equals(p.Language)).TranslationPairs
				.Find(p => key.Equals(p.Key));
			}

			return null;
		}

		private TranslationPair FindTranslationPairByValue(Language language, IList<LocalizationResource> resources, string value)
		{
			if (resources.Any(p => language.Equals(p.Language)))
			{
				return resources.First(p => language.Equals(p.Language)).TranslationPairs
				.Find(p => value.Equals(p.Value));
			}

			return null;
		}

		private TranslationPair FindTranslationPair(IList<LocalizationResource> resources, string key)
		{
			return FindTranslationPair(LocalizationState.CurrentLanguage.Value, resources, key);
		}

		#endregion
	}
}
