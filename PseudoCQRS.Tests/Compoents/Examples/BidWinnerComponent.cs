using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Read example
using PseudoCQRS.Checkers;

namespace PseudoCQRS.Tests.Compoents.Examples
{
	public class BidWinnerComponent
	{
		public BidWinnerComponent(IComponentBuilder builder, IComponentDependenciesFactory factory)
		{
			builder
				.Route( "Bid/{Id}/Winner", x => x.View( "Bid/Winner" ) )
				.AuthorizedTo<LoggedInUser>()
				.AddValidator<BidWinnerArgument, BidWinnerValidationCheck>()
				.ViewModelProvider( factory.ViewModelProviderFor<BidWinnerViewModel> );
		}

		public class BidWinnerArgument
		{
			
		}

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
