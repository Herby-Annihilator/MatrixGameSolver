using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixGameSolver.Model.Data
{
	public class StrategyLikelihood
	{
		public string Name { get; set; }
		public double Probability { get; set; }

		public StrategyLikelihood(string name, double probability)
		{
			Name = name;
			Probability = probability;
		}
	}
}
