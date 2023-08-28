using Assets.NotTwice.UP.Notifier.Runtime.ScriptableObjects;

namespace Assets.NotTwice.UP.Notifier.Editor
{
	internal static class Constants
	{
		public static class Folders
		{
			public const string AssetsFolder = "Assets";
			public const string ScriptableObjectsFolder = "ScriptableObjects";
			public const string ScriptableObjectsPath = AssetsFolder + "/" + ScriptableObjectsFolder;

			public const string ConfigurationsFolder = "Configurations";
			public const string LocalizationFolder = "Localization";
			public const string ConfigurationsPath = ScriptableObjectsPath + "/" + ConfigurationsFolder;
			public const string LocalizationPath = ScriptableObjectsPath + "/" + LocalizationFolder;

			public const string SharedResourcesFolder = "Shared";
			public const string SharedResourcesPath = LocalizationPath + "/" + SharedResourcesFolder;
		}

		public static class Files
		{
			public const string ConfigurationAssetName = nameof(NotifierConfiguration);
			public const string AssetExtention = "asset";

			public const string ConfigurationAssetPath = Folders.ConfigurationsPath + "/" + ConfigurationAssetName + "." + AssetExtention;
		}
	}
}
