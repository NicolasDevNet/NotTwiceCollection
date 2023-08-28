using Assets.NotTwice.UP.Mediator.Runtime.Enums;

namespace Assets.NotTwice.UP.Mediator.Runtime.Helpers
{
	internal static class LogHelper
	{
		public static string BuildMediatorLog<T>(ErrorType errorType, MediationType mediationType)
			=> ErrorTypeHelper.GetErrorMessage(errorType) +
				  " | Type sent: " + typeof(T).Name +
				  BuildErrorCodeLog(errorType) +
				  BuildMediationTypeLog(mediationType);

		public static string BuildMediatorLog(ErrorType errorType, MediationType mediationType)
			=> ErrorTypeHelper.GetErrorMessage(errorType) +
				BuildErrorCodeLog(errorType) +
				BuildMediationTypeLog(mediationType);

		private static string BuildErrorCodeLog(ErrorType errorType)
			=> " | Error code: " + ((int)errorType).ToString();

		private static string BuildMediationTypeLog(MediationType mediationType)
			=> " | Mediation type: " + MediationTypeHelper.GetMediationString(mediationType);
	}
}
