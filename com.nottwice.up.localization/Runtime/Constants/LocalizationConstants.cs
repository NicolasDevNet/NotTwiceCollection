namespace Assets.NotTwice.UP.Localization.Runtime
{
	public static class Constants
	{
		internal static class Addressables
		{
			public const string SharedResourcesLabel = "SharedLocalization";
			public const string SceneResourcesLabel = "SceneLocalization/{0}";
			public const string LocalizationConfigurationLabel = "Configuration/Localization";
		}

		internal static class Errors
		{
			public const string LoadingResourceFailed = "An error occurred while loading the resource, please check the addressable resource settings.";
			public const string LoadingConfigurationFailed = "An error occurred while loading the configuration, please check the addressable resource settings.";
			public const string MissingDropdownComponent = "A drop-down component is missing to perform the translation.";
			public const string MissingTextComponent = "A text component is missing to perform the translation.";
			public const string MissingLanguageDropdownComponent = "A drop-down component is missing to change language.";
			public const string MissingKeyForValue = "There is no existing key for this value.";
			public const string WrongLanguageTextValue = "The text value in the drop-down list cannot be read.";
		}

		public static class Resources
		{
			public const string FrResourceName = "FR";
			public const string EnResourceName = "EN";
			public const string SpResourceName = "SP";
			public const string ChResourceName = "CH";
		}

		public static class Displays
		{
			public const string FrFrDisplayName = "Français";
			public const string FrEnDisplayName = "Anglais";
			public const string FrSpDisplayName = "Espagnol";
			public const string FrChDisplayName = "Chinois";

			public const string SpFrDisplayName = "Francés";
			public const string SpEnDisplayName = "Inglés";
			public const string SpSpDisplayName = "Español";
			public const string SpChDisplayName = "Chino";

			public const string ChFrDisplayName = "法语";
			public const string ChEnDisplayName = "英语";
			public const string ChSpDisplayName = "西班牙语";
			public const string ChChDisplayName = "中国人";
		}
	}
}
