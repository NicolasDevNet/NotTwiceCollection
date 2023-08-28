using Assets.NotTwice.UP.Notifier.Runtime.Components;
using Assets.NotTwice.UP.Notifier.Runtime.ScriptableObjects;
using Cysharp.Text;
using System.Collections.Generic;
using UnityEngine;
using static Assets.NotTwice.UP.Notifier.Runtime.States.ApplicationState;
using System;
using DG.Tweening;
using System.Linq;

namespace Assets.NotTwice.UP.Notifier.Runtime.Services
{
	public class NotifierService : INotifierService
	{
		private readonly NotifierConfiguration _notifierConfiguration;
		private readonly INotificationPool _notificationPool;
		private readonly ILogger _logger;

		private List<(string StatusLabel, string Content)> _inQueue;

		public NotifierService(INotificationPool notificationPool, ILogger logger)
		{
			_notifierConfiguration = NotifierState.GetNotifierConfiguration();
			_notificationPool = notificationPool;
			_logger = logger;

			if (!_notifierConfiguration.HasPersistentNotifications)
				_inQueue = new List<(string StatusLabel, string Content)>();

			if (_notifierConfiguration.MaxVisibleNotifications <= 0)
			{
				throw new Exception(Constants.Errors.MinVisibleStatus);
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public void Notify(string statusLabel, string content)
		{
			var status = TryFindNotifierStatus(statusLabel);

			if (_notifierConfiguration.HasPersistentNotifications)
			{
				HandlePersistentNotifications(status, content);
			}
			else
			{
				HandleTemporarilyNotifications(status, content);
			}

		}

		/// <summary>
		/// Private method for managing the temporarily notification system
		/// </summary>
		/// <param name="status">The status to be applied to the notification</param>
		/// <param name="content">The textual content of the notification</param>
		private void HandleTemporarilyNotifications(NotifierStatus status, string content)
		{
			//If the maximum number of notifications is reached
			if (_notificationPool.ActiveItems.Count == _notifierConfiguration.MaxVisibleNotifications)
			{
				//Add notification in queue
				_inQueue.Add((status.Label, content));
				_logger.Log(LogType.Log, ZString.Format(Constants.Logs.StatusAddedInQueue, status.Label));
				return;
			}

			var sequence = _notificationPool.Spawn(status.Color, content, _notifierConfiguration.SpaceY)
				.ShowTemporarily(_notifierConfiguration.FadeInDuration, _notifierConfiguration.FadeOutDuration, _notifierConfiguration.VisibleDuration);

			sequence.onComplete += () =>
			{
				if (_inQueue.Count > 0)
				{
					var item = _inQueue.First();
					_inQueue.Remove(item);

					HandleTemporarilyNotifications(item.StatusLabel, item.Content);
				}
			};

			sequence.Play();
		}

		/// <summary>
		/// Private method for managing the temporarily notification system
		/// </summary>
		/// <param name="statusLabel">Status label used to search</param>
		/// <param name="content">The textual content of the notification</param>
		private void HandleTemporarilyNotifications(string statusLabel, string content)
		{
			HandleTemporarilyNotifications(TryFindNotifierStatus(statusLabel), content);
		}

		/// <summary>
		/// Private method for managing the permanent notification system
		/// </summary>
		/// <param name="status">The status to be applied to the notification</param>
		/// <param name="content">The textual content of the notification</param>
		private void HandlePersistentNotifications(NotifierStatus status, string content)
		{
			//If the maximum number of notifications is reached
			if (_notificationPool.ActiveItems.Count == _notifierConfiguration.MaxVisibleNotifications)
			{
				//Clear the first in the list, then bring up the desired notification
				var despawnSequence = _notificationPool.Despawn(_notificationPool.ActiveItems.First(), _notifierConfiguration.FadeOutDuration, _notifierConfiguration.SpaceY, _notifierConfiguration.MoveNotificationsDuration);
				despawnSequence.onComplete += () =>
				{
					_notificationPool.Spawn(status.Color, content, _notifierConfiguration.SpaceY)
					.Show(_notifierConfiguration.FadeInDuration)
					.Play();
				};
			}
			else
			{
				//Or just display the desired notification
				_notificationPool.Spawn(status.Color, content, _notifierConfiguration.SpaceY)
					.Show(_notifierConfiguration.FadeInDuration)
					.Play();
			}
		}

		/// <summary>
		/// Private method for attempting a notification status search
		/// </summary>
		/// <param name="statusLabel">Status label used to search</param>
		/// <returns>The status found if this is the case</returns>
		/// <exception cref="Exception">Exception thrown if status not found</exception>
		private NotifierStatus TryFindNotifierStatus(string statusLabel)
		{
			return _notifierConfiguration.NotifierStatus.Find(p => p.Label == statusLabel)
				?? throw new Exception(ZString.Format(Constants.Warnings.MissingLabelStatus, statusLabel));
		}
	}
}
