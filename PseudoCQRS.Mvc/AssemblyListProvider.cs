using System;
using System.Collections.Generic;
using System.Reflection;

namespace PseudoCQRS
{
	public class AssemblyListProvider : IAssemblyListProvider
	{
		public IEnumerable<Assembly> GetAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}
	}
}