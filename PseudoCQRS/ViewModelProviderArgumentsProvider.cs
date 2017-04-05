using System;
using System.Linq;
using System.Reflection;
using PseudoCQRS.Attributes;
using PseudoCQRS.PropertyValueProviders;

namespace PseudoCQRS
{
	public class ViewModelProviderArgumentsProvider : IViewModelProviderArgumentsProvider
	{
		private readonly IPropertyValueProviderFactory _propertyValueProviderFactory;

		public ViewModelProviderArgumentsProvider( IPropertyValueProviderFactory propertyValueProviderFactory )
		{
			_propertyValueProviderFactory = propertyValueProviderFactory;
		}

		private readonly PropertyInfoCacher _propertyInfoCacher = new PropertyInfoCacher();

		public TArg GetArguments<TArg>() where TArg : new()
		{
			var retVal = new TArg();
			var properties = _propertyInfoCacher.GetProperties( typeof( TArg ) );
			var propertyValueProviders = _propertyValueProviderFactory.GetPropertyValueProviders();
			foreach ( var propertyValueProvider in propertyValueProviders )
			{
				foreach ( var kvp in properties )
				{
					if ( propertyValueProvider.HasValue<TArg>( kvp.Key ) )
						kvp.Value.SetValue( retVal, propertyValueProvider.GetValue<TArg>( kvp.Key, kvp.Value.PropertyType ), null );
				}
			}
			return retVal;
		}

		public void Persist<TArg>() where TArg : new()
		{
			Persist( GetArguments<TArg>() );
		}

		internal void Persist<TArg>( TArg arguments )
		{
			foreach ( var property in typeof( TArg ).GetTypeInfo().DeclaredProperties.Where( x => x.GetCustomAttribute( typeof(PersistAttribute) ) != null ) )
			{
				var persistLocation = property.GetCustomAttribute<PersistAttribute>().PersistanceLocation;
				var propertyValueProvider = _propertyValueProviderFactory.GetPersistablePropertyValueProvider( persistLocation );
				var propertyValue = property.GetValue( arguments, null );
				if ( propertyValue != null )
					propertyValueProvider.SetValue<TArg>( property.Name, propertyValue );
			}
		}
	}
}