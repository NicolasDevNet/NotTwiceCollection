using System.Collections.Generic;
using UnityEngine;

namespace Assets.NotTwice.UP.ZenjectExtend.Runtime.Abstract
{
	public interface IExtendedPool<TObject> : IBasePool<TObject> where TObject : MonoBehaviour
	{
		List<TObject> ActiveItems { get; }
	}
}
