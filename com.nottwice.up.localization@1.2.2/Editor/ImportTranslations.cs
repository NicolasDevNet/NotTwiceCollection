#if UNITY_EDITOR

using Assets.NotTwice.UP.Localization.Runtime.Enums;
using Assets.NotTwice.UP.Localization.Runtime.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.NotTwice.UP.Localization.Editor
{
	public static class ImportTranslations
	{
		[MenuItem("NotTwice/Localization/" + nameof(ImportTranslations))]
		public static void Execute()
		{
			if (!TranslationDataHelper.PhysicalLocalizationDirectoryExists())
			{
				Debug.Log("No directory found at: " + TranslationDataHelper.GetPhysicalLocalizationFolder());
				return;
			}

			var files = Directory.GetFiles(TranslationDataHelper.GetPhysicalLocalizationFolder());

			foreach(var file in files)
			{
				var fileName = Path.GetFileNameWithoutExtension(file);
				using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
				{
					using(StreamReader reader = new StreamReader(fileStream))
					{
						int lineCount = 0;

						List<LocalizationResource> resources = new List<LocalizationResource>();

						while (!reader.EndOfStream)
						{
							string line = reader.ReadLine();
							string[] values = line.Split(";");

							//Read headers
							if(lineCount == 0)
							{
								resources = GetResourcesFromFileValues(values);
							}
							else
							{
								FillTranslationPair(values, resources);
							}

							lineCount++;
						}

						DeleteAndCreateResources(Constants.Folders.LocalizationPath + "/" + fileName, resources);
					}
				}
			}
		}

		private static void FillTranslationPair(string[] values, List<LocalizationResource> resources)
		{
			var key = values[0];
			values = values.Skip(1).ToArray();

			for (var i = 0; i < resources.Count; i++)
			{
				resources[i].TranslationPairs.Add(new TranslationPair()
				{
					Key = key,
					Value = values[i]
				});
			}
		}

		private static List<LocalizationResource> GetResourcesFromFileValues(string[] values)
		{
			//Skip column key to get languages
			//Create resources associeted to language
			return values.Skip(1).Select(p => (Language)Enum.Parse(typeof(Language), p))
			.Select(p =>
			{
				var resourceInstance = ScriptableObject.CreateInstance<LocalizationResource>();
				resourceInstance.Language = p;
				resourceInstance.TranslationPairs = new List<TranslationPair>();
				return resourceInstance;
			}).ToList();
		}

		private static void DeleteAndCreateResources(string folderPath, List<LocalizationResource> resources)
		{
			DeleteAssets<LocalizationResource>(folderPath);

			AssetDatabase.SaveAssets();

			foreach (var resource in resources)
			{
				//Create a resource file as asset
				var path = folderPath + "/" + LanguageConverter.ConvertToResourceName(resource.Language) + "." + Constants.Files.AssetExtention;
				AssetDatabase.CreateAsset(resource, path);
				Debug.Log("Asset created at: " + path);
			}

			AssetDatabase.SaveAssets();
		}

		private static IEnumerable<bool> DeleteAssets<T>(params string[] paths) where T : UnityEngine.Object
		{
			string[] assetGUIDs = AssetDatabase.FindAssets("t:" + typeof(T), paths);
			foreach (string guid in assetGUIDs)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(guid);
				yield return AssetDatabase.DeleteAsset(assetPath);
			}
		}
	}
}

#endif