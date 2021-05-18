using MatrixGameSolver.Infrastructure.Commands;
using MatrixGameSolver.Model.Data;
using MatrixGameSolver.Model;
using MatrixGameSolver.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Markup;

namespace MatrixGameSolver.ViewModels
{
	[MarkupExtensionReturnType(typeof(MainWindowViewModel))]
	public class MainWindowViewModel : ViewModel
	{

		public MainWindowViewModel()
		{
			GetSolutionCommand = new LambdaCommand(OnGetSolutionCommandExecuted, CanGetSolutionCommandExecute);
		}
		#region Properties
		private string _title = "Title";
		public string Title { get => _title; set => Set(ref _title, value); }

		private string _status = "Итерационный метод решения матричной игры";
		public string Status { get => _status; set => Set(ref _status, value); }

		private string _paymentMatrix;
		public string PaymentMatrix { get => _paymentMatrix; set => Set(ref _paymentMatrix, value); }

		private string _precision;
		public string Precision { get => _precision; set => Set(ref _precision, value); }

		private string _maxStepsCount;
		public string MaxStepsCount { get => _maxStepsCount; set => Set(ref _maxStepsCount, value); }

		public ObservableCollection<StrategyLikelihood> TableA { get; set; } = new ObservableCollection<StrategyLikelihood>();
		public ObservableCollection<StrategyLikelihood> TableB { get; set; } = new ObservableCollection<StrategyLikelihood>();

		private string _gamePrice = "Не найдено";
		public string GamePrice { get => _gamePrice; set => Set(ref _gamePrice, value); }

		private string _stepsCount = "Не найдено";
		public string StepsCount { get => _stepsCount; set => Set(ref _stepsCount, value); }
		#endregion

		#region Commands
		public ICommand GetSolutionCommand { get; }
		private void OnGetSolutionCommandExecuted(object p)
		{
			try
			{
				TableA.Clear();
				TableB.Clear();
				GamePrice = "Не найдено";
				StepsCount = "Не найдено";
				double precision = Convert.ToDouble(Precision);
				int maxStepsCount = int.MaxValue;
				if (!string.IsNullOrWhiteSpace(MaxStepsCount))
				{
					maxStepsCount = Convert.ToInt32(MaxStepsCount);
				}
				double[][] paymentMatrix = GetEquivalentMatrix(PaymentMatrix);
				IterativeMethod method = new IterativeMethod(paymentMatrix, precision, maxStepsCount);
				var answer = method.Solve();
				GamePrice = answer.GamePrice.ToString();
				StepsCount = answer.IterationsCount.ToString();
				for (int i = 0; i < answer.LikelihoodsA.Count; i++)
				{
					TableA.Add(answer.LikelihoodsA[i]);
				}
				for (int i = 0; i < answer.LikelihoodsB.Count; i++)
				{
					TableB.Add(answer.LikelihoodsB[i]);
				}
				Status = "Успешное выполнение";
			}
			catch(Exception e)
			{
				Status = $"Операция не удалась. Причина {e.Message}";
			}
		}
		private bool CanGetSolutionCommandExecute(object p) => !(string.IsNullOrWhiteSpace(PaymentMatrix) || string.IsNullOrWhiteSpace(Precision));
		#endregion

		private double[][] GetEquivalentMatrix(string matrStr)
		{
			double[][] matrix;
			string[] rows = matrStr.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
			string[] numbers;
			matrix = new double[rows.Length][];
			for (int i = 0; i < rows.Length; i++)
			{
				numbers = rows[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
				matrix[i] = new double[numbers.Length];
				for (int j = 0; j < numbers.Length; j++)
				{
					matrix[i][j] = Convert.ToDouble(numbers[j]);
				}
			}
			return matrix;
		}
	}
}
