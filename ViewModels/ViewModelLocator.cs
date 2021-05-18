using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixGameSolver.ViewModels
{
	public class ViewModelLocator
	{
		public MainWindowViewModel MainWindowViewModel =>
			App.Host.Services.GetRequiredService<MainWindowViewModel>();
	}
}
