using System.Collections.Generic;
using UnityEngine;

namespace Assets.NotTwice.UP.ZenjectExtend.Runtime.Abstract
{
	public class ExtendedPool<TObject> : BasePool<TObject>, IExtendedPool<TObject>
		where TObject : MonoBehaviour
	{
		public List<TObject> ActiveItems { get; }

		public ExtendedPool()
		{
			ActiveItems = new List<TObject>();
		}

		public ExtendedPool(Transform parent) : base(parent)
		{
			ActiveItems = new List<TObject>();
		}
		protected override void OnSpawned(TObject item)
		{
			ActiveItems.Add(item);
			base.OnSpawned(item);
		}

		protected override void OnDespawned(TObject item)
		{
			ActiveItems.Remove(item);

			base.OnDespawned(item);
		}		
	}
}
