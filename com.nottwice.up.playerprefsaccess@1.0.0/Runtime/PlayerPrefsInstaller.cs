using Assets.NotTwice.UP.PlayerPrefsAccess.Runtime.Gateways;
using Assets.NotTwice.UP.PlayerPrefsAccess.Runtime.Wrappers;
using Zenject;

namespace Assets.NotTwice.UP.PlayerPrefsAccess.Runtime
{
	internal class PlayerPrefsInstaller : Installer<PlayerPrefsInstaller>
	{
		public override void InstallBindings()
		{
			Container.Bind<IPlayerPrefsWrapper>().To<PlayerPrefsWrapper>().AsTransient();

			Container.Bind<IPlayerPrefsAccessGateway>().To<PlayerPrefsAccessGateway>().AsTransient();
		}
	}
}
