using Assets.NotTwice.UP.Mediator.Runtime.Interfaces.Commands;
using Assets.NotTwice.UP.Mediator.Runtime.Interfaces.Messages;
using Assets.NotTwice.UP.Mediator.Runtime.Interfaces.Queries;
using Assets.NotTwice.UP.Mediator.Runtime.Interfaces;
using System;
using System.Linq;
using Zenject;

namespace Assets.NotTwice.UP.Mediator.Runtime
{
	public class MediatorInstaller : Installer<MediatorInstaller>
	{
		public override void InstallBindings()
		{
			//Register Commands
			RegisterMediation(Container, typeof(ICommand<>));

			RegisterMediation(Container, typeof(ICommandAsync<>));

			//Register Messengers
			RegisterMediation(Container, typeof(IMessenger<>));

			RegisterMediation(Container, typeof(IMessengerAsync<>));

			//Register Query handlers
			RegisterMediation(Container, typeof(IQueryHandler<,>));

			RegisterMediation(Container, typeof(IQueryHandlerAsync<,>));

			//Register Mediator
			Container.Bind<IMediator>().To<Mediator>().AsCached();
		}

		private static void RegisterMediation(DiContainer container, Type mediationType)
		{
			var mediations = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(p => p.GetInterfaces().Any(x =>
			  x.IsGenericType && x.GetGenericTypeDefinition() == mediationType));

			if ((!mediations?.Any()) ?? true)
				return;

			//Register Mediations
			foreach (var mediation in mediations)
			{
				var interfaceType = mediation.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == mediationType);

				container.Bind(interfaceType).To(mediation).AsTransient();
			}
		}
	}
}
