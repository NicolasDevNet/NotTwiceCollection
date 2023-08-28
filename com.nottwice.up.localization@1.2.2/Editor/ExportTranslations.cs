#if UNITY_EDITOR

using Assets.NotTwice.UP.Localization.Runtime.Enums;
using Assets.NotTwice.UP.Localization.Runtime.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.NotTwice.UP.Localization.Editor
{
	public static class ExportTranslations
	{
		[MenuItem("NotTwice/Localization/" + nameof(ExportTranslations))]
		public static void Execute()
		{
			var scenesName = TranslationDataHelper.GetAllScenesName().ToArray();

			var localizationFolderPath = TryCreateFolder();

			var configuration = InitializeLocalization.InitializeLocalizationConfiguration();

			var sharedResources = FindAssets<LocalizationResource>(Constants.Folders.SharedResourcesPath);

			var headers = CreateCsvHeaders(configuration.SupportedLanguages);

			//Create files
			//For shared translations
			CreateOrUpdateResourcesCsv(sharedResources, headers, configuration.SupportedLanguages.Count,
				BuildFilePath(localizationFolderPath, "Shared"));

			//For scenes
			foreach (var sceneName in scenesName)
			{
				var sceneFolder = Constants.Folders.LocalizationPath + "/" + sceneName + "/";

				if (AssetDatabase.IsValidFolder(sceneFolder))
				{
					var sceneResources = FindAssets<LocalizationResource>(sceneFolder);
					CreateOrUpdateResourcesCsv(sceneResources, headers, configuration.SupportedLanguages.Count,
						BuildFilePath(localizationFolderPath, sceneName));
				}
			}

			Debug.Log("Files created at: " + localizationFolderPath);
		}

		private static string TryCreateFolder()
		{
			var localizationFolderPath = TranslationDataHelper.GetPhysicalLocalizationFolder();

			if (!TranslationDataHelper.PhysicalLocalizationDirectoryExists())
				Directory.CreateDirectory(localizationFolderPath);

			return localizationFolderPath;
		}

		private static string CreateCsvHeaders(List<Language> supportedLanguages)
		{
			var headers = "Key;";

			var orderedList = supportedLanguages.OrderBy(p => p).ToList();

			for (var i = 0; i < orderedList.Count; i++)
			{
				var separator = orderedList.Count - 1 == i ? "" : ";";
				headers += $"{orderedList[i]}{separator}";
			}

			return headers;
		}

		private static void CreateOrUpdateResourcesCsv(IEnumerable<LocalizationResource> resources, string headers, int languageCount, string path)
		{
			var csv = headers + Environment.NewLine;

			Dictionary<string, List<(Language lang, string value)>> translations
				= FormatTranslations(resources, languageCount);

			csv += CreateCsvFromFormatedTranslations(translations);

			if(!File.Exists(path))
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
				{
					WriteToStream(fileStream, csv);
				}
			}
			else
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Write))
				{
					fileStream.SetLength(0);
					WriteToStream(fileStream, csv);
				}
			}
		}

		private static void WriteToStream(FileStream fileStream, string content)
		{
			using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
			{
				streamWriter.Write(content);
			}
		}

		private static string BuildFilePath(string localizationFolderPath, string targetName)
		{
			return localizationFolderPath + "/" + targetName + ".csv";
		}

		public static IEnumerable<T> FindAssets<T>(params string[] paths) where T : UnityEngine.Object
        {
			string[] assetGUIDs = AssetDatabase.FindAssets("t:" + typeof(T), paths);
			foreach (string guid in assetGUIDs)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(guid);
				yield return  AssetDatabase.LoadAssetAtPath<T>(assetPath);
			}
		}

		private static Dictionary<string, List<(Language lang, string value)>> FormatTranslations(IEnumerable<LocalizationResource> translationResources, int languageCount)
		{
			Dictionary<string, List<(Language lang, string value)>> translations = new Dictionary<string, List<(Language, string)>>();

			foreach (var translationResource in translationResources)
			{
				foreach (var translationPair in translationResource.TranslationPairs)
				{
					if (!translations.ContainsKey(translationPair.Key))
					{
						translations[translationPair.Key] = new List<(Language, string)>();
					}

					translations[translationPair.Key].Add((translationResource.Language, translationPair.Value));

					if (translations[translationPair.Key].Count == languageCount)
						translations[translationPair.Key] = translations[translationPair.Key].OrderBy(p => p).ToList();
				}
			}

			return translations;
		}

		private static string CreateCsvFromFormatedTranslations(Dictionary<string, List<(Language lang, string value)>> translations)
		{
			var csv = string.Empty;

			foreach (var translation in translations)
			{
				csv += translation.Key + ";";

				var i = 0;

				foreach (var languageTranslation in translation.Value)
				{
					var separator = translation.Value.Count - 1 == i ? "" : ";";
					csv += languageTranslation.value + separator;

					i++;
				}

				csv += Environment.NewLine;
			}

			return csv;
		}
	}
}

#endif