using Assets.NotTwice.UP.ZenjectExtend.Runtime.Abstract;
using DG.Tweening;
using ModestTree;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.NotTwice.UP.Notifier.Runtime.Components
{
	/// <summary>
	/// Unity component to be used with the notification system, text and image components to be supplied with the prefab.
	/// </summary>    
	[DisallowMultipleComponent]
	[AddComponentMenu("NotTwice/Notifier/" + nameof(Notification))]
	public class Notification : MonoBehaviour
	{
		[SerializeField, Tooltip("Component containing the textual content of the notification")]
		public TMP_Text Content;

		[SerializeField, Tooltip("Component containing notification background image")]
		public Image Background;

		private MaskableGraphic[] _maskables;
		private RectTransform _rectTransform;

		public float Height { get { return _rectTransform?.rect.height ?? 0; } }

		public void OnCreated()
		{
			_maskables = GetComponentsInChildren<MaskableGraphic>();
			_rectTransform = GetComponent<RectTransform>();

			Hide(0);
		}

		public void Spawn(Color backgroundColor, string content, Vector2 position)
		{
			Content.text = content;
			Background.color = backgroundColor;
			transform.localPosition = position;
		}

		public Sequence ShowTemporarily(float fadeInDuration, float fadeOutDuration, float isVisibleDuration)
		{
			var sequence = DOTween.Sequence();

			sequence.Append(Show(fadeInDuration));

			sequence.Append(DOTween.Sequence().AppendInterval(isVisibleDuration));

			sequence.Append(Hide(fadeOutDuration));

			return sequence;
		}

		public Sequence Show(float fadeInDuration)
		{
			return GetFadeSequence(1, fadeInDuration);
		}

		public Sequence Hide(float fadeOutDuration)
		{
			return GetFadeSequence(0, fadeOutDuration);
		}

		private Sequence GetFadeSequence(float fadeTarget, float fadeDuration)
		{
			var sequence = DOTween.Sequence();

			foreach(var maskable in _maskables)
				sequence.Join(maskable.DOFade(fadeTarget, fadeDuration));

			return sequence;
		}
	}

	public class NotificationPool : ExtendedPool<Notification>, INotificationPool
	{
		public NotificationPool([Inject(Id = "NotificationsParent")]Transform parent) : base(parent)
		{
		}

		public Notification Spawn(Color backgroundColor, string content, float spaceY)
		{
			var instance = SpawnWithParent(true);

			var position = new Vector2(0, instance.Height * ActiveItems.Count + spaceY);

			instance.Spawn(backgroundColor, content, position);

			return instance;
		}

		public Sequence Despawn(Notification instance, float fadeOutDuration, float spaceY, float moveOthersAnimationDuration)
		{
			var sequence = instance.Hide(fadeOutDuration);

			if(ActiveItems.Count > 1)
			{
				var moveOthersSequence = DOTween.Sequence();
				var toMove = ActiveItems.Except(instance);

				foreach (var notification in toMove)
				{
					moveOthersSequence.Join(notification.transform.DOLocalMoveY(notification.transform.position.y - instance.Height - spaceY, moveOthersAnimationDuration));
				}

				sequence.Append(moveOthersSequence);
			}

			sequence.onComplete += () => base.Despawn(instance);

			return sequence;
		}

		protected override void OnCreated(Notification item)
		{
			item.OnCreated();
			base.OnCreated(item);
		}
	}

	public interface INotificationPool : IExtendedPool<Notification>
	{
		Notification Spawn(Color backgroundColor, string content, float spaceY);

		Sequence Despawn(Notification instance, float fadeOutDuration, float spaceY, float moveOthersAnimationDuration);
	}
}
