using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace PseudoCQRS.PropertyValueProviders
{
	public class FormDataPropertyValueProvider : BasePropertyValueProvider, IPropertyValueProvider
	{
		private readonly IHttpContextWrapper _httpContextWrapper;
		public FormDataPropertyValueProvider( IHttpContextWrapper httpContextWrapper )
		{
			_httpContextWrapper = httpContextWrapper;
		}

		private string GetKey( Type objectType, string propertyName )
		{
			return propertyName;
		}

		public bool HasValue<T>( string key )
		{
			var hasValue = _httpContextWrapper.ContainsFormItem( key );
			if ( !hasValue && _httpContextWrapper.ContainsFormItem( key + "_Exists" ) )
				hasValue = true;

			return hasValue;
		}

		public object GetValue<T>( string key, Type propertyType )
		{
			object val = _httpContextWrapper.GetFormItem( GetKey( propertyType, key ) );
			if ( PropertyIsListAndValueIsNull( propertyType, val ) )
				return GetEmptyListProperty( propertyType );

			return ConvertValue( val.ToString(), propertyType );
		}

		private bool PropertyIsListAndValueIsNull( Type propertyType, object value )
		{
			return propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof( List<> ) && value == null;
		}

		private IList GetEmptyListProperty( Type propertyType )
		{
			var containedType = propertyType.GetGenericArguments()[ 0 ];
			var emptyList = (IList)Activator.CreateInstance( ( typeof( List<> ).MakeGenericType( containedType ) ) );
			return emptyList;
		}
	}
}