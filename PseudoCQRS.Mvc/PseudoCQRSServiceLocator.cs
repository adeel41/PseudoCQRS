using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace PseudoCQRS.Mvc
{
	public class PseudoCQRSServiceLocator : IPseudoCQRSServiceLocator
	{
		public object GetInstance( Type type ) => ServiceLocator.Current.GetInstance( type );
	}
}
