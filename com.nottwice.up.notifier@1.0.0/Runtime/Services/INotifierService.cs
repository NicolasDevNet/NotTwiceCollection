using Assets.NotTwice.UP.Notifier.Runtime.ScriptableObjects;

namespace Assets.NotTwice.UP.Notifier.Runtime.Services
{
	public interface INotifierService
	{
		/// <summary>
		/// Method for displaying a notification permanently or temporarily depending on configuration <see cref="NotifierConfiguration"/>.
		/// </summary>
		/// <param name="statusLabel">Status label used to search</param>
		/// <param name="content">The textual content of the notification</param>
		void Notify(string statusLabel, string content);
	}
}