using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PseudoCQRS.Checkers;

namespace PseudoCQRS
{


	public interface IComponentBuilder
	{
		//todo: urlTemplate can be replace with ComponentRouteBuilder
		IComponentBuilder Route( string urlTemplate, ComponentRouteHttpMethod method = ComponentRouteHttpMethod.GET );
		IComponentBuilder View( string viewPath );
		IComponentBuilder ViewModelProvider<TViewModel>( Func<TViewModel> viewModelProviderFor );
		IComponentBuilder CommandHandler<TCommand>( Func<TCommand> commandHandlerFor );
		IComponentBuilder AccessChecker<T, TAccessChecker>( Func<T, object> property ) where TAccessChecker : IAccessChecker;
		IComponentBuilder AddValidator<T, TValidationChecker>( Func<ComponentResultHelper, IComponentResult> onfailure = null ) where TValidationChecker : IValidationChecker<T>;
		IComponentBuilder OnSuccess( Func<ComponentResultHelper, IComponentResult> onSuccessFunction );
		IComponentBuilder AuthorizedBy<TAuthorizationChecker>() where TAuthorizationChecker : IAuthorizationChecker;
	}

	public enum ComponentRouteHttpMethod
	{
		// ReSharper disable InconsistentNaming
		GET,
		POST
		// ReSharper restore InconsistentNaming
	}

	public class ComponentResultHelper
	{
		public IComponentResult RedirectTo( string url )
		{
			throw new NotImplementedException();
		}

		public IComponentResult RedirectToComponent<TComponent>(  )
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

	public class ComponentRouteBuilder {}


	public interface IComponentResult {}


	public interface IComponentDependenciesFactory
	{
		TViewModel ViewModelProviderFor<TViewModel>();
		TCommand CommandHandlerFor<TCommand>();
	}
}