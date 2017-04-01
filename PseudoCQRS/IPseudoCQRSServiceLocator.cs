using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PseudoCQRS
{
	public interface IPseudoCQRSServiceLocator
	{
		object GetInstance( Type type );
	}
}
