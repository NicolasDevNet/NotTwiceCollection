using Assets.NotTwice.UP.Mediator.Runtime.Enums;
using Assets.NotTwice.UP.Mediator.Runtime.Helpers;
using System;
using UnityEngine;

namespace Assets.NotTwice.UP.Mediator.Runtime.Extentions
{
	internal static class LoggerExtentions
	{
		public static void LogMediatorErrorAsInformation<T>(this ILogger logger, ErrorType errorType, MediationType mediationType)
			=> logger.Log(LogHelper.BuildMediatorLog<T>(errorType, mediationType));

		public static void LogMediatorErrorAsInformation(this ILogger logger, ErrorType errorType, MediationType mediationType)
			=> logger.Log(LogHelper.BuildMediatorLog(errorType, mediationType));

		public static void LogMediatorErrorAsInformation<T>(this ILogger logger, ErrorType errorType, MediationType mediationType, Exception exception)
			=> logger.Log(exception.Message + " | " + LogHelper.BuildMediatorLog<T>(errorType, mediationType));

		public static void LogMediatorErrorAsInformation(this ILogger logger, ErrorType errorType, MediationType mediationType, Exception exception)
			=> logger.Log(exception.Message + " | " + LogHelper.BuildMediatorLog(errorType, mediationType));
	}
}
