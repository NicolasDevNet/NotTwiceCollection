using Assets.NotTwice.UP.Addressables.Runtime.Services;
using Assets.NotTwice.UP.Notifier.Runtime.Components;
using Assets.NotTwice.UP.Notifier.Runtime.ScriptableObjects;
using Assets.NotTwice.UP.ZenjectExtend.Runtime.Extentions;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Assets.NotTwice.UP.Notifier.Runtime.States
{
	public static partial class ApplicationState
	{
		public static class NotifierState
		{

			private static AsyncOperationHandle<NotifierConfiguration> _notifierConfigurationHandle;

			public static NotifierConfiguration GetNotifierConfiguration()
			{
				if (_notifierConfigurationHandle.IsDone)
				{
					return _notifierConfigurationHandle.Result;
				}

				return null;
			}

			public static async UniTask<NotifierConfiguration> GetNotifierConfigurationAsync(IAddressablesService addressablesService)
			{
				if(_notifierConfigurationHandle.IsValid())
				{
					return _notifierConfigurationHandle.Result;
				}

				var taskResult = await addressablesService.LoadAssetAsync<NotifierConfiguration>(Constants.Addressables.NotifierConfigurationLabel);

				_notifierConfigurationHandle = taskResult.operation;

				return _notifierConfigurationHandle.Result;
			}

			public static async UniTask AddNotificationPoolAsync(DiContainer container, IAddressablesService addressablesService)
			{
				await container.AddPoolAsync<Notification, NotificationPool, INotificationPool>(
				Constants.Addressables.NotificationPrefabAddress,
				addressablesService,
				GetNotifierConfiguration().MaxVisibleNotifications);
			}
		}
	}
}
