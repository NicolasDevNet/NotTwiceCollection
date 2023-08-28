using Assets.NotTwice.UP.Notifier.Runtime.ScriptableObjects;
using UnityEditor;

namespace Assets.NotTwice.UP.Notifier.Editor
{
	public static class InitializeNotifier
	{
		[MenuItem("NotTwice/Notifier/" + nameof(InitializeNotifier))]
		public static void Execute()
		{
			InitializeNotifierFolders();

			InitializeNotifierConfiguration();
		}

		internal static void InitializeNotifierFolders()
		{
			if (!AssetDatabase.IsValidFolder(Constants.Folders.ScriptableObjectsPath))
				AssetDatabase.CreateFolder(Constants.Folders.AssetsFolder, Constants.Folders.ScriptableObjectsFolder);

			if (!AssetDatabase.IsValidFolder(Constants.Folders.ConfigurationsPath))
				AssetDatabase.CreateFolder(Constants.Folders.ScriptableObjectsPath, Constants.Folders.ConfigurationsFolder);

			AssetDatabase.SaveAssets();
		}

		internal static void InitializeNotifierConfiguration()
		{
			var configuration = AssetDatabase.LoadAssetAtPath(Constants.Files.ConfigurationAssetPath, typeof(NotifierConfiguration)) as NotifierConfiguration;

			if (configuration == null)
			{
				configuration = NotifierConfiguration.GetDefault();
				AssetDatabase.CreateAsset(configuration, Constants.Files.ConfigurationAssetPath);

				AssetDatabase.SaveAssets();
			}
		}
	}
}
