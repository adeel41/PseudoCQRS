using System;
using PseudoCQRS.Checkers;

//Write example

namespace PseudoCQRS.Tests.Components.Examples
{
	public class AdminDeleteUserComponent
	{
		public AdminDeleteUserComponent(IComponentBuilder builder, IComponentDependenciesFactory factory)
		{
			builder
				.Route( "Admin/User/{Id}/Delete", ComponentRouteHttpMethod.POST )
				.AuthorizedBy<AdminUserOnly>()
				.CommandHandler<DeleteUserCommandHandler>()
				.AddValidator<DeleteUserCommand, DeleteUserCommandValidator>()
				.OnSuccess( x => x.RedirectToComponent<AdminListUsersComponent>() );
			//todo:: should have a onfalilure method.
		}

		public class DeleteUserCommand
		{
			
		}

		public class DeleteUserCommandValidator : IValidationChecker<DeleteUserCommand>
		{
			public CheckResult Check( DeleteUserCommand instance )
			{
				throw new NotImplementedException();
			}
		}
    }

	public class AdminUserOnly : IAuthorizationChecker
	{
		public CheckResult Check()
		{
			throw new NotImplementedException();
		}
	}
}
