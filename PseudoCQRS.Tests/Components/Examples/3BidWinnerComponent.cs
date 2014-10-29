// Read example

using System;
using PseudoCQRS.Checkers;

namespace PseudoCQRS.Tests.Components.Examples
{
	public class BidWinnerComponent
	{
		public BidWinnerComponent( IComponentBuilder builder, IComponentDependenciesFactory factory )
		{
			builder
				.Route( "Bid/{Id}/Winner" )
				.View( "Bid/Winner" )
				.AuthorizedBy<LoggedInUser>()
				.AddValidator<BidWinnerArgument, BidWinnerValidationCheck>()
				.ViewModelProvider( factory.ViewModelProviderFor<BidWinnerViewModel> );
		}

		public class BidWinnerArgument {}

		public class BidWinnerViewModel {}


		public class BidWinnerValidationCheck : IValidationChecker<BidWinnerArgument>
		{
			public CheckResult Check( BidWinnerArgument instance )
			{
				throw new NotImplementedException();
			}
		}
	}
}