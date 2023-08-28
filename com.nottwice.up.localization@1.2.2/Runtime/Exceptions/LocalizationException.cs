using Assets.NotTwice.UP.Localization.Runtime.Enums;
using Cysharp.Text;
using System;

namespace Assets.NotTwice.UP.Localization.Runtime.Exceptions
{
	internal class LocalizationException : Exception
	{
		public LocalizationException(string message, ResourceType resourceType, Exception innerException)
			: base(ZString.Format("{0} | Resource type: {1}", message, resourceType), innerException)
		{
		}

		public LocalizationException(string message, ResourceType resourceType, string sceneName)
			: base(ZString.Format("{0} | Resource type: {1} | Scene: {2}", message, resourceType, sceneName))
		{
		}

		public LocalizationException(string message, string sceneName, string language)
			: base(ZString.Format("{0} | Scene: {1} | Language: {2}", message, sceneName, language))
		{
		}

		public LocalizationException(string message) : base(message)
		{
		}
	}
}
