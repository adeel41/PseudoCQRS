using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PseudoCQRS.Checkers;
using PseudoCQRS.Controllers;
using PseudoCQRS.ExtensionMethods;
using PseudoCQRS.Helpers;
using PseudoCQRS.PropertyValueProviders;

namespace PseudoCQRS.Configuration
{
	public class DIConfiguration
	{
		private readonly List<Action<OverridableServicesContainer>> _overrides;
		private readonly List<Assembly> _viewModelProvidersAssemblies;

		public DIConfiguration()
		{
			_overrides = new List<Action<OverridableServicesContainer>>();
			_viewModelProvidersAssemblies = new List<Assembly>();
		}

		public DIConfiguration SetImplementationType( Action<OverridableServicesContainer> func )
		{
			_overrides.Add( func );
			return this;
		}

		public DIConfiguration ConfigureViewModelProvidersIn( params Assembly[] assembliesContainingViewModelProviders )
		{
			_viewModelProvidersAssemblies.AddRange( assembliesContainingViewModelProviders );
			return this;
		}

		public Dictionary<Type, Type> Build()
		{
			var result = new Dictionary<Type, Type>();

			result.AddRange( GetOverriddenTypes() );
			AddDefaultImplementations( result );
			var viewModelProvidersTypes = GetViewModelProvidersTypes();
			result.AddRange( viewModelProvidersTypes );
			result.AddRange( GetViewModelFactoriesTypesFor( viewModelProvidersTypes ) );

			return result;
		}

		private static void AddDefaultImplementations( Dictionary<Type, Type> mappings )
		{
			SetImplementation( mappings, typeof( ICheckersExecuter ), typeof( CheckersExecuter ) );
			SetImplementation( mappings, typeof( ICheckersFinder ), typeof( CheckersFinder ) );
			SetImplementation( mappings, typeof( IPrerequisitesChecker ), typeof( PrerequisitesChecker ) );
			SetImplementation( mappings, typeof( ICommandExecutor ), typeof( CommandExecutor ) );
			SetImplementation( mappings, typeof( IMessageManager ), typeof( SessionBasedMessageManager ) );
			SetImplementation( mappings, typeof( IReferrerProvider ), typeof( ReferrerProvider ) );
			SetImplementation( mappings, typeof( IObjectLookupCache ), typeof( ObjectLookupCache ) );
			SetImplementation( mappings, typeof( IPropertyValueProviderFactory ), typeof( PropertyValueProviderFactory ) );
			SetImplementation( mappings, typeof( IAssemblyListProvider ), typeof( AssemblyListProvider ) );
			SetImplementation( mappings, typeof( ICommandBus ), typeof( CommandBus ) );
			SetImplementation( mappings, typeof( ICommandHandlerFinder ), typeof( CommandHandlerFinder ) );
			SetImplementation( mappings, typeof( ICommandHandlerProvider ), typeof( CommandHandlerProvider ) );
			SetImplementation( mappings, typeof( IEventPublisher ), typeof( EventPublisher ) );
			SetImplementation( mappings, typeof( ISubscriptionService ), typeof( SubscriptionService ) );
			SetImplementation( mappings, typeof( IViewModelProviderArgumentsProvider ),
			                   typeof( ViewModelProviderArgumentsProvider ) );

			var types = FindImplementationsInOtherPseduoCQRSAssemblies();
			foreach ( var type in types )
				SetImplementation( mappings, type.Key, type.Value );
		}

		private static Dictionary<Type, Type> FindImplementationsInOtherPseduoCQRSAssemblies()
		{
			var mvcAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where( x => x.FullName.Contains( "PseudoCQRS." ) );
			var types = ( from mvcAssembly in mvcAssemblies
			              from type in mvcAssembly.GetTypes()
			              where typeof( IImplementationsFinder ).IsAssignableFrom( type )
			              let instance = Activator.CreateInstance( type ) as IImplementationsFinder
			              select instance.FindImplementations() ).SelectMany( x => x ).ToDictionary( x => x.Key, x => x.Value );

			return types;
		}

		private static void SetImplementation( Dictionary<Type, Type> mappings, Type interfaceType, Type implementationType )
		{
			if ( !mappings.ContainsKey( interfaceType ) )
				mappings.Add( interfaceType, implementationType );
		}

		private Dictionary<Type, Type> GetOverriddenTypes()
		{
			var result = new Dictionary<Type, Type>();
			foreach ( var @override in _overrides )
			{
				var container = new OverridableServicesContainer();
				@override( container );
				result.AddRange( container.GetOverrides() );
			}

			return result;
		}

		private Dictionary<Type, Type> GetViewModelProvidersTypes()
		{
			var viewModelProviderInterfaceType = typeof( IViewModelProvider<,> );
			var allViewModelProviderTypes = from assembly in _viewModelProvidersAssemblies
			                                from type in assembly.DefinedTypes
			                                let viewModelProviderInterface = type.ImplementedInterfaces.FirstOrDefault(x => x.FullName == viewModelProviderInterfaceType.FullName)
			                                where viewModelProviderInterface != null
			                                let genericArguments = viewModelProviderInterface.GenericTypeArguments
			                                let interfaceGenericType = viewModelProviderInterfaceType.MakeGenericType( genericArguments )
			                                select new
			                                {
				                                Interface = interfaceGenericType,
				                                Implementation = type.AsType()
			                                };

			var result = allViewModelProviderTypes.ToDictionary( x => x.Interface, x => x.Implementation );
			return result;
		}

		private static Dictionary<Type, Type> GetViewModelFactoriesTypesFor( Dictionary<Type, Type> viewModelProvidersTypes )
		{
			var viewModelFactoriesTypes = from viewModelProviderMapping in viewModelProvidersTypes
			                              let genericArguments = viewModelProviderMapping.Key.GenericTypeArguments
			                              let interfaceType = typeof( IViewModelFactory<,> ).MakeGenericType( genericArguments )
			                              let implementationType = typeof( ViewModelFactory<,> ).MakeGenericType( genericArguments )
			                              select new
			                              {
				                              Interface = interfaceType,
				                              Implementation = implementationType
			                              };

			var result = viewModelFactoriesTypes.ToDictionary( x => x.Interface, x => x.Implementation );
			return result;
		}
	}
}