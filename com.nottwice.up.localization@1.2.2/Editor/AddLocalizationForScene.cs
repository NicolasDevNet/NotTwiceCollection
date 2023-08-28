#if UNITY_EDITOR

using Assets.NotTwice.UP.Localization.Runtime.Enums;
using Assets.NotTwice.UP.Localization.Runtime.ScriptableObjects;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Assets.NotTwice.UP.Localization.Editor
{
	public static class AddLocalizationForScene
	{
		[MenuItem("NotTwice/Localization/" + nameof(AddLocalizationForScene))]
		public static void Execute()
		{
			InitializeLocalization.InitializeLocalizationFolders();

			var configuration = InitializeLocalization.InitializeLocalizationConfiguration();

			InitSceneResources(configuration);
		}

		private static void InitSceneResources(LocalizationConfiguration localizationConfiguration)
		{
			var sceneName = SceneManager.GetActiveScene().name;
			var basePath = $"{Constants.Folders.LocalizationPath}/{sceneName}";

			if (!AssetDatabase.IsValidFolder(basePath))
				AssetDatabase.CreateFolder(Constants.Folders.LocalizationPath, sceneName);

			AssetDatabase.SaveAssets();

			foreach (var supportedLanguage in localizationConfiguration.SupportedLanguages)
			{
				var path = basePath + "/" + LanguageConverter.ConvertToResourceName(supportedLanguage) + "." + Constants.Files.AssetExtention;

				if (!AssetDatabase.LoadAssetAtPath(path, typeof(LocalizationResource)))
				{
					AssetDatabase.CreateAsset(LocalizationResource.GetDefault(supportedLanguage), path);
				}
			}

			AssetDatabase.SaveAssets();
		}
	}
}

#endif