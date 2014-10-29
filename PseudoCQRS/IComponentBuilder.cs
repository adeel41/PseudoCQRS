using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PseudoCQRS.Checkers;

namespace PseudoCQRS
{
	public interface IComponentBuilder
	{
		IComponentBuilder Route( string urlTemplate, Func<ComponentResultHelper, IComponentResult> returnValue  );
		IComponentBuilder ViewModelProvider<TViewModel>( Func<TViewModel> viewModelProviderFor );
		IComponentBuilder CommandHandler<TCommand>( Func<TCommand> commandHandlerFor );
		IComponentBuilder AccessChecker<T, TAccessChecker>( Func<T, object> property ) where TAccessChecker : IAccessChecker;
		IComponentBuilder AddValidator<T, TValidationChecker>(Func<ComponentResultHelper, IComponentResult> onfailure = null ) where TValidationChecker : IValidationChecker<T>;
		IComponentBuilder OnSuccess( Func<ComponentResultHelper, IComponentResult> onSuccessFunction );
		IComponentBuilder AuthorizedTo<TAuthorizationChecker>() where TAuthorizationChecker : IAuthorizationChecker;
	}

	public class ComponentResultHelper
	{
		public IComponentResult RedirectTo( string url )
		{
			throw new NotImplementedException();
		}

		public IComponentResult RedirectToRoute( Func<ComponentRouteBuilder, string> routeBuilder )
		{
			throw new NotImplementedException();
		}

		public IComponentResult View( string path )
		{
			throw new NotImplementedException();
		}

		public IComponentResult Json()
		{
			throw new NotImplementedException();
		}
	}

	public class ComponentRouteBuilder
	{
	}


	public interface IComponentResult { }


	public interface IComponentDependenciesFactory
	{
		TViewModel ViewModelProviderFor<TViewModel>();
		TCommand CommandHandlerFor<TCommand>();
	}
}