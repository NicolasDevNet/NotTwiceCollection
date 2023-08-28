using UniRx;
using UnityEngine;

namespace Assets.NotTwice.UP.ZenjectExtend.Runtime.Abstract
{
	public interface IObservableExtendedPool<TObject> : IBasePool<TObject> where TObject : MonoBehaviour
	{
		public ReactiveCollection<TObject> ActiveItems { get; }
	}
}