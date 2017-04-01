using System;
using Microsoft.Practices.ServiceLocation;

namespace PseudoCQRS.Controllers
{
	[Obsolete( "Use IViewModelToCommandMappingEngine directly to convert from ViewModel to Command" )]
	public static class Mapper
	{
		public static TTo Map<TFrom, TTo>( TFrom viewModel )
		{
			IViewModelToCommandMappingEngine mappingEngine = null;
			var serviceLocator = ServiceLocator.Current;
			if ( serviceLocator == null )
				throw new ApplicationException(
					"ServiceLocator is null. Did you call ServiceLocator.SetLocatorProvider( {arg} ) in application initialization code? " );

			mappingEngine = ServiceLocator.Current.GetInstance<IViewModelToCommandMappingEngine>();
			if ( mappingEngine == null )
				throw new Exception(
					"IViewModelToCommandMappingEngine implementation is not registered. Please register it in your initialization code." );

			return mappingEngine.Map<TFrom, TTo>( viewModel );
		}
	}
}