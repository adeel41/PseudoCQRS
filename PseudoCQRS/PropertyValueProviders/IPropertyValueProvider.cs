using System;

namespace PseudoCQRS.PropertyValueProviders
{
	public interface IPropertyValueProvider
	{
		bool HasValue<T>( string key );
		object GetValue<T>( string key, Type propertyType );
	}
}