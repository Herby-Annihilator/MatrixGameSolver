using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixGameSolver.Model
{
	public static class DoubleMatrixExtensions
	{
		public static int IndexOf(this double[] arr, double element)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				if (arr[i] == element)
					return i;
			}
			return -1;
		}

		public static void AddSpecifiedRow(this double[] arr, double[] row)
		{
			int size = GetMinSize(arr, row);
			for (int i = 0; i < size; i++)
			{
				arr[i] += row[i];
			}
		}

		public static void AddSpecifiedColumnOfMatrix(this double[] arr, double[][] matrix, int colIndex)
		{
			int size;
			if (arr.Length < matrix.GetLength(0))
				size = arr.Length;
			else
				size = matrix.GetLength(0);
			for (int i = 0; i < size; i++)
			{
				arr[i] += matrix[i][colIndex];
			}
		}

		private static int GetMinSize(double[] first, double[] second)
		{
			int size;
			if (first.Length < second.Length)
				size = first.Length;
			else
				size = second.Length;
			return size;
		}
	}
}
