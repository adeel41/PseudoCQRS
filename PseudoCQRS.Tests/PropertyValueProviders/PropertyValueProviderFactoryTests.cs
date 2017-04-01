using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PseudoCQRS.PropertyValueProviders;
using Rhino.Mocks;

namespace PseudoCQRS.Tests.PropertyValueProviders
{
	[TestFixture]
	public class PropertyValueProviderFactoryTests
	{
		[SetUp]
		public void Setup()
		{
			var serviceLocator = MockRepository.GenerateMock<IServiceLocator>();
			ServiceLocator.SetLocatorProvider( () => serviceLocator );
		}

		[Test]
		public void ShouldReturnAllPropertyValueProviders()
		{
			var factory = CreateFactory();
			Assert.AreEqual( 5, factory.GetPropertyValueProviders().Count() );
		}

		private static PropertyValueProviderFactory CreateFactory()
		{
			return new PropertyValueProviderFactory(
				new CookiePropertyValueProvider( MockRepository.GenerateMock<IHttpContextWrapper>() ),
				new SessionPropertyValueProvider( MockRepository.GenerateMock<IHttpContextWrapper>() ),
				new RouteDataPropertyValueProvider( MockRepository.GenerateMock<IHttpContextWrapper>() ),
				new QueryStringPropertyValueProvider( MockRepository.GenerateMock<IHttpContextWrapper>() ),
				new FormDataPropertyValueProvider( MockRepository.GenerateMock<IHttpContextWrapper>() ) );
		}

		[Test]
		public void GetPersistablePropertyValueProviders_PersistanceLocationIsCookie_ReturnsCookiePersistablePropertyValueProvider()
		{
			var factory = CreateFactory();
			Assert.IsInstanceOf<CookiePropertyValueProvider>( factory.GetPersistablePropertyValueProvider( PersistanceLocation.Cookie ) );
		}

		[Test]
		public void GetPersistablePropertyValueProvider_PersistanceLocationIsSession_ReturnsSessionPropertyValueProvider()
		{
			var factory = CreateFactory();
			Assert.IsInstanceOf<SessionPropertyValueProvider>( factory.GetPersistablePropertyValueProvider( PersistanceLocation.Session ) );
		}
	}
}