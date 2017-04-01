using System.Collections.Generic;

namespace PseudoCQRS.PropertyValueProviders
{
	public class PropertyValueProviderFactory : IPropertyValueProviderFactory
	{
		private readonly List<IPropertyValueProvider> _propertyValueProviders;
		private readonly CookiePropertyValueProvider _cookiePropertyValueProvider;
		private readonly SessionPropertyValueProvider _sessionPropertyValueProvider;

		public PropertyValueProviderFactory(
			CookiePropertyValueProvider cookie,
			SessionPropertyValueProvider session,
			RouteDataPropertyValueProvider route,
			QueryStringPropertyValueProvider queryString,
			FormDataPropertyValueProvider formData )
		{
			_cookiePropertyValueProvider = cookie;
			_sessionPropertyValueProvider = session;
			_propertyValueProviders = new List<IPropertyValueProvider>
			{
				cookie,
				session,
				route,
				queryString,
				formData
			};
		}

		public IEnumerable<IPropertyValueProvider> GetPropertyValueProviders() { return _propertyValueProviders; }

		public IPersistablePropertyValueProvider GetPersistablePropertyValueProvider( PersistanceLocation location )
		{
			IPersistablePropertyValueProvider result = null;
			switch ( location )
			{
				case PersistanceLocation.Cookie:
					result = _cookiePropertyValueProvider;
					break;
				case PersistanceLocation.Session:
					result = _sessionPropertyValueProvider;
					break;
			}

			return result;
		}
	}
}