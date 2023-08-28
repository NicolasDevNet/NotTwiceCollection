using Assets.NotTwice.UP.Notifier.Runtime.Services;
using Zenject;

namespace Assets.NotTwice.UP.Notifier.Runtime
{
	public class NotifierInstaller : Installer<NotifierInstaller>
	{
		public override void InstallBindings()
		{
			Container.Bind<INotifierService>().To<NotifierService>();
		}
	}
}
