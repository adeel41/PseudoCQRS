using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;

namespace PseudoCQRS.Checkers
{
	public class CheckersFinder : ICheckersFinder
	{
		private readonly IServiceLocator _serviceLocator;

		private class CheckersFinderResult<TAttribute, TChecker>
		{
			public TAttribute Attribute { get; set; }
			public TChecker Checker { get; set; }
		}

		public CheckersFinder( IServiceLocator serviceLocator ) { _serviceLocator = serviceLocator; }

		private IEnumerable<CheckersFinderResult<TAttribute, TChecker>> GetCheckersImplmenting<TAttribute, TChecker>( object instance )
		{
			var result = new List<CheckersFinderResult<TAttribute, TChecker>>();

			foreach ( var attrib in instance.GetType().GetCustomAttributes( typeof( TAttribute ), true ) )
			{
				var checker = _serviceLocator.GetInstance( ( (BaseCheckAttribute)attrib ).CheckerType );
				result.Add(
					new CheckersFinderResult<TAttribute, TChecker>
					{
						Checker = (TChecker)checker,
						Attribute = (TAttribute)attrib
					} );
			}

			return result;
		}

		public List<IValidationChecker<T>> FindValidationCheckers<T>( T instance )
		{
			return GetCheckersImplmenting<ValidationCheckAttribute, IValidationChecker<T>>( instance ).Select( x => x.Checker ).ToList();
		}

		public List<IAuthorizationChecker> FindAuthorizationCheckers( object instance )
		{
			return GetCheckersImplmenting<AuthorizationCheckAttribute, IAuthorizationChecker>( instance ).Select( x => x.Checker ).ToList();
		}

		public List<AccessCheckerAttributeDetails> FindAccessCheckers( object instance )
		{
			return GetCheckersImplmenting<AccessCheckAttribute, IAccessChecker>( instance )
				.Select(
					x => new AccessCheckerAttributeDetails
					{
						AccessChecker = x.Checker,
						PropertyName = x.Attribute.PropertyName
					} )
				.ToList();
		}
	}
}