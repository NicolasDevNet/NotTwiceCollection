namespace Assets.NotTwice.UP.PlayerPrefsAccess.Runtime.Gateways
{
	/// <summary>
	/// This gateway acts as an overlay to interact with the Unity user preference system.
	/// <para>It mainly allows the use of complex objects</para>.
	/// </summary>
	public interface IPlayerPrefsAccessGateway
	{
		/// <summary>
		/// Method for retrieving a user preference in the form of a complex object
		/// </summary>
		/// <typeparam name="T">Expected output data type. The generic type is also used as a key to retrieve the value.</typeparam>
		/// <returns>Value found or null</returns>
		T Get<T>() where T : class;

		/// <summary>
		/// Method for retrieving a user preference in the form of a complex object
		/// </summary>
		/// <param name="key">Key used to retrieve the value.</param>
		/// <typeparam name="T">Type of data expected on output.</typeparam>
		/// <returns>Value found or null</returns>
		T Get<T>(string key) where T : class;

		/// <summary>
		/// Method for assigning a complex value to a key in user preferences
		/// </summary>
		/// <typeparam name="T">Type of data expected on input.</typeparam>
		/// <param name="key">Key used to set the value.</param>
		/// <param name="value">Value saved as preference</param>
		void Set<T>(string key, T value) where T : class;

		/// <summary>
		/// Method for assigning a complex value to a key in user preferences
		/// </summary>
		/// <typeparam name="T">Type of data expected on input. The generic type is also used as a key to set the value.</typeparam>
		/// <param name="value">Value saved as preference</param>
		void Set<T>(T value) where T : class;
	}
}
