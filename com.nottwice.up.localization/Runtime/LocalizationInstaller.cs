using Zenject;
using Assets.NotTwice.UP.Localization.Runtime.ScriptableObjects;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using Assets.NotTwice.UP.Localization.Runtime.Services;

namespace Assets.NotTwice.UP.Localization.Runtime
{
	public class LocalizationInstaller : Installer<LocalizationInstaller>
	{

		private static AsyncOperationHandle<IList<LocalizationResource>> _sceneResourcesHandle;

		public override void InstallBindings()
		{
			Container.Bind<ILocalizationService>().To<LocalizationService>().AsCached();
		}
	}
}
