using System;

namespace PseudoCQRS.PropertyValueProviders
{
	public class CookiePropertyValueProvider : BasePropertyValueProvider,
		IPropertyValueProvider,
		IPersistablePropertyValueProvider
	{
		private readonly IHttpContextWrapper _httpContextWrapper;

		public CookiePropertyValueProvider( IHttpContextWrapper httpContextWrapper )
		{
			_httpContextWrapper = httpContextWrapper;
		}

		private const string KeyFormat = @"{0}:{1}";

		private string GetKey( Type objectType, string propertyName )
		{
			return String.Format( KeyFormat, objectType.FullName, propertyName );
		}

		public bool HasValue<T>( string key )
		{
			return _httpContextWrapper.RequestContainsCookie( GetKey( typeof ( T ), key ) );
		}

		public object GetValue<T>( string key, Type propertyType )
		{
			string value = _httpContextWrapper.RequestGetCookieValue( GetKey( typeof ( T ), key ) );
			return ConvertValue( value, propertyType );
		}

		public void SetValue<T>( string key, object value )
		{
			_httpContextWrapper.ResponseSetCookie( GetKey( typeof ( T ), key ), value.ToString() );
		}
	}
}