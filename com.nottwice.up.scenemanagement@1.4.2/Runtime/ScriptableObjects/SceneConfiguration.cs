using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Assets.NotTwice.UP.SceneManagement.Runtime.ScriptableObjects
{
	/// <summary>
	/// Configuration object for a scene
	/// </summary>
	[CreateAssetMenu(fileName = nameof(SceneConfiguration), menuName = "NotTwice/Scenes/" + nameof(SceneConfiguration))]
	public class SceneConfiguration : ScriptableObject
	{
		/// <summary>
		/// Scene name, it must correspond to the real name of the scene
		/// </summary>
		public string SceneName;

		/// <summary>
		/// List of scene dependencies that will be checked additively
		/// </summary>
		public List<SceneConfiguration> Dependencies;

		/// <summary>
		/// State name for exiting a scene
		/// </summary>
		public string ExitSceneState;

		/// <summary>
		/// State name for entering a scene
		/// </summary>
		public string EnterSceneState;

		/// <summary>
		/// Duration of animation after which loading of next scene begins in ms
		/// </summary>
		public int AnimationDuration;

		/// <summary>
		/// Method to retrieve a default instance of the localization configuration
		/// </summary>
		public static SceneConfiguration GetDefault()
		{
			var instance = CreateInstance<SceneConfiguration>();

			instance.SceneName = SceneManager.GetActiveScene().name;

			return instance;
		}
	}
}
