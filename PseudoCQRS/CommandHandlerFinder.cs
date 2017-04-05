using System;
using System.Linq;

namespace PseudoCQRS
{
	public class CommandHandlerFinder : ICommandHandlerFinder
	{
		private readonly IPseudoCQRSServiceLocator _serviceLocator;
		private readonly IAssemblyListProvider _assembliesListProvider;

		public CommandHandlerFinder( IPseudoCQRSServiceLocator serviceLocator, IAssemblyListProvider assembliesListProvider )
		{
			_serviceLocator = serviceLocator;
			_assembliesListProvider = assembliesListProvider;
		}

		public ICommandHandler<TCommand> FindHandlerForCommand<TCommand>()
		{
			ICommandHandler<TCommand> result = default( ICommandHandler<TCommand> );

			var commandType = typeof( TCommand );
			var handlerInheritingFromType = typeof( ICommandHandler<> ).MakeGenericType( commandType );

			var commandHandlerType = GetCommandHandlerType( handlerInheritingFromType );
			if ( commandHandlerType != null )
				result = _serviceLocator.GetInstance( commandHandlerType ) as ICommandHandler<TCommand>;

			return result;
		}

		private Type GetCommandHandlerType( Type handlerInheritingFromType )
		{
			var commandHandlerType = _assembliesListProvider
				.GetAssemblies()
				.SelectMany( x => x.DefinedTypes )
				.SingleOrDefault( x => x.ImplementedInterfaces.Any( y => y == handlerInheritingFromType ) );

			return commandHandlerType.AsType();
		}
	}
}