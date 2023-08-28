namespace Assets.NotTwice.UP.Mediator.Runtime.Interfaces.Messages
{
	/// <summary>
	/// Interface contract to implement for sending messages
	/// </summary>
	/// <typeparam name="T">Type of class designated as a message</typeparam>
	public interface IMessenger<T> where T : class, IMessage
	{
		/// <summary>
		/// Method for checking whether a message can be sent and sending it if so
		/// </summary>
		/// <param name="message">The type of class to be sent</param>
		void Send(T message);

		/// <summary>
		/// Method to check if a message can be sent
		/// </summary>
		/// <param name="message">The type of class to be sent</param>
		/// <returns>The status of the operation</returns>
		bool CanSend(T message);
	}
}
