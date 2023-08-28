#if UNITY_EDITOR

using Assets.NotTwice.UP.SceneManagement.Runtime.Components;
using Assets.NotTwice.UP.SceneManagement.Runtime.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Assets.NotTwice.UP.SceneManagement.Editor
{
	public static class EditorOnly
	{
		private const string BASE_MENU_ITEM_PATH = "NotTwice/Scenes";

		[MenuItem(BASE_MENU_ITEM_PATH + "/" + nameof(Initialize))]
		public static void Initialize()
		{
			InitScenesFolders();
		}

		[MenuItem(BASE_MENU_ITEM_PATH + "/" + nameof(AddCurrentSceneConfiguration))]
		public static void AddCurrentSceneConfiguration()
		{
			var instance = SceneConfiguration.GetDefault();

			AssetDatabase.CreateAsset(instance, $"Assets/ScriptableObjects/Configurations/Scenes/{instance.SceneName}.asset");

			AssetDatabase.SaveAssets();
		}

		[MenuItem(BASE_MENU_ITEM_PATH + "/" + nameof(AddSceneManagerToScene))]
		public static void AddSceneManagerToScene()
		{
			var gameObject = new GameObject(nameof(SceneManager)).AddComponent<SceneManager>();

			if (Selection.activeGameObject != null)
				gameObject.transform.SetParent(Selection.activeGameObject.transform);
		}

		private static void InitScenesFolders()
		{
			if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects"))
				AssetDatabase.CreateFolder("Assets", "ScriptableObjects");

			if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects/Configurations"))
				AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Configurations");

			if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects/Configurations/Scenes"))
				AssetDatabase.CreateFolder("Assets/ScriptableObjects/Configurations", "Scenes");

			AssetDatabase.SaveAssets();
		}
	}
}

#endif