using System;
using System.Collections.Generic;
using System.Linq;
using PseudoCQRS.ExtensionMethods;
using PseudoCQRS.Helpers;

namespace PseudoCQRS
{
	public class SubscriptionService : ISubscriptionService
	{
		private readonly IObjectLookupCache _cache;
		private readonly IEventSubscriberAssembliesProvider _eventSubscriberAssembliesProvider;
		private readonly IPseudoCQRSServiceLocator _serviceLocator;

		public SubscriptionService(
			IObjectLookupCache cache,
			IEventSubscriberAssembliesProvider eventSubscriberAssembliesProvider,
			IPseudoCQRSServiceLocator serviceLocator )
		{
			_cache = cache;
			_eventSubscriberAssembliesProvider = eventSubscriberAssembliesProvider;
			_serviceLocator = serviceLocator;
		}

		public IEnumerable<IEventSubscriber<T>> GetSubscriptions<T>()
		{
			var result = new List<IEventSubscriber<T>>();

			string typeFullName = typeof( T ).FullName;
			var subscribers = _cache.GetValue<IEnumerable<Type>>( typeFullName, null );
			if ( subscribers == null )
			{
				subscribers = GetSubscribers<T>();
				if ( subscribers.Any() )
					_cache.SetValue<IEnumerable<Type>>( typeFullName, subscribers );
			}

			foreach ( var subscriberType in subscribers )
				result.Add( _serviceLocator.GetInstance( subscriberType ) as IEventSubscriber<T> );

			return result;
		}

		protected virtual List<Type> GetSubscribers<T>()
		{
			return ( from assembly in _eventSubscriberAssembliesProvider.GetEventSubscriberAssemblies()
					 from t in assembly.GetImplementationsOf( typeof( IEventSubscriber<> ), typeof( T ) )
					 select t ).ToList();
		}
	}
}