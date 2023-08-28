using Assets.NotTwice.UP.Localization.Runtime.Enums;

namespace Assets.NotTwice.UP.Localization.Runtime.Services
{
	/// <summary>
	/// Presenter to manage the translations of a scene from a translation repository
	/// </summary>
	public interface ILocalizationService
	{
		/// <summary>
		/// Method for saving user preferences
		/// </summary>
		/// <param name="value">Int value of enum <see cref="Language"/></param>
		void ChangePlayerPrefLanguage(int value);

		/// <summary>
		/// Method for finding the key of a translation by its value
		/// </summary>
		/// <param name="language">The language to be used</param>
		/// <param name="value">The value associated with a key</param>
		/// <returns>The key found or a default value</returns>
		string GetKeyFromValue(Language language, string value);

		/// <summary>
		/// Method for translating a text by looking up its key in the translation repository
		/// </summary>
		/// <param name="key">The translation key to provide</param>
		/// <returns>The value found or the default value</returns>
		string TranslateWithKey(string key);

		/// <summary>
		/// Method for translating a text by looking up its key in the translation repository
		/// </summary>
		/// <param name="key">The translation key to provide</param>
		/// <returns>The value found or the default value</returns>
		string TranslateWithKey(Language language, string key);
	}
}