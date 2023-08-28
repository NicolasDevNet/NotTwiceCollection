using Assets.NotTwice.UP.Mediator.Runtime.Enums;
using Assets.NotTwice.UP.Mediator.Runtime.Interfaces.Commands;
using Assets.NotTwice.UP.Mediator.Runtime.Interfaces.Messages;
using Assets.NotTwice.UP.Mediator.Runtime.Interfaces.Queries;
using Assets.NotTwice.UP.Mediator.Runtime.Interfaces;
using NotTwice.CA.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using Assets.NotTwice.UP.Mediator.Runtime.Extentions;
using Cysharp.Threading.Tasks;

namespace Assets.NotTwice.UP.Mediator.Runtime
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public sealed class Mediator : IMediator
	{
		private readonly DiContainer _container;
		private readonly ILogger _logger;

		#region Collections

		private Dictionary<Type, object> _commands;
		private Dictionary<Type, object> _asyncCommands;

		private Dictionary<Type, object> _queryHandlers;
		private Dictionary<Type, object> _asyncQueryHandlers;

		private Dictionary<Type, object> _messengers;
		private Dictionary<Type, object> _asyncMessengers;

		#endregion


		public Mediator(DiContainer diContainer, ILogger logger = null)
		{
			if (diContainer == null)
				throw new MediatorException(ErrorType.FailedToInitMediator, MediationType.None, new ArgumentNullException(nameof(diContainer)));

			_container = diContainer;
			_logger = logger;

			_commands = new Dictionary<Type, object>();
			_asyncCommands = new Dictionary<Type, object>();

			_queryHandlers = new Dictionary<Type, object>();
			_asyncQueryHandlers = new Dictionary<Type, object>();

			_messengers = new Dictionary<Type, object>();
			_asyncMessengers = new Dictionary<Type, object>();
		}

		#region Commands

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public bool TryExecute<T>()
			where T : class, ICommand<T>
		{
			T mediation = RetrieveMediation<T>(_commands, MediationType.Command);

			if (!mediation.CanExecute())
				return false;

			try
			{
				mediation.Execute();
				return true;
			}
			catch (Exception ex)
			{
				_logger?.LogMediatorErrorAsInformation(ErrorType.CommandFailed, MediationType.Command, ex);
				return false;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public bool TryUndo<T>()
			where T : class, ICommand<T>
		{
			T mediation = RetrieveMediation<T>(_commands, MediationType.Command);

			try
			{
				mediation.Undo();
				return true;
			}
			catch (Exception ex)
			{
				_logger?.LogMediatorErrorAsInformation(ErrorType.CommandFailed, MediationType.Command, ex);
				return false;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public async UniTask<bool> TryExecuteAsync<T>()
			where T : class, ICommandAsync<T>
		{
			T mediation = RetrieveMediation<T>(_asyncCommands, MediationType.CommandAsync);

			if (!mediation.CanExecute())
				return false;

			try
			{
				await mediation.ExecuteAsync(new CancellationToken());
				return true;
			}
			catch (Exception ex)
			{
				_logger?.LogMediatorErrorAsInformation(ErrorType.CommandFailed, MediationType.CommandAsync, ex);
				return false;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public async UniTask<bool> TryUndoAsync<T>()
			where T : class, ICommandAsync<T>
		{
			T mediation = RetrieveMediation<T>(_asyncCommands, MediationType.CommandAsync);

			try
			{
				await mediation.UndoAsync(new CancellationToken());
				return true;
			}
			catch (Exception ex)
			{
				_logger?.LogMediatorErrorAsInformation(ErrorType.CommandFailed, MediationType.CommandAsync, ex);
				return false;
			}
		}

		#endregion

		#region Query

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public bool TryInteract<TIn, TOut>(TIn @in, out TOut @out)
			where TIn : class, IQuery<TOut>
		{
			IQueryHandler<TIn, TOut> mediation = RetrieveMediation<IQueryHandler<TIn, TOut>>(_queryHandlers, MediationType.Query);

			@out = default(TOut);

			if (!mediation.CanInteract(@in))
				return false;

			try
			{
				@out = mediation.Interact(@in);
				return true;
			}
			catch (Exception ex)
			{
				_logger?.LogMediatorErrorAsInformation(ErrorType.QueryFailed, MediationType.Query, ex);
				return false;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public async UniTask<(bool success, TOut result)> TryInteractAsync<TIn, TOut>(TIn @in)
			where TIn : class, IQuery<TOut>
		{
			IQueryHandlerAsync<TIn, TOut> mediation = RetrieveMediation<IQueryHandlerAsync<TIn, TOut>>(_asyncQueryHandlers, MediationType.QueryAsync);

			if (!mediation.CanInteract(@in))
				return (false, default(TOut));

			try
			{
				return (true, await mediation.InteractAsync(@in, new CancellationToken()));
			}
			catch (Exception ex)
			{
				_logger?.LogMediatorErrorAsInformation(ErrorType.QueryFailed, MediationType.QueryAsync, ex);
				return (false, default(TOut));
			}
		}

		#endregion

		#region Messages

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public bool TrySend<T>(T @in)
			 where T : class, IMessage
		{
			IMessenger<T> mediation = RetrieveMediation<IMessenger<T>>(_messengers, MediationType.Messenger);

			if (!mediation.CanSend(@in))
				return false;

			try
			{
				mediation.Send(@in);
				return true;
			}
			catch (Exception ex)
			{
				_logger?.LogMediatorErrorAsInformation(ErrorType.MessageFailed, MediationType.Messenger, ex);
				return false;
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public async UniTask<bool> TrySendAsync<T>(T @in)
			where T : class, IMessage
		{
			IMessengerAsync<T> mediation = RetrieveMediation<IMessengerAsync<T>>(_asyncMessengers, MediationType.MessengerAsync);

			if (!mediation.CanSend(@in))
				return false;

			try
			{
				await mediation.SendAsync(@in, new CancellationToken());
				return true;
			}
			catch (Exception ex)
			{
				_logger?.LogMediatorErrorAsInformation(ErrorType.MessageFailed, MediationType.MessengerAsync, ex);
				return false;
			}
		}

		#endregion

		#region Private

		private T RetrieveMediation<T>(Dictionary<Type, object> mediationsDictionary, MediationType mediationType)
			where T : class
		{
			T mediation;

			if (mediationsDictionary.TryGetValue(typeof(T), out object foundResource)
				&& foundResource is T instance)
				mediation = instance;
			else
			{
				mediation = _container.TryResolve<T>();

				if(mediation == null)
					throw new MediatorException<T>(ErrorType.FailedToRetrieveInstance, mediationType);
				else
					mediationsDictionary.Add(typeof(T), mediation);
			}

			return mediation;
		}

		#endregion
	}
}
