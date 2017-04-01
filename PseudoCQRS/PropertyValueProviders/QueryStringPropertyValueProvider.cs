using System;

namespace PseudoCQRS.PropertyValueProviders
{
	public class QueryStringPropertyValueProvider : BasePropertyValueProvider, IPropertyValueProvider
	{
		private readonly IHttpContextWrapper _httpContextWrapper;

		public QueryStringPropertyValueProvider( IHttpContextWrapper httpContextWrapper )
		{
			_httpContextWrapper = httpContextWrapper;
		}

		public bool HasValue<T>( string key )
		{
			return _httpContextWrapper.ContainsQueryStringItem( key );
		}

		public object GetValue<T>( string key, Type propertyType )
		{
			return ConvertValue( _httpContextWrapper.GetQueryStringItem( key ), propertyType );
		}
	}
}