using System.Threading;
using System.Threading.Tasks;

namespace PseudoCQRS
{
	public class EventPublisher : IEventPublisher
	{
		private readonly ISubscriptionService _subscriptionService;
		private readonly IDbSessionManager _dbSessionManager;

		public EventPublisher( ISubscriptionService subscriptionService, IDbSessionManager dbSessionManager )
		{
			_subscriptionService = subscriptionService;
			_dbSessionManager = dbSessionManager;
		}

		public void Publish<T>( T @event )
		{
			PublishInternal( @event, true );
		}

		public void PublishSynchronously<T>( T @event )
		{
			PublishInternal( @event, false );
		}

		private void PublishInternal<T>( T @event, bool doAsync )
		{
			var subscribers = _subscriptionService.GetSubscriptions<T>();
			foreach ( var subscriber in subscribers )
			{
				if ( doAsync && subscriber.IsAsynchronous )
				{
					var eventSubscriber = subscriber;
					Task.Factory.StartNew(
						() =>
						{
							eventSubscriber.Notify( @event );
							_dbSessionManager.CloseSession();
						}, TaskCreationOptions.LongRunning );
				}
				else
				{
					subscriber.Notify( @event );
				}
			}
		}
	}
}