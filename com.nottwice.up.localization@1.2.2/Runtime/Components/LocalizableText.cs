using Assets.NotTwice.UP.Localization.Runtime.Services;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using Assets.NotTwice.UP.Localization.Runtime.Exceptions;
using Assets.NotTwice.UP.Localization.Runtime.Enums;
using UnityEngine.SceneManagement;
using static Assets.NotTwice.UP.Localization.Runtime.States.ApplicationState;

namespace Assets.NotTwice.UP.Localization.Runtime.Components
{
	/// <summary>
	/// Unity component to associate a translation with a text component
	/// </summary>
	[DisallowMultipleComponent]
	[AddComponentMenu("NotTwice/Localization/" + nameof(LocalizableText))]
	public class LocalizableText : MonoBehaviour
	{
		/// <summary>
		/// Translation key associated with the component
		/// </summary>
		[Tooltip("Translation key linked to a key in the translation files.")]
		public string TranslationKey;

		private ILocalizationService _localizationService;

		private TMP_Text _internalTmpText;
		private Text _internalText;

		private bool _hasTmpText;
		private bool _hasText;

		[Inject]
		private void Initialize(ILocalizationService localizationService)
		{
			_localizationService = localizationService;
			_hasTmpText = TryGetComponent(out _internalTmpText);
			_hasText = TryGetComponent(out _internalText);

			if(!_hasText && !_hasTmpText)
				throw new LocalizationException(Constants.Errors.MissingTextComponent, ResourceType.Component, SceneManager.GetActiveScene().name);

			LocalizationState.CurrentLanguage.Subscribe(_ => Translate());
		}

		private void Start()
		{
			Translate();
		}

		/// <summary>
		/// Method to translate the text component according to its type
		/// </summary>
		/// <exception cref="Exception">Exception thrown if no component is found</exception>
		private void Translate()
		{
			if (_hasTmpText)
				_internalTmpText.text = _localizationService.TranslateWithKey(TranslationKey);
			else if (_hasText)
				_internalText.text = _localizationService.TranslateWithKey(TranslationKey);
		}
	}
}
