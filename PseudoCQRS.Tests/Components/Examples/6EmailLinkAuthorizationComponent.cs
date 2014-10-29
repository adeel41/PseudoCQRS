using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PseudoCQRS.Checkers;

namespace PseudoCQRS.Tests.Components.Examples
{
	public class EmailLinkAuthorizationComponent
	{
		// issue: how do i tell that you need to run command handler first.
		public EmailLinkAuthorizationComponent( IComponentBuilder builder, IComponentDependenciesFactory factory )
		{

			//VERY RARE: We need to exectute het command on the get.
			builder
				.Route( "AuthorizedLink/{Id}" )

				.AddValidator<EmailLinkAuthorizationCommand, ValidationCodeNotFound>( x => x.RedirectToComponent<LoginComponent>() )
				.AddValidator<EmailLinkAuthorizationCommand, ValidationCodeAlreadyUsed>( x => x.RedirectToComponent<LoginComponent>() )
				.CommandHandler( factory.CommandHandlerFor<EmailLinkAuthorizationCommand> )
				.OnSuccess( x => x.RedirectTo( "" ) ); // todo:: overload method which accepts Command or CommandResult
		}

		public class EmailLinkAuthorizationCommand {}

		public class ValidationCodeNotFound : IValidationChecker<EmailLinkAuthorizationCommand>
		{
			public CheckResult Check( EmailLinkAuthorizationCommand instance )
			{
				throw new NotImplementedException();
			}
		}

		public class ValidationCodeAlreadyUsed : IValidationChecker<EmailLinkAuthorizationCommand>
		{
			public CheckResult Check( EmailLinkAuthorizationCommand instance )
			{
				throw new NotImplementedException();
			}
		}
	}
}