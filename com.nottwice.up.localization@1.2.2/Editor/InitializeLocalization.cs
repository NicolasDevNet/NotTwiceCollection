#if UNITY_EDITOR

using Assets.NotTwice.UP.Localization.Runtime.Enums;
using Assets.NotTwice.UP.Localization.Runtime.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Assets.NotTwice.UP.Localization.Editor
{
	public static class InitializeLocalization
	{
		[MenuItem("NotTwice/Localization/" + nameof(InitializeLocalization))]
		public static void Execute()
		{
			InitializeLocalizationFolders();

			var configuration = InitializeLocalizationConfiguration();

			InitializeSharedResources(configuration);
		}

		internal static void InitializeLocalizationFolders()
		{
			if (!AssetDatabase.IsValidFolder(Constants.Folders.ScriptableObjectsPath))
				AssetDatabase.CreateFolder(Constants.Folders.AssetsFolder, Constants.Folders.ScriptableObjectsFolder);

			if (!AssetDatabase.IsValidFolder(Constants.Folders.ConfigurationsPath))
				AssetDatabase.CreateFolder(Constants.Folders.ScriptableObjectsPath, Constants.Folders.ConfigurationsFolder);

			if (!AssetDatabase.IsValidFolder(Constants.Folders.LocalizationPath))
				AssetDatabase.CreateFolder(Constants.Folders.ScriptableObjectsPath, Constants.Folders.LocalizationFolder);

			if (!AssetDatabase.IsValidFolder(Constants.Folders.SharedResourcesPath))
				AssetDatabase.CreateFolder(Constants.Folders.LocalizationPath, Constants.Folders.SharedResourcesFolder);

			AssetDatabase.SaveAssets();
		}

		internal static LocalizationConfiguration InitializeLocalizationConfiguration()
		{
			var configuration = AssetDatabase.LoadAssetAtPath(Constants.Files.ConfigurationAssetPath, typeof(LocalizationConfiguration)) as LocalizationConfiguration;

			if (configuration == null)
			{
				configuration = LocalizationConfiguration.GetDefault();
				AssetDatabase.CreateAsset(configuration, Constants.Files.ConfigurationAssetPath);

				AssetDatabase.SaveAssets();
			}

			return configuration;
		}

		internal static List<string> GetSupportedLanguages(Language targetLanguage)
		{
			var configuration = InitializeLocalizationConfiguration();
			return configuration.SupportedLanguages.OrderBy(x => x).ToList()
				.Select(p => LanguageConverter.ConvertToDisplayName(targetLanguage, p)).ToList();
		}

		internal static void InitializeSharedResources(LocalizationConfiguration localizationConfiguration)
		{
			var keysSupportedLanguages = GetSupportedLanguages(Language.English);

			foreach (var supportedLanguage in localizationConfiguration.SupportedLanguages)
			{
				var path = Constants.Folders.SharedResourcesPath + "/" + LanguageConverter.ConvertToResourceName(supportedLanguage) + "." + Constants.Files.AssetExtention;

				if (!AssetDatabase.LoadAssetAtPath(path, typeof(LocalizationResource)))
				{
					CreateDefaultLocalizationResource(supportedLanguage, path, keysSupportedLanguages);
				}
			}

			AssetDatabase.SaveAssets();
		}

		internal static void CreateDefaultLocalizationResource(Language supportedLanguage, string path, List<string> keysSupportedLanguages)
		{
			var instance = LocalizationResource.GetDefault(supportedLanguage);

			var valuesSupportedLanguages = GetSupportedLanguages(instance.Language);

			var translationPairs = new List<TranslationPair>();

			for (var i = 0; i < keysSupportedLanguages.Count; i++)
			{
				translationPairs.Add(new TranslationPair()
				{
					Key = keysSupportedLanguages[i],
					Value = valuesSupportedLanguages[i],
				});
			}

			instance.TranslationPairs = translationPairs;

			AssetDatabase.CreateAsset(instance, path);
		}
	}
}

#endif