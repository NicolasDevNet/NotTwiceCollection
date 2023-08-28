namespace Assets.NotTwice.UP.Notifier.Runtime
{
	public static class Constants
	{
		internal class Addressables
		{
			public const string NotifierConfigurationLabel = "Configuration/Notifier";
			public const string NotificationPrefabAddress = "Notification";
		}

		internal class Errors
		{
			public const string MinVisibleStatus = "At least one visible notification in capacity is required for the service to work.";
		}

		internal class Warnings
		{
			public const string MissingLabelStatus = "Status not found with label {0}";
		}

		internal class Logs
		{
			public const string StatusAddedInQueue = "Status {0} added in queue";
		}
	}
}
