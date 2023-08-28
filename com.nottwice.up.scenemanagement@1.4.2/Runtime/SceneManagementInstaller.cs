using Assets.NotTwice.UP.SceneManagement.Runtime.Services;
using Zenject;

namespace Assets.NotTwice.UP.SceneManagement.Runtime
{
	public class SceneManagementInstaller : Installer<SceneManagementInstaller>
	{
		public override void InstallBindings()
		{
			Container.Bind<IScenesService>().To<ScenesService>().AsCached();
		}
	}
}
