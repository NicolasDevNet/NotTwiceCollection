using Assets.NotTwice.UP.Mediator.Runtime.Constants;
using Assets.NotTwice.UP.Mediator.Runtime.Enums;

namespace Assets.NotTwice.UP.Mediator.Runtime.Helpers
{
	internal class MediationTypeHelper
	{
		public static string GetMediationString(MediationType mediationType)
		{
			switch (mediationType)
			{
				case MediationType.None:
					return MediatorConstants.MediatorType;
				case MediationType.Query:
					return MediatorConstants.QueryType;
				case MediationType.QueryAsync:
					return MediatorConstants.QueryAsyncType;
				case MediationType.Command:
					return MediatorConstants.CommandType;
				case MediationType.CommandAsync:
					return MediatorConstants.CommandAsyncType;
				case MediationType.Messenger:
					return MediatorConstants.MessengerType;
				case MediationType.MessengerAsync:
					return MediatorConstants.MessengerAsyncType;
				default:
					return string.Empty;
			}
		}
	}
}
