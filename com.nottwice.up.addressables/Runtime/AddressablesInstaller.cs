using Assets.NotTwice.UP.Addressables.Runtime.Services;
using Zenject;

namespace Assets.NotTwice.UP.Addressables.Runtime
{
	public class AddressablesInstaller : Installer<AddressablesInstaller>
	{
		public override void InstallBindings()
		{
			Container.Bind<IAddressablesService>().To<AddressablesService>().AsCached();
		}
	}
}
