namespace Assets.NotTwice.UP.Mediator.Runtime.Constants
{
	internal static class MediatorConstants
	{
		#region Error messages

		public const string MsgErr01 = "An error occurred during the execution of the command. Check its behavior.";
		public const string MsgErr02 = "An error occurred during the mediation instance recovery process. Check your instance container.";
		public const string MsgErr03 = "A parameter is missing to use the mediator correctly.";
		public const string MsgErr04 = "There is no query handler attached to it.";
		public const string MsgErr05 = "An error occurred during the execution of the query. Check its behavior.";
		public const string MsgErr06 = "An error occurred during the execution of the message. Check its behavior.";


		#endregion

		#region Mediation types

		public const string MediatorType = "Mediator";
		public const string CommandType = "Command";
		public const string CommandAsyncType = "CommandAsync";
		public const string QueryType = "Query";
		public const string QueryAsyncType = "QueryAsync";
		public const string MessengerType = "Messenger";
		public const string MessengerAsyncType = "MessengerAsync";

		#endregion
	}
}
