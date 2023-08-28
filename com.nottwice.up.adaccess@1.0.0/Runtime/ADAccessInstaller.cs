using Assets.NotTwice.UP.ADAccess.Runtime.Gateways;
using Assets.NotTwice.UP.ADAccess.Runtime.Wrappers;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.NotTwice.UP.ADAccess.Runtime
{
	public class ADAccessInstaller : Installer<ADAccessInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInstance(Application.dataPath).WithId("applicationDataPath");

			Container.Bind<IFileAccessWrapper>().To<FileAccessWrapper>().AsTransient();

			//For the application data access part,
			//search all classes using ApplicationDataAccessGateway for injection
			var applicationDataAccessGateways = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t => t.IsClass && !t.IsAbstract && (t.IsSubclassOf(typeof(ADAccessGateway<>)) || t.IsSubclassOf(typeof(CryptoADAccessGateway<>))));

			if ((!applicationDataAccessGateways?.Any()) ?? true)
				return;

			foreach (var gateway in applicationDataAccessGateways)
			{
				var interfaceType = gateway.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IApplicationDataAccessGateway<>));
				Container.Bind(interfaceType).To(gateway).AsCached();
			}
		}
	}
}
