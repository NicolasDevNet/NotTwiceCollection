using UnityEngine;
using Zenject;

namespace Assets.NotTwice.UP.ZenjectExtend.Runtime.Abstract
{
	public interface IBasePool<TObject> : IMemoryPool<TObject> where TObject : MonoBehaviour
	{
		TObject SpawnWithParent(Vector3 position, bool withDefaultScale);
		TObject SpawnWithParent();
		TObject SpawnWithParent(Vector3 position);
		TObject SpawnWithParent(bool withDefaultScale);
	}
}
