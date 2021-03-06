﻿namespace PseudoCQRS
{
	public interface IViewModelProviderArgumentsProvider
	{
		TArg GetArguments<TArg>() where TArg : new();
		void Persist<TArg>() where TArg : new();
	}
}