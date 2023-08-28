using Assets.NotTwice.UP.Localization.Runtime.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.NotTwice.UP.Localization.Runtime.ScriptableObjects
{
	/// <summary>
	/// Configuration object for the translation system
	/// </summary>
	[CreateAssetMenu(fileName = nameof(LocalizationConfiguration), menuName = "NotTwice/Localization/" + nameof(LocalizationConfiguration))]
	public class LocalizationConfiguration : ScriptableObject
	{
		/// <summary>
		/// Default text displayed when translation key does not exist
		/// </summary>
		[Tooltip("Default text displayed when translation key does not exist")]
		public string MissingTranslationText;

		/// <summary>
		/// Key used for user language preferences
		/// </summary>
		[Tooltip("Key used for user language preferences")]
		public string LanguagePrefsKey;

		/// <summary>
		/// List of languages supported by the translation system
		/// </summary>
		[Tooltip("List of languages supported by the translation system")]
		public List<Language> SupportedLanguages;

		/// <summary>
		/// Method to retrieve a default instance of the localization configuration
		/// </summary>
		public static LocalizationConfiguration GetDefault()
		{
			var instance = CreateInstance<LocalizationConfiguration>();

			instance.MissingTranslationText = "Missing translation";
			instance.LanguagePrefsKey = "Language";
			instance.SupportedLanguages = new List<Language>
			{
				Language.English,
				Language.French
			};

			return instance;
		}
	}
}
