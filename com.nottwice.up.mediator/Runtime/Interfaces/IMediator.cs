using Assets.NotTwice.UP.Mediator.Runtime.Interfaces.Commands;
using Assets.NotTwice.UP.Mediator.Runtime.Interfaces.Messages;
using Assets.NotTwice.UP.Mediator.Runtime.Interfaces.Queries;
using Cysharp.Threading.Tasks;

namespace Assets.NotTwice.UP.Mediator.Runtime.Interfaces
{
	/// <summary>
	/// Interface implemented by the <see cref="Mediator"/> class to perform actions
	/// between different application layers.
	/// </summary>
	public interface IMediator
	{
		/// <summary>
		/// Method for checking whether a command can be executed and then executing it if it is.
		/// </summary>
		/// <typeparam name="T">Type of command class</typeparam>
		/// <returns>Operation status</returns>
		bool TryExecute<T>() where T : class, ICommand<T>;

		/// <summary>
		/// Method for checking whether an asynchronous command can be executed, and then executing it if it is.
		/// </summary>
		/// <typeparam name="T">Type of command class</typeparam>
		/// <returns>Operation status</returns>
		UniTask<bool> TryExecuteAsync<T>() where T : class, ICommandAsync<T>;

		/// <summary>
		/// Method for checking whether a request can be executed and then executing it if it is.
		/// </summary>
		/// <typeparam name="TIn">Query input class type</typeparam>
		/// <typeparam name="TOut">Type of output expected in response to sending this request</typeparam>
		/// <param name="in">Request input class</param>
		/// <param name="out">Object resulting from sending the request</param>
		/// <returns>Operation status</returns>
		bool TryInteract<TIn, TOut>(TIn @in, out TOut @out) where TIn : class, IQuery<TOut>;

		/// <summary>
		/// Method for checking whether an asynchronous request can be executed, and then executing it if it is.
		/// </summary>
		/// <typeparam name="TIn">Query input class type</typeparam>
		/// <typeparam name="TOut">Type of output expected in response to sending this request</typeparam>
		/// <param name="in">Request input class</param>
		/// <returns>Status of the operation and object resulting from sending the request</returns>
		UniTask<(bool success, TOut result)> TryInteractAsync<TIn, TOut>(TIn @in) where TIn : class, IQuery<TOut>;

		/// <summary>
		/// Method used to check whether a message can be sent and to send it if so.
		/// </summary>
		/// <typeparam name="T">Type of message input class</typeparam>
		/// <param name="in">Message input class</param>
		/// <returns>Operation status</returns>
		bool TrySend<T>(T @in) where T : class, IMessage;

		/// <summary>
		/// Method used to check whether an asynchronous message can be sent, and to send it if so.
		/// </summary>
		/// <typeparam name="T">Type of message input class</typeparam>
		/// <param name="in">Message input class</param>
		/// <returns>Operation status</returns>
		UniTask<bool> TrySendAsync<T>(T @in) where T : class, IMessage;

		/// <summary>
		/// Method for performing the reverse of a command execution.
		/// </summary>
		/// <typeparam name="T">Type of command input class</typeparam>
		/// <returns>Operation status</returns>
		bool TryUndo<T>() where T : class, ICommand<T>;

		/// <summary>
		/// Method for performing the reverse of an async command execution.
		/// </summary>
		/// <typeparam name="T">Type of command input class</typeparam>
		/// <returns>Operation status</returns>
		UniTask<bool> TryUndoAsync<T>() where T : class, ICommandAsync<T>;
	}
}
