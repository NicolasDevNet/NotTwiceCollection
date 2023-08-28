using Assets.NotTwice.UP.SceneManagement.Runtime.ScriptableObjects;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using static Assets.NotTwice.UP.SceneManagement.Runtime.States.ApplicationState;

namespace Assets.NotTwice.UP.SceneManagement.Runtime.Services
{
	public class ScenesService : IScenesService
	{
		private IList<SceneConfiguration> _sceneConfigurations;

		private ZenjectSceneLoader _zenjectSceneLoader;

		private SceneConfiguration _currentScene;

		private Animator _animator;

		public SceneConfiguration CurrentScene { get => _currentScene; }

		public ScenesService(ZenjectSceneLoader zenjectSceneLoader, [InjectOptional(Id = "TransitionAnimator")] Animator transitionAnimator)
		{
			_sceneConfigurations = SceneManagementState.GetSceneConfigurations();
			_zenjectSceneLoader = zenjectSceneLoader;
			_currentScene = FindSceneByName(SceneManager.GetActiveScene().name);
			_animator = transitionAnimator;
		}

		public async UniTask LoadSceneAsync(SceneConfiguration sceneConfiguration)
		{
			if (sceneConfiguration == null)
				return;

			if (_animator != null)
			{
				_animator.Play(_currentScene.ExitSceneState);

				await UniTask.Delay(_currentScene.AnimationDuration);
			}

			_currentScene = sceneConfiguration;

			List<UniTask> dependenciesTasks = null;

			if (sceneConfiguration.Dependencies != null)
			{
				dependenciesTasks = new List<UniTask>();

				foreach (var dependency in sceneConfiguration.Dependencies)
					dependenciesTasks.Add(LoadSceneAsAsync(dependency.SceneName, LoadSceneMode.Additive));
			}

			if (dependenciesTasks != null)
			{
				await UniTask.WhenAll(dependenciesTasks)
					.ContinueWith(async () => await LoadSceneAsAsync(sceneConfiguration.SceneName, LoadSceneMode.Single));
			}
			else
			{
				await LoadSceneAsAsync(sceneConfiguration.SceneName, LoadSceneMode.Single);
			}
		}

		public async UniTask LoadSceneWithInjectionAsync<T>(SceneConfiguration sceneConfiguration, Dictionary<string, object> instancesToInject)
		{
			if (sceneConfiguration == null)
				return;

			if (instancesToInject?.Count == 0)
				throw new Exception("This method need not be called if there are no instances to inject.");

			if (_animator != null)
			{
				_animator.Play(_currentScene.ExitSceneState);

				await UniTask.Delay(_currentScene.AnimationDuration);
			}

			_currentScene = sceneConfiguration;

			List<UniTask> dependenciesTasks = null;

			if (sceneConfiguration.Dependencies != null)
			{
				dependenciesTasks = new List<UniTask>();

				foreach (var dependency in sceneConfiguration.Dependencies)
					dependenciesTasks.Add(LoadSceneAsAsync(dependency.SceneName, LoadSceneMode.Additive));
			}

			if (dependenciesTasks != null)
			{
				await UniTask.WhenAll(dependenciesTasks)
					.ContinueWith(async () => await LoadSceneWithInjectionAsync<T>(sceneConfiguration.SceneName, LoadSceneMode.Single, instancesToInject));
			}
			else
			{
				await LoadSceneWithInjectionAsync<T>(sceneConfiguration.SceneName, LoadSceneMode.Single, instancesToInject);
			}
		}

		public SceneConfiguration FindSceneByName(string sceneName)
		{
			return _sceneConfigurations.FirstOrDefault(p => p.SceneName == sceneName);
		}

		private UniTask LoadSceneAsAsync(string sceneName, LoadSceneMode loadSceneMode)
			=> SceneManager.LoadSceneAsync(sceneName, loadSceneMode).ToUniTask();

		private UniTask LoadSceneWithInjectionAsync<T>(string sceneName, LoadSceneMode loadSceneMode, Dictionary<string, object> instancesToInject)
		{
			return _zenjectSceneLoader.LoadSceneAsync(sceneName, loadSceneMode, (container) =>
			{
				foreach (var instanceToInject in instancesToInject)
				{
					container.BindInstance(instanceToInject.Value).WithId(instanceToInject.Key).WhenInjectedInto<T>();
				}
			}).ToUniTask();
		}
	}
}
