namespace Assets.NotTwice.UP.Mediator.Runtime.Interfaces.Commands
{
	/// <summary>
	/// Interface designating a structure as an executable command.
	/// This same command can be used using Mediator.
	/// </summary>
	/// <typeparam name="T">The type of command to fill in</typeparam>
	public interface ICommand<T> where T : class, ICommand<T>
	{
		/// <summary>
		/// Method for executing a command powered by the implementation
		/// </summary>
		void Execute();

		/// <summary>
		/// Method for checking whether the command can be executed.
		/// This method is automatically called by Mediator when the order is executed.
		/// </summary>
		/// <returns>Returns check status</returns>
		bool CanExecute();

		/// <summary>
		/// Method for performing the reverse operation of the implemented command.
		/// </summary>
		void Undo();
	}
}
