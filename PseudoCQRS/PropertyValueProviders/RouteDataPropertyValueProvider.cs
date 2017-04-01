using System;

namespace PseudoCQRS.PropertyValueProviders
{
	public class RouteDataPropertyValueProvider : BasePropertyValueProvider, IPropertyValueProvider
	{
		private readonly IHttpContextWrapper _httpContextWrapper;

		public RouteDataPropertyValueProvider( IHttpContextWrapper httpContextWrapper )
		{
			_httpContextWrapper = httpContextWrapper;
		}

		public bool HasValue<T>( string key )
		{
			return _httpContextWrapper.ContainsRouteDataItem( key );
		}

		public object GetValue<T>( string key, Type propertyType )
		{
			return ConvertValue( _httpContextWrapper.GetRouteDataItem( key ).ToString(), propertyType );
		}
	}
}