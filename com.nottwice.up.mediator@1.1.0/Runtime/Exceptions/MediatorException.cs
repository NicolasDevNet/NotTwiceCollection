using Assets.NotTwice.UP.Mediator.Runtime.Enums;
using Assets.NotTwice.UP.Mediator.Runtime.Helpers;
using System;

namespace NotTwice.CA.Exceptions
{
	internal class MediatorException<T> : Exception
	{
		public MediatorException(ErrorType errorType, MediationType mediationType, Exception innerException = null)
			: base(LogHelper.BuildMediatorLog<T>(errorType, mediationType), innerException)
		{
		}
	}

	internal class MediatorException : Exception
	{
		public MediatorException(ErrorType errorType, MediationType mediationType, Exception innerException = null)
			: base(LogHelper.BuildMediatorLog(errorType, mediationType), innerException)
		{
		}
	}
}
