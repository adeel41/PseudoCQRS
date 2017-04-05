using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PseudoCQRS.Configuration
{
	public class DependencyInjectionContainer : IDependencyInjectionContainer
	{
		private static readonly Dictionary<Type, Type> registrations;
		private static readonly Dictionary<Type, object> instances;

		static DependencyInjectionContainer()
		{
			registrations = new Dictionary<Type, Type>();
			instances = new Dictionary<Type, object>();
		}

		public void Register( Type interfaceType, Type implementationType )
		{
			registrations.Add( interfaceType, implementationType );
		}

		public void Register<TInterface, TImplementation>()
		{
			Register( typeof( TInterface ), typeof( TImplementation ) );
		}

		public void Register( IDictionary<Type, Type> mappings )
		{
			foreach ( var mapping in mappings )
				registrations.Add( mapping.Key, mapping.Value );
		}

		public object Resolve( Type type )
		{
			object result;

			if ( instances.ContainsKey( type ) )
				result = instances[ type ];
			else
			{
				result = CreateInstanceOfType( type );
				SaveRegisteredTypeInstance( type, result );
			}

			return result;
		}

		public TService Resolve<TService>() where TService : class
		{
			return Resolve( typeof( TService ) ) as TService;
		}

		private static void SaveRegisteredTypeInstance( Type type, object instance )
		{
			if ( !registrations.ContainsKey( type ) )
				instances.Add( type, instance );
		}

		private object CreateInstanceOfType( Type type )
		{
			var concreateType = GetConcreteType( type );
			var constructors = concreateType.GetTypeInfo().DeclaredConstructors;

			var result = new object();
			var constructor = constructors.FirstOrDefault();
			if ( constructor != null )
				result = CreateInstanceFromConstructor( constructor, concreateType );

			return result;
		}

		private object CreateInstanceFromConstructor( ConstructorInfo constructor, Type type )
		{
			var parameters = new List<object>();
			foreach ( var parameterInfo in constructor.GetParameters() )
				parameters.Add( Resolve( parameterInfo.ParameterType ) );

			var result = Activator.CreateInstance( type, parameters.ToArray() );
			return result;
		}

		private static Type GetConcreteType( Type type )
		{
			const string exceptionFormat = "The type '{0}' is not registered as a dependency";
			bool isRegistered = registrations.ContainsKey( type );
			if ( !isRegistered && type.GetTypeInfo().IsInterface )
				throw new Exception( String.Format( exceptionFormat, type.FullName ) );

			var result = registrations.ContainsKey( type ) ? registrations[ type ] : type;
			return result;
		}

		public void ClearAll()
		{
			registrations.Clear();
			instances.Clear();
		}
	}
}