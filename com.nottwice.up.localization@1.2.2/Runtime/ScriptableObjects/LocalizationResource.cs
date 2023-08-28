using Assets.NotTwice.UP.Localization.Runtime.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.NotTwice.UP.Localization.Runtime.ScriptableObjects
{
	/// <summary>
	/// Translation resource that contains a list of translations and an associated language
	/// </summary>
	[CreateAssetMenu(fileName = nameof(LocalizationResource), menuName = "NotTwice/Localization/" + nameof(LocalizationResource))]
	public class LocalizationResource : ScriptableObject
	{
		/// <summary>
		/// Language associated with this resource file
		/// </summary>
		[Tooltip("Language associated with this resource file")]
		public Language Language;

		/// <summary>
		/// List of translations for this resource file
		/// </summary>
		[Tooltip("List of translations for this resource file")]
		public List<TranslationPair> TranslationPairs;

		/// <summary>
		/// Method to create a default instance of the translation resource
		/// </summary>
		public static LocalizationResource GetDefault(Language language)
		{
			var instance = CreateInstance<LocalizationResource>();

			instance.Language = language;

			return instance;
		}
	}

	/// <summary>
	/// Translation pair to associate a key with a value
	/// </summary>
	[Serializable]
	public class TranslationPair
	{
		/// <summary>
		/// Translation key
		/// </summary>
		[Tooltip("Translation key")]
		public string Key;

		/// <summary>
		/// Value of the translation
		/// </summary>
		[Tooltip("Value of the translation")]
		public string Value;
	}
}
