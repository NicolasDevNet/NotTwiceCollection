using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.NotTwice.UP.Notifier.Runtime.ScriptableObjects
{
	/// <summary>
	/// Configuration object for the notifier system
	/// </summary>
	[CreateAssetMenu(fileName = nameof(NotifierConfiguration), menuName = "NotTwice/Notifier/" + nameof(NotifierConfiguration))]
	public class NotifierConfiguration : ScriptableObject
	{
		[Tooltip("Duration for which a notification is visible")]
		public float VisibleDuration;

		[Tooltip("Time during which a notification becomes visible")]
		public float FadeInDuration;

		[Tooltip("Time during which a notification becomes invisible")]
		public float FadeOutDuration;

		[Tooltip("Animation time to move notifications after the one at the beginning of the queue has been deleted")]
		public float MoveNotificationsDuration;

		[Tooltip("Y-axis space between each notification")]
		public float SpaceY;

		[Tooltip("The maximum number of visible notifications.")]
		public int MaxVisibleNotifications;

		[Tooltip("Indicates whether notifications are displayed permanently until the maximum visible number is reached")]
		public bool HasPersistentNotifications;

		[Tooltip("Collection of different status customizations")]
		public List<NotifierStatus> NotifierStatus;

		/// <summary>
		/// Method to retrieve a default instance of the notifier configuration
		/// </summary>
		public static NotifierConfiguration GetDefault()
		{
			var instance = CreateInstance<NotifierConfiguration>();

			instance.FadeInDuration = 0.3f;
			instance.FadeOutDuration = 0.3f;
			instance.VisibleDuration = 3f;
			instance.MaxVisibleNotifications = 3;
			instance.SpaceY = 15;
			instance.MoveNotificationsDuration = 0.3f;
			instance.HasPersistentNotifications = false;

			instance.NotifierStatus = new List<NotifierStatus>()
			{
				new NotifierStatus()
				{
					Color = Color.red,
					Label = "Error"
				},
				new NotifierStatus()
				{
					Color = Color.green,
					Label = "Success"
				},
				new NotifierStatus()
				{
					Color = Color.grey,
					Label = "Informations"
				},
			};

			return instance;
		}
	}

	/// <summary>
	/// Serializable class for creating specific status notifications
	/// </summary>
	[Serializable]
	public class NotifierStatus
	{
		[Tooltip("Status label")]
		public string Label;

		[Tooltip("Status color")]
		public Color Color;
    }
}
