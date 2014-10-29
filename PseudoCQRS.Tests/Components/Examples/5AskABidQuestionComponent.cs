using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PseudoCQRS.Tests.Components.Examples
{
	public class AskABidQuestionReadComponent
	{
		public AskABidQuestionReadComponent( IComponentBuilder builder, IComponentDependenciesFactory factory )
		{
			builder
				.Route( "Bid/{Id}/AskAQuestion" )
				.View( "Job/AskAQuestion" )
				.AuthorizedBy<LoggedInUser>()
				.AccessChecker<BidAskAQuestionArgument, BidVisibleToJobPosterAndAdminAccessChecker>( x => x.Id )
				.AddValidator<BidAskAQuestionViewModel, AskAQuestionViewModelValidator>()
				.ViewModelProvider( factory.ViewModelProviderFor<BidAskAQuestionViewModel> );
		}
	}

	public class AskABidQuestionWriteComponent
	{
		public AskABidQuestionWriteComponent( IComponentBuilder builder, IComponentDependenciesFactory factory )
		{
			builder
				.Route( "Bid/{Id}/AskAQuestion", ComponentRouteHttpMethod.POST )
				.CommandHandler( factory.CommandHandlerFor<BidAskAQuestionCommand> )
				.AddValidator<BidAskAQuestionCommand, BidAskAQuestionCommandNoContactDetailsValidator>()
				.AddValidator<BidAskAQuestionCommand, BidAskAQuestionCommandNoDuplicateQuestionsValidator>()
				.OnSuccess( x => x.View( "Job/QuestionSubmitted.cshtml" ) );
		}
	}
}