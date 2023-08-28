using UnityEngine;

namespace Assets.NotTwice.UP.Localization.Runtime.Enums
{
	/// <summary>
	/// Repository of languages supported by the application
	/// </summary>
	public enum Language
	{
		English,
		French,
		Spanish,
		Chinese,
		None
	}

	/// <summary>
	/// Enum converter to convert SystemLanguage to Language
	/// </summary>
	public static class LanguageConverter
	{
		public static Language ConvertFromSystemLanguage(SystemLanguage systemLanguage)
			=> systemLanguage switch
			{
				SystemLanguage.French => Language.French,
				SystemLanguage.English => Language.English,
				SystemLanguage.Chinese or SystemLanguage.ChineseSimplified or SystemLanguage.ChineseTraditional => Language.Chinese,
				SystemLanguage.Spanish => Language.Spanish,
				_ => Language.English
			};

		public static string ConvertToResourceName(Language language)
			=> language switch
			{
				Language.French => Constants.Resources.FrResourceName,
				Language.English => Constants.Resources.EnResourceName,
				Language.Chinese => Constants.Resources.ChResourceName,
				Language.Spanish => Constants.Resources.SpResourceName,
				_ => Constants.Resources.EnResourceName
			};

		public static string ConvertToDisplayName(Language targetLanguage, Language language)
			=> targetLanguage switch
			{
				Language.French => language switch
				{
					Language.French => Constants.Displays.FrFrDisplayName,
					Language.English => Constants.Displays.FrEnDisplayName,
					Language.Chinese => Constants.Displays.FrChDisplayName,
					Language.Spanish => Constants.Displays.FrSpDisplayName,
					_ => Language.English.ToString()
				},
				Language.English => language.ToString(),
				Language.Chinese => language switch
				{
					Language.French => Constants.Displays.ChFrDisplayName,
					Language.English => Constants.Displays.ChEnDisplayName,
					Language.Chinese => Constants.Displays.ChChDisplayName,
					Language.Spanish => Constants.Displays.ChSpDisplayName,
					_ => Language.English.ToString()
				},
				Language.Spanish => language switch
				{
					Language.French => Constants.Displays.SpFrDisplayName,
					Language.English => Constants.Displays.SpEnDisplayName,
					Language.Chinese => Constants.Displays.SpChDisplayName,
					Language.Spanish => Constants.Displays.SpSpDisplayName,
					_ => Language.English.ToString()
				},
				_ => Language.English.ToString()
			};
	}
}
