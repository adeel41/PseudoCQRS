using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PseudoCQRS.ExtensionMethods
{
	public static class ReflectionExtensionMethods
	{
		public static IEnumerable<Type> GetImplementationsOf( this Assembly assembly, Type genericType, Type genericArgumentType )
		{
			var types = from t in assembly.DefinedTypes
						from i in t.ImplementedInterfaces
						let interfaceType = i.GetTypeInfo()
						where interfaceType.IsGenericType
						let interfaceGenericType = interfaceType.GetGenericTypeDefinition()
						where interfaceGenericType == genericType
						let firstGenericArgument = interfaceGenericType.GenericTypeArguments.FirstOrDefault()
						where firstGenericArgument != null && firstGenericArgument == genericArgumentType
						select t.AsType();


			return types
				.Distinct( new GenericEqualityComparer<Type>( x => x.FullName ) );
		}
	}
}