#if UNITY_EDITOR

using Assets.NotTwice.UP.Localization.Runtime.Components;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Assets.NotTwice.UP.Localization.Runtime.Enums;

namespace Assets.NotTwice.UP.Localization.Editor
{
	public static class AddComponents
	{
		[MenuItem("NotTwice/Localization/" + nameof(AddLocalizedText))]
		public static void AddLocalizedText()
			=> AddLocalizableGenericText<Text>();

		[MenuItem("NotTwice/Localization/" + nameof(AddLocalizedTmpText))]
		public static void AddLocalizedTmpText()
			=> AddLocalizableGenericText<TextMeshPro>();

		[MenuItem("NotTwice/Localization/" + nameof(AddLocalizedDropdown))]
		public static void AddLocalizedDropdown()
			=> AddLocalizableGenericDropdown<Dropdown>();

		[MenuItem("NotTwice/Localization/" + nameof(AddLocalizedTmpDropdown))]
		public static void AddLocalizedTmpDropdown()
			=> AddLocalizableGenericDropdown<TMP_Dropdown>();

		[MenuItem("NotTwice/Localization/" + nameof(AddLanguageDropdown))]
		public static void AddLanguageDropdown()
		{
			var gameObject = AddGenericLanguageDropdown<Dropdown>();
			var dropdown = gameObject.GetComponent<Dropdown>();

			var supportedLanguages = InitializeLocalization.GetSupportedLanguages(Language.English);

			gameObject.GetComponent<LocalizableDropdown>().TranslationKeys = supportedLanguages;

			dropdown.AddOptions(supportedLanguages);
		}

		[MenuItem("NotTwice/Localization/" + nameof(AddLanguageTmpDropdown))]
		public static void AddLanguageTmpDropdown()
		{
			var gameObject = AddGenericLanguageDropdown<TMP_Dropdown>();
			var dropdown = gameObject.GetComponent<TMP_Dropdown>();

			var supportedLanguages = InitializeLocalization.GetSupportedLanguages(Language.English);

			gameObject.GetComponent<LocalizableDropdown>().TranslationKeys = supportedLanguages;

			dropdown.AddOptions(supportedLanguages);
		}

		private static GameObject AddGenericLanguageDropdown<T>()
			where T : Component
		{
			var gameObject = AddLocalizableGenericDropdown<T>();

			gameObject.AddComponent<LanguageDropdown>();

			return gameObject;
		}

		private static GameObject AddLocalizableGenericDropdown<T>()
			where T : Component
		{
			var gameObject = CreateLocalizableGameobject("Dropdown");

			gameObject.AddComponent<T>();
			gameObject.AddComponent<LocalizableDropdown>();

			return gameObject;
		}

		private static void AddLocalizableGenericText<T>()
			where T : Component
		{
			var gameObject = CreateLocalizableGameobject("Text");

			gameObject.AddComponent<T>();
			gameObject.AddComponent<LocalizableText>();
		}

		private static GameObject CreateLocalizableGameobject(string componentName)
		{
			var parent = Selection.activeGameObject?.transform;

			var newGameObject = new GameObject($"Localizable_{componentName}");

			if (parent != null)
				newGameObject.transform.SetParent(parent);

			return newGameObject;
		}
	}
}

#endif