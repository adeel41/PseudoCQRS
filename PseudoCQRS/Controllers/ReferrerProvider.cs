using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.ServiceLocation;

namespace PseudoCQRS.Controllers
{
	public class ReferrerProvider : IReferrerProvider
	{
		private readonly IHttpContextWrapper _httpContextWrapper;

		public ReferrerProvider( IHttpContextWrapper httpContextWrapper )
		{
			_httpContextWrapper = httpContextWrapper;
		}

		public string GetAbsoluteUri()
		{
			return _httpContextWrapper.GetUrlReferrerAbsoluteUri();
		}
	}
}