using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Assets.NotTwice.UP.Localization.Runtime.Services;
using UniRx;
using System.Linq;
using Assets.NotTwice.UP.Localization.Runtime.Exceptions;
using Assets.NotTwice.UP.Localization.Runtime.Enums;
using UnityEngine.SceneManagement;
using static Assets.NotTwice.UP.Localization.Runtime.States.ApplicationState;

namespace Assets.NotTwice.UP.Localization.Runtime.Components
{
	/// <summary>
	/// Unity component to associate a list of translations with a drop-down list
	/// </summary>    
	[DisallowMultipleComponent]
	[AddComponentMenu("NotTwice/Localization/" + nameof(LocalizableDropdown))]
	public class LocalizableDropdown : MonoBehaviour
	{
		/// <summary>
		/// List of translation keys for a drop-down list
		/// </summary>
		[Tooltip("Translation keys linked to several keys in the translation files. The order must correspond to the drop-down list")]
		public List<string> TranslationKeys;

		private ILocalizationService _localizationService;

		private TMP_Dropdown _internalTmpDropdown;
		private Dropdown _internalDropdown;

		private bool _hasTmpDropdown;
		private bool _hasDropdown;

		[Inject]
		private void Initialize(ILocalizationService localizationService)
		{
			_localizationService = localizationService;
			_hasTmpDropdown = TryGetComponent(out _internalTmpDropdown);
			_hasDropdown = TryGetComponent(out _internalDropdown);

			if(!_hasDropdown && !_hasTmpDropdown)
				throw new LocalizationException(Constants.Errors.MissingDropdownComponent, ResourceType.Component, SceneManager.GetActiveScene().name);

			LocalizationState.CurrentLanguage.Subscribe(_ => Translate());
		}

		private void Start()
		{
			Translate();
		}

		/// <summary>
		/// Method to translate the dropdown component according to its type
		/// </summary>
		/// <exception cref="Exception">Exception thrown if no component is found</exception>
		private void Translate()
		{
			if (_hasTmpDropdown)
				TranslateForTmpDropdown();
			else if (_hasDropdown)
				TranslateDropdown();
		}

		/// <summary>
		/// Method to manage the translation of a TMP drop-down list
		/// </summary>
		private void TranslateForTmpDropdown()
		{
			for (var i = 0; i < TranslationKeys.Count; i++)
			{
				var option = _internalTmpDropdown.options.ElementAtOrDefault(i);
				if (option != null)
					option.text = _localizationService.TranslateWithKey(TranslationKeys[i]);
			}

			_internalTmpDropdown.RefreshShownValue();
		}

		/// <summary>
		/// Method to manage the translation of a drop-down list
		/// </summary>
		private void TranslateDropdown()
		{
			for (var i = 0; i < TranslationKeys.Count; i++)
			{
				var option = _internalDropdown.options.ElementAtOrDefault(i);
				if (option != null)
					option.text = _localizationService.TranslateWithKey(TranslationKeys[i]);
			}

			_internalDropdown.RefreshShownValue();
		}
	}
}
