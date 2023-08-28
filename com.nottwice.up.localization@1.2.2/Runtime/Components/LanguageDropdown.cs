using Assets.NotTwice.UP.Localization.Runtime.Enums;
using Assets.NotTwice.UP.Localization.Runtime.Exceptions;
using Assets.NotTwice.UP.Localization.Runtime.Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using System;
using static Assets.NotTwice.UP.Localization.Runtime.States.ApplicationState;

namespace Assets.NotTwice.UP.Localization.Runtime.Components
{
	/// <summary>
	/// Unity component to change the language used by the application
	/// </summary>    
	[DisallowMultipleComponent]
	[AddComponentMenu("NotTwice/Localization/" + nameof(LanguageDropdown))]
	public class LanguageDropdown : MonoBehaviour
	{
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

			if (!_hasTmpDropdown && !_hasDropdown)
				throw new LocalizationException(Constants.Errors.MissingLanguageDropdownComponent, ResourceType.Component, SceneManager.GetActiveScene().name);
		}

		private void Start()
		{
			InitComponents();

			BindComponent();
		}

		private void InitComponents()
		{
			if (_hasTmpDropdown)
			{
				InitTmpDropdown();
			}
			else if (_hasDropdown)
			{
				InitDropdown();
			}
		}

		private void InitDropdown()
		{
			for(var i = 0; i < _internalDropdown.options.Count; i++)
			{
				if (_internalDropdown.options[i].text == _localizationService.TranslateWithKey(LocalizationState.CurrentLanguage.Value.ToString()))
				{
					_internalDropdown.value = i;
					_internalDropdown.RefreshShownValue();
					break;
				}
			}
		}

		private void InitTmpDropdown()
		{
			for (var i = 0; i < _internalTmpDropdown.options.Count; i++)
			{
				if (_internalTmpDropdown.options[i].text == _localizationService.TranslateWithKey(LocalizationState.CurrentLanguage.Value.ToString()))
				{
					_internalTmpDropdown.value = i;
					_internalTmpDropdown.RefreshShownValue();
					break;
				}
			}
		}

		private void BindComponent()
		{
			Language choosenLanguage = Language.None;

			if (_hasTmpDropdown)
			{
				_internalTmpDropdown.onValueChanged.AddListener((value) =>
				{
					var optionText = _internalTmpDropdown.options[value].text;
					if (!Enum.TryParse(_localizationService.GetKeyFromValue(LocalizationState.CurrentLanguage.Value, optionText), out choosenLanguage))
					{
						throw new LocalizationException(Constants.Errors.WrongLanguageTextValue, ResourceType.Component, SceneManager.GetActiveScene().name);
					}
				});
			}
			else if (_hasDropdown)
			{
				_internalDropdown.onValueChanged.AddListener((value) =>
				{
					var optionText = _internalDropdown.options[value].text;
					if (!Enum.TryParse(_localizationService.GetKeyFromValue(LocalizationState.CurrentLanguage.Value, optionText), out choosenLanguage))
					{
						throw new LocalizationException(Constants.Errors.WrongLanguageTextValue, ResourceType.Component, SceneManager.GetActiveScene().name);
					}
				});
			}

			LocalizationState.CurrentLanguage.Value = choosenLanguage;
		}
	}
}
