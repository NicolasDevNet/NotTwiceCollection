#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.NotTwice.UP.Localization.Editor
{
	internal static class TranslationDataHelper
	{
		public static string GetPhysicalLocalizationFolder()
		{
			return Application.persistentDataPath + "/" + Constants.Folders.LocalizationFolder;
		}

		public static bool PhysicalLocalizationDirectoryExists()
		{
			return Directory.Exists(GetPhysicalLocalizationFolder());
		}

		public static IEnumerable<string> GetAllScenesName()
		{
			var sceneNumber = SceneManager.sceneCountInBuildSettings;
			for (int i = 0; i < sceneNumber; i++)
			{
				yield return Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
			}
		}
	}
}

#endif