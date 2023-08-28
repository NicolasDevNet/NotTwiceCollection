using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Assets.NotTwice.UP.ADAccess.Runtime.Gateways
{
	/// <summary>
	/// This gateway allows interaction with the client location dedicated to the application in use
	/// <para>Processing exceptions are logged with Unity</para>
	/// </summary>
	/// <typeparam name="T">The T type of the data to be processed</typeparam>
	public interface IApplicationDataAccessGateway<T> where T : class
	{
		/// <summary>
		/// Method for creating an encrypted entry in the application location
		/// </summary>
		/// <param name="data">The data written to the entry</param>
		/// <param name="id">The input identifier in numeric format</param>
		/// <returns>The success of the operation</returns>
		UniTask<bool> CreateAsync(T data, int id);

		/// <summary>
		/// Method for deleting an entry at the application location
		/// </summary>
		/// <param name="id">The input identifier in numeric format</param>
		/// <returns>The success of the operation</returns>
		bool Delete(int id);

		/// <summary>
		/// Method for reading encrypted input at application location
		/// </summary>
		/// <param name="id">The input identifier in numeric format</param>
		/// <returns>The deserialized result of the data</returns>
		UniTask<T> ReadAsync(int id);

		/// <summary>
		/// Method for reading all entries at the application location
		/// </summary>
		/// <returns>Data iterations found</returns>
		IAsyncEnumerable<(T data, int id)> ReadAllAsync();

		/// <summary>
		/// Method for updating an entry at the application location
		/// </summary>
		/// <param name="data">New data</param>
		/// <param name="id">The input identifier in numeric format</param>
		/// <returns>The success of the operation</returns>
		UniTask<bool> UpdateAsync(T data, int id);
	}
}
