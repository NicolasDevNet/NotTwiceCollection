using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.NotTwice.UP.Mediator.Runtime.Interfaces.Queries
{
	/// <summary>
	/// Interface contract to implement for executing async queries
	/// </summary>
	/// <typeparam name="TIn">Type of class designated as a message</typeparam>
	/// <typeparam name="TOut">Type expected in response to query execution</typeparam>
	public interface IQueryHandlerAsync<TIn, TOut> where TIn : class, IQuery<TOut>
	{
		/// <summary>
		/// Method to check whether an async query can be sent, and to send it if so
		/// </summary>
		/// <param name="in">Object with query class type</param>
		/// <returns>Response expected in response to query execution</returns>
		UniTask<TOut> InteractAsync(TIn @in, CancellationToken cancellationToken);

		/// <summary>
		/// Method to check whether a query can be sent
		/// </summary>
		/// <param name="in">Object with query class type</param>
		/// <returns>Operation status</returns>
		bool CanInteract(TIn @in);
	}
}
