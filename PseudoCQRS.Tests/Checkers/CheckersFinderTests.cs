using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using PseudoCQRS.Checkers;
using PseudoCQRS.Mvc;
using Rhino.Mocks;

namespace PseudoCQRS.Tests.Checkers
{
	[TestFixture]
	public class CheckersFinderTests
	{
		[ValidationCheck( typeof( TestValidationChecker ) )]
		[ValidationCheck( typeof( TestValidationChecker2 ) )]
		[AuthorizationCheck( typeof( TestAuthorizationChecker ) )]
		[AccessCheck( typeof( TestAccessChecker1 ), "TeamId" )]
		[AccessCheck( typeof( TestAccessChecker2 ), "UserId" )]
		[AccessCheck( typeof( TestAccessChecker3 ), "JobId" )]
		public class TestCommandForCheckersFinder {}

		public class TestValidationChecker : IValidationChecker<TestCommandForCheckersFinder>
		{
			public CheckResult Check( TestCommandForCheckersFinder instance )
			{
				return new CheckResult();
			}
		}

		public class TestValidationChecker2 : IValidationChecker<TestCommandForCheckersFinder>
		{
			public CheckResult Check( TestCommandForCheckersFinder instance )
			{
				return new CheckResult();
			}
		}

		public class TestAuthorizationChecker : IAuthorizationChecker
		{
			public CheckResult Check()
			{
				return new CheckResult();
			}
		}

		public class TestAccessChecker1 : IAccessChecker
		{
			public CheckResult Check( string propertyName, object instance )
			{
				return new CheckResult();
			}
		}

		public class TestAccessChecker2 : IAccessChecker
		{
			public CheckResult Check( string propertyName, object instance )
			{
				return new CheckResult();
			}
		}

		public class TestAccessChecker3 : IAccessChecker
		{
			public CheckResult Check( string propertyName, object instance )
			{
				return new CheckResult();
			}
		}

		private CheckersFinder _finder;

		[SetUp]
		public void Setup()
		{
			var mockedServiceLocator = MockRepository.GenerateMock<IServiceLocator>();
			mockedServiceLocator
				.Stub( x => x.GetInstance( typeof( TestValidationChecker ) ) )
				.Return( new TestValidationChecker() );
			mockedServiceLocator
				.Stub( x => x.GetInstance( typeof( TestValidationChecker2 ) ) )
				.Return( new TestValidationChecker2() );
			mockedServiceLocator
				.Stub( x => x.GetInstance( typeof( TestAuthorizationChecker ) ) )
				.Return( new TestAuthorizationChecker() );
			mockedServiceLocator
				.Stub( x => x.GetInstance( typeof( TestAccessChecker1 ) ) )
				.Return( new TestAccessChecker1() );
			mockedServiceLocator
				.Stub( x => x.GetInstance( typeof( TestAccessChecker2 ) ) )
				.Return( new TestAccessChecker2() );
			mockedServiceLocator
				.Stub( x => x.GetInstance( typeof( TestAccessChecker3 ) ) )
				.Return( new TestAccessChecker3() );

			ServiceLocator.SetLocatorProvider( () => mockedServiceLocator );
			_finder = new CheckersFinder( new PseudoCQRSServiceLocator() );
		}

		[Test]
		public void FindValidationCheckersCanReturnCheckers()
		{
			Assert.AreEqual( 2, _finder.FindValidationCheckers( new TestCommandForCheckersFinder() ).Count );
		}

		[Test]
		public void FindAuthorizationCheckersCanReturnCheckers()
		{
			Assert.AreEqual( 1, _finder.FindAuthorizationCheckers( new TestCommandForCheckersFinder() ).Count );
		}

		[Test]
		public void FindAccessCheckersCanReturnCheckers()
		{
			Assert.AreEqual( 3, _finder.FindAccessCheckers( new TestCommandForCheckersFinder() ).Count );
		}
	}
}