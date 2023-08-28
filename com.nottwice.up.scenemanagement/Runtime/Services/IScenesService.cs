using Assets.NotTwice.UP.SceneManagement.Runtime.ScriptableObjects;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Assets.NotTwice.UP.SceneManagement.Runtime.Services
{
	public interface IScenesService
	{
		SceneConfiguration CurrentScene { get; }

		SceneConfiguration FindSceneByName(string sceneName);
		UniTask LoadSceneAsync(SceneConfiguration sceneConfiguration);
		UniTask LoadSceneWithInjectionAsync<T>(SceneConfiguration sceneConfiguration, Dictionary<string, object> instancesToInject);
	}
}