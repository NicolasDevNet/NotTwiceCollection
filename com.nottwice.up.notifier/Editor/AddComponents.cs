using Assets.NotTwice.UP.Notifier.Runtime.Components;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.NotTwice.UP.Notifier.Editor
{
	public static class AddComponents
	{
		[MenuItem("NotTwice/Notifier/" + nameof(AddNotification))]
		public static void AddNotification()
		{
			var parent = Selection.activeGameObject?.transform;

			var newGameObject = new GameObject(nameof(Notification));

			if (parent != null)
				newGameObject.transform.SetParent(parent);

			var backgroundObject = new GameObject("Background");
			var imageComponent = backgroundObject.AddComponent<Image>();

			backgroundObject.transform.SetParent(newGameObject.transform);

			var contentObject = new GameObject("Content");
			var textComponent = contentObject.AddComponent<TextMeshProUGUI>();

			contentObject.transform.SetParent(newGameObject.transform);

			var notification = newGameObject.AddComponent<Notification>();
			notification.Content = textComponent;
			notification.Background = imageComponent;

			newGameObject.transform.localPosition = Vector2.zero;
		}
	}
}
