using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PseudoCQRS
{
	public class PropertyInfoCacher
	{
		private readonly Dictionary<Type, Dictionary<string, PropertyInfo>> _propertiesCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
		private readonly object _lockObj = new object();

		public Dictionary<string, PropertyInfo> GetProperties( Type type )
		{
			if ( !_propertiesCache.ContainsKey( type ) )
			{
				lock ( _lockObj )
				{
					if ( !_propertiesCache.ContainsKey( type ) )
						FillCache( type );
				}
			}
			return _propertiesCache[ type ];
		}

		private void FillCache( Type type )
		{
			_propertiesCache.Add( type, type.GetTypeInfo().DeclaredProperties.ToDictionary( x => x.Name, x => x ) );
		}
	}
}