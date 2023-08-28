using Assets.NotTwice.UP.SceneManagement.Runtime.ScriptableObjects;
using Assets.NotTwice.UP.SceneManagement.Runtime.Services;
using UnityEngine;
using Zenject;

namespace Assets.NotTwice.UP.SceneManagement.Runtime.Components
{
	[DisallowMultipleComponent]
	[AddComponentMenu("NotTwice/Scenes/" + nameof(SceneManager))]
	public class SceneManager : MonoBehaviour
	{
		private IScenesService _scenesService;

		private Animator _animator;

		[Inject]
		private void Initialize(IScenesService scenesService, [InjectOptional(Id = "TransitionAnimator")] Animator transitionAnimator)
		{
			_scenesService = scenesService;
			_animator = transitionAnimator;
		}

		private void Awake()
		{
			if (_animator != null)
			{
				_animator.Play(_scenesService.CurrentScene.EnterSceneState);
			}
		}

		public async void LoadSceneAsync(SceneConfiguration sceneConfiguration)
		{
			await _scenesService.LoadSceneAsync(sceneConfiguration);
		}
	}
}
