using Assets.NotTwice.UP.Addressables.Runtime.Services;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Assets.NotTwice.UP.ZenjectExtend.Runtime.Extentions
{
	public static class DIContainerExtentions
	{
		private static Dictionary<string, AsyncOperationHandle> _operationsHandle = new Dictionary<string, AsyncOperationHandle>();

		public static async UniTask AddPoolAsync<TObject, TPoolConcrete, TPoolContract>(this DiContainer container, string address, IAddressablesService addressablesService, int poolSize)
			where TPoolConcrete : TPoolContract, IMemoryPool
			where TPoolContract : IMemoryPool
			where TObject : UnityEngine.Object
		{

			if (_operationsHandle.ContainsKey(address))
			{
				if (_operationsHandle[address].IsValid())
				{
					container.AddPool<TObject, TPoolConcrete, TPoolContract>(_operationsHandle[address].Result as TObject, poolSize);
				}
				else
				{
					_operationsHandle.Remove(address);
					await AddPoolFromAddressablesAsync<TObject, TPoolConcrete, TPoolContract>(container, address, addressablesService, poolSize);
				}				
			}
			else
			{
				await AddPoolFromAddressablesAsync<TObject, TPoolConcrete, TPoolContract>(container, address, addressablesService, poolSize);
			}
		}

		public static DiContainer AddPool<TObject, TPoolConcrete, TPoolContract>(this DiContainer container, TObject prefab, int poolSize)
			where TPoolConcrete : TPoolContract, IMemoryPool
			where TPoolContract : IMemoryPool
			where TObject : UnityEngine.Object
		{
			container.BindMemoryPoolCustomInterface<TObject, TPoolConcrete, TPoolContract>()
						.WithInitialSize(poolSize)
						.FromComponentInNewPrefab(prefab)
						.UnderTransformGroup($"{typeof(TObject).Name}sPool");

			return container;
		}

		public static void ReleasePrefab(string key, AddressablesService addressablesService)
		{
			addressablesService.Release(_operationsHandle[key]);
		}

		private static async UniTask AddPoolFromAddressablesAsync<TObject, TPoolConcrete, TPoolContract>(DiContainer container, string address, IAddressablesService addressablesService, int poolSize)
			where TPoolConcrete : TPoolContract, IMemoryPool
			where TPoolContract : IMemoryPool
			where TObject : UnityEngine.Object
		{
			await addressablesService.LoadAssetAsync<TObject>(address)
			.ContinueWith((taskResult) =>
			{
				container.AddPool<TObject, TPoolConcrete, TPoolContract>(taskResult.result, poolSize);
				_operationsHandle.Add(address, taskResult.operation);
			});
		}
	}
}
