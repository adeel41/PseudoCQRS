using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PseudoCQRS.Helpers;
using PseudoCQRS.Mvc;
using Rhino.Mocks;

namespace PseudoCQRS.Tests
{
	[TestFixture]
	public class SubscriptionServiceTests
	{
		private IServiceLocator _mockedServiceLocator;
		private IEventSubscriberAssembliesProvider _eventSubscriberAssembliesProvider;

		private IObjectLookupCache _cache;
		private SubscriptionService _service;

		[SetUp]
		public void Setup()
		{
			_mockedServiceLocator = MockRepository.GenerateMock<IServiceLocator>();
			_eventSubscriberAssembliesProvider = MockRepository.GenerateMock<IEventSubscriberAssembliesProvider>();

			_eventSubscriberAssembliesProvider.Stub( x => x.GetEventSubscriberAssemblies() ).Return( new List<Assembly>()
			{
				this.GetType().Assembly
			} );

			ServiceLocator.SetLocatorProvider( () => _mockedServiceLocator );

			_cache = MockRepository.GenerateMock<IObjectLookupCache>();

			_service = new SubscriptionService( _cache, _eventSubscriberAssembliesProvider, new PseudoCQRSServiceLocator() );
		}

		// EXPECT: Call _cache.GetValue
		[Test]
		public void ShouldCallCacheGetValue()
		{
			_service.GetSubscriptions<TestingEvent>();

			_cache.AssertWasCalled( x => x.GetValue<IEnumerable<Type>>( typeof( TestingEvent ).FullName, null ) );
		}


		// WHEN: _cache.GetValue returns null
		// WHEN: GetImplementationsOf returns 0
		// EXPECT: Return a list with 0 items
		[Test]
		public void ShouldReturn0ItemsWhenNoSubscribersFound()
		{
			var result = _service.GetSubscriptions<EventWithNoSubscribers>();
			Assert.AreEqual( 0, result.Count() );
		}

		// WHEN: _cache.GetValue returns null
		// WHEN: GetImplementationsOf returns 0
		// EXPECT: Not Call _cache.SetValue
		[Test]
		public void ShouldNotCallCacheSetValueWhenGetImplementationsOfReturns0()
		{
			var result = _service.GetSubscriptions<EventWithNoSubscribers>();

			_cache.AssertWasNotCalled( x => x.SetValue<IEnumerable<Type>>( Arg<string>.Is.Anything, Arg<IEnumerable<Type>>.Is.Anything ) );
		}


		// WHEN: _cache.GetValue returns null
		// WHEN: GetImplementationsOf return > 0
		// EXPECT: Call _cache.SetValue
		[Test]
		public void ShouldCallCacheSetValueWhenGetImplementationsOfReturnsGreaterThan0()
		{
			var result = _service.GetSubscriptions<TestingEvent>();

			_cache.AssertWasCalled( x => x.SetValue<IEnumerable<Type>>( Arg.Text.Contains( typeof( TestingEvent ).FullName ), Arg<IEnumerable<Type>>.Is.Anything ) );
		}

		// WHEN: _cache.GetValue is not null
		// EXPECT: Returned Value = 1
		[Test]
		public void ShouldReturnSubsribersWhenCacheGetValueIsNotNull()
		{
			_cache
				.Stub( x => x.GetValue<IEnumerable<Type>>( String.Empty, null ) )
				.IgnoreArguments()
				.Return( new List<Type>
				{
					typeof( EventWithNoSubscribers )
				} );

			var result = _service.GetSubscriptions<EventWithNoSubscribers>();

			Assert.AreEqual( 1, result.Count() );
		}

		// WHEN: _cache.GetValue is null
		// WHEN: GetImplementationsOf return > 0
		// EXPECT: Returned Value = 1
		[Test]
		public void ShouldReturnSubscribersWhenCacheGetValueIsNullButGetImplementationsOfGreaterThan0()
		{
			var result = _service.GetSubscriptions<TestingEvent>();

			Assert.AreEqual( 1, result.Count() );
		}
	}


	public class EventWithNoSubscribers {}

	public class TestingEvent {}

	public class TestingEventSubscriber : IEventSubscriber<TestingEvent>
	{
		public void Notify( TestingEvent @event )
		{
			throw new NotImplementedException();
		}

		public bool IsAsynchronous
		{
			get { throw new NotImplementedException(); }
		}
	}
}