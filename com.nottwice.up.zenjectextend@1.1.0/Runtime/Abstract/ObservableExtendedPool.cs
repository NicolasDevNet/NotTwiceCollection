using UniRx;
using UnityEngine;

namespace Assets.NotTwice.UP.ZenjectExtend.Runtime.Abstract
{
	public class ObservableExtendedPool<TObject> : BasePool<TObject>, IObservableExtendedPool<TObject>
		where TObject : MonoBehaviour
	{
		public ReactiveCollection<TObject> ActiveItems { get; }

		public ObservableExtendedPool()
		{
			ActiveItems = new ReactiveCollection<TObject>();
		}

		public ObservableExtendedPool(Transform parent) : base(parent)
		{
			ActiveItems = new ReactiveCollection<TObject>();
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
