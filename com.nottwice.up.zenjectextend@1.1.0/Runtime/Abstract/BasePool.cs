using UnityEngine;
using Zenject;

namespace Assets.NotTwice.UP.ZenjectExtend.Runtime.Abstract
{
	public class BasePool<TObject> : MonoMemoryPool<TObject>, IBasePool<TObject>
		where TObject : MonoBehaviour
	{
		private readonly Transform _parent;

		public BasePool()
		{
		}

		public BasePool(Transform parent)
		{
			_parent = parent;
		}

		public virtual TObject SpawnWithParent()
		{
			var newInstance = Spawn();

			newInstance.transform.SetParent(_parent);

			return newInstance;
		}

		public virtual TObject SpawnWithParent(bool withDefaultScale)
		{
			var newInstance = SpawnWithParent();

			if (withDefaultScale)
				newInstance.transform.localScale = new Vector3(1, 1, 1);

			return newInstance;
		}

		public virtual TObject SpawnWithParent(Vector3 position)
		{
			var newInstance = SpawnWithParent();

			newInstance.transform.localPosition = position;

			return newInstance;
		}

		public virtual TObject SpawnWithParent(Vector3 position, bool withDefaultScale)
		{
			var newInstance = SpawnWithParent(position);

			if (withDefaultScale)
				newInstance.transform.localScale = new Vector3(1, 1, 1);

			return newInstance;
		}

		public virtual new void Despawn(TObject instance)
		{
			base.Despawn(instance);
		}
	}
}
