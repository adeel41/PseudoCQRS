using System;
using System.Linq;
using System.Reflection;

namespace PseudoCQRS
{
	public static class CommandHandlerExtensionMethods
	{
		public static bool HasTransactionAttribute<T>( this ICommandHandler<T> commandHandler )
		{
			var attributeType = typeof( DbTransactionAttribute );
			return commandHandler.GetType().GetTypeInfo().GetCustomAttributes( attributeType , false ).Any();
		}
	}
}