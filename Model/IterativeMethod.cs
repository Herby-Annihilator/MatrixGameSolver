using MatrixGameSolver.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MatrixGameSolver.Model
{
	public class IterativeMethod
	{
		private int _maxStepsCount;
		private double _precision;
		private double[][] _matrix;
		public IterativeMethod(double[][] matrix, double precision, int maxStepsCount = int.MaxValue)
		{
			_precision = precision;
			_maxStepsCount = maxStepsCount;
			_matrix = matrix;
		}
		public IterativeMethodAnswer Solve()
		{
			int currentIteration = 0;

			int currentAStrategy = FindRowWithMaxElement();
			int[] playerAStrategiesUsed = new int[_matrix.GetLength(0)];			
			double[] accumulatedWinningsOfPlayerA = new double[_matrix[0].Length]; // выигрыши игрока А при конкрентной стратегии, размерность - число столбцов в матрице (стратки В)

			int currentBStrategy;
			int[] playerBStrategiesUsed = new int[_matrix[0].Length];
			double[] accumulatedLossOfPlayerB = new double[_matrix.GetLength(0)]; // проигрыши игрока В при конкретной стратегии, размерность - число строк в матрице (стратки А)

			double minGamePrice = 0, maxGamePrice = 0;
			double currentGamePrice = double.MaxValue;

			while (!IsPrecisionAchieved(minGamePrice, maxGamePrice, currentGamePrice) && (currentIteration < _maxStepsCount))
			{
				currentIteration++;

				playerAStrategiesUsed[currentAStrategy]++;
				accumulatedWinningsOfPlayerA.AddSpecifiedRow(_matrix[currentAStrategy]);
				minGamePrice = accumulatedWinningsOfPlayerA.Min();

				currentBStrategy = accumulatedWinningsOfPlayerA.IndexOf(minGamePrice);
				playerBStrategiesUsed[currentBStrategy]++;
				accumulatedLossOfPlayerB.AddSpecifiedColumnOfMatrix(_matrix, currentBStrategy);
				maxGamePrice = accumulatedLossOfPlayerB.Max();

				currentAStrategy = accumulatedLossOfPlayerB.IndexOf(maxGamePrice);

				minGamePrice /= currentIteration;
				maxGamePrice /= currentIteration;

				currentGamePrice = (minGamePrice + maxGamePrice) / 2;
			}

			IterativeMethodAnswer answer = new IterativeMethodAnswer(currentIteration, currentGamePrice);
			for (int i = 0; i < playerAStrategiesUsed.Length; i++)
			{
				answer.LikelihoodsA.Add(new StrategyLikelihood($"A{i + 1}", ((double)playerAStrategiesUsed[i]) / currentIteration));
			}
			for (int i = 0; i < playerBStrategiesUsed.Length; i++)
			{
				answer.LikelihoodsB.Add(new StrategyLikelihood($"B{i + 1}", ((double)playerBStrategiesUsed[i]) / currentIteration));
			}
			return answer;
		}

		private bool IsPrecisionAchieved(double minGamePrice, double maxGamePrice, double currentGamePrice) => ((currentGamePrice - minGamePrice) <= _precision) && ((maxGamePrice - currentGamePrice) <= _precision);
		private int FindRowWithMaxElement()
		{
			int index = 0;
			int currentIndexOfMaxElementInRow = FindIndexOfMaxElementInSpecifiedRow(0);
			double max = _matrix[index][currentIndexOfMaxElementInRow];
			for (int i = 1; i < _matrix.GetLength(0); i++)
			{
				currentIndexOfMaxElementInRow = FindIndexOfMaxElementInSpecifiedRow(i);
				if (_matrix[i][currentIndexOfMaxElementInRow] > max)
				{
					max = _matrix[i][currentIndexOfMaxElementInRow];
					index = i;
				}
			}
			return index;
		}

		private int FindIndexOfMaxElementInSpecifiedRow(int rowIndex)
		{
			double max = _matrix[rowIndex][0];
			int index = 0;
			for (int i = 1; i < _matrix[rowIndex].Length; i++)
			{
				if (_matrix[rowIndex][i] > max)
				{
					max = _matrix[rowIndex][i];
					index = i;
				}
			}
			return index;
		}

	}

	public class IterativeMethodAnswer
	{
		public int IterationsCount { get; }
		public double GamePrice { get; }

		public List<StrategyLikelihood> LikelihoodsA { get; set; }
		public List<StrategyLikelihood> LikelihoodsB { get; set; }

		public IterativeMethodAnswer(int iterationsCount, double gamePrice)
		{
			IterationsCount = iterationsCount;
			GamePrice = gamePrice;
			LikelihoodsA = new List<StrategyLikelihood>();
			LikelihoodsB = new List<StrategyLikelihood>();
		}
	}
}
