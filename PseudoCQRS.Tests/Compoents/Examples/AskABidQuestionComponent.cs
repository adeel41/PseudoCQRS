using System;
using PseudoCQRS.Checkers;

namespace PseudoCQRS.Tests
{
	//Read Execute example

	// we need a convention to find all components or implement an interface which acts like a placeholder or an attribute decorated on components.
	public class AskABidQuestionComponent
	{
		public AskABidQuestionComponent( IComponentBuilder builder, IComponentDependenciesFactory factory )
		{
			builder
				.Route( "Bid/{Id}/AskAQuestion", x => x.View( "Job/AskAQuestion" ) ) // we need something which register these routes and also something which can build urls for these components.
				.AuthorizedTo<LoggedInUser>()
				.AccessChecker<BidAskAQuestionArgument, BidVisibleToJobPosterAndAdminAccessChecker>( x => x.Id )
				.AddValidator<BidAskAQuestionViewModel, AskAQuestionViewModelValidator>()
				.ViewModelProvider( factory.ViewModelProviderFor<BidAskAQuestionViewModel> )
				.CommandHandler( factory.CommandHandlerFor<BidAskAQuestionCommand> )
				.AddValidator<BidAskAQuestionCommand, BidAskAQuestionCommandNoContactDetailsValidator>()
				.AddValidator<BidAskAQuestionCommand, BidAskAQuestionCommandNoDuplicateQuestionsValidator>()
				.OnSuccess( x => x.View( "Job/QuestionSubmitted.cshtml" ) );
		}
	}

	public class BidAskAQuestionArgument
	{
		public int Id { get; set; }
	}

	public class BidAskAQuestionViewModel
	{
		public string QuestionText { get; set; }
		public int Id { get; set; }
		public JobDetailsViewModel JobDetails { get; set; }
	}

	public class JobDetailsViewModel { }

	public class AskAQuestionViewModelValidator : IValidationChecker<BidAskAQuestionViewModel>
	{
		public CheckResult Check( BidAskAQuestionViewModel instance )
		{
			throw new NotImplementedException();
		}
	}

	public class BidAskAQuestionCommand
	{
		public int Id { get; set; }
		public string QuestionText { get; set; }
	}

	public class BidVisibleToJobPosterAndAdminAccessChecker : IAccessChecker
	{
		public CheckResult Check( string propertyName, object instance )
		{
			throw new NotImplementedException();
		}
	}

	public class BidAskAQuestionCommandNoContactDetailsValidator : IValidationChecker<BidAskAQuestionCommand>
	{
		public CheckResult Check( BidAskAQuestionCommand instance )
		{
			throw new NotImplementedException();
		}
	}

	public class BidAskAQuestionCommandNoDuplicateQuestionsValidator : IValidationChecker<BidAskAQuestionCommand>
	{
		public CheckResult Check( BidAskAQuestionCommand instance )
		{
			throw new NotImplementedException();
		}
	}

	public class LoggedInUser : IAuthorizationChecker
	{
		public CheckResult Check()
		{
			throw new NotImplementedException();
		}
	}
}