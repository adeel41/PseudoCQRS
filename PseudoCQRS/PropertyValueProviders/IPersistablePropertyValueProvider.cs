namespace PseudoCQRS.PropertyValueProviders
{
	public interface IPersistablePropertyValueProvider
	{
		void SetValue<TArg>( string key, object value );
	}
}