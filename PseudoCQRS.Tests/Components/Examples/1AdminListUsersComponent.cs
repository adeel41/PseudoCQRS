namespace PseudoCQRS.Tests.Components.Examples
{
	//todo: implement IComponent... and implement method which accepts a builder as an argument.
	public class AdminListUsersComponent
	{
		public AdminListUsersComponent( IComponentBuilder builder )
		{
			builder
				.Route( "Admin/Users" )
				.AuthorizedBy<AdminUserOnly>()
				.View( "Admin/ListUsers" );
			//.ViewModelProvider<AdminListUsersViewModelProvider>(); //todo: use this one.

			// ( factory.ViewModelProviderFor<AdminListUsersViewModel> ) // use convention to find view model provider and command handler.
		}

		public class AdminListUsersViewModel {}
	}
}