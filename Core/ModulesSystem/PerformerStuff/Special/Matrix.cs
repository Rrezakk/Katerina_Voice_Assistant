using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.Special
{
    public class Matrix
    {
        public Matrix(Matrix m)
        {
            this.InnerDoubles = (double[,])m.InnerDoubles.Clone();
            this.RowsCount = m.RowsCount;
            this.ColumnsCount = m.ColumnsCount;
        }
        public Matrix(double[,] from)
        {
            var rows = from.GetLength(0);
            var cols = from.GetLength(1);
            InnerDoubles = (double[,])from.Clone();
            RowsCount = rows;
            ColumnsCount = cols;
        }
        public double[,] InnerDoubles { get; set; }
        public int RowsCount { get; set; }
        public int ColumnsCount { get; set; }
        public bool IsSquare()
        {
            return ColumnsCount == RowsCount;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("----------Matrix--------\n");
            for (var i = 0; i < RowsCount; i++)
            {
                for (var j = 0; j < ColumnsCount; j++)
                {
                    sb.Append($"{InnerDoubles[i, j],6:F1}");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
        public static Matrix CreateDiagonalic(Func<int, double[]> seriesGenerator, int rows, int cols)
        {
            var seriesLength = Math.Max(rows, cols);//ex: 100 90 80 70 60 50 40 30 20 10
            var series = seriesGenerator.Invoke(seriesLength);
            var raw = new double[rows, cols];
            for (var i = rows - 1; i >= 0; i--)
            {
                for (var j = cols - 1; j >= 0; j--)
                {
                    if (i == j)
                    {
                        raw[i, j] = series[0];
                    }
                    else if (i - j > 0)
                    {
                        raw[i, j] = series[i - j];
                    }
                    else
                    {
                        raw[i, j] = raw[j, i];
                    }
                }
            }

            return new Matrix(raw);
        }
        public static Matrix operator *(Matrix matrix, double coeff)
        {
            var m = new Matrix(matrix);
            for (int i = 0; i < m.RowsCount; i++)
            {
                for (int j = 0; j < m.ColumnsCount; j++)
                {
                    m.InnerDoubles[i, j] *= coeff;
                }
            }
            return m;
        }
        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.RowsCount == matrix2.RowsCount && matrix2.ColumnsCount == matrix1.ColumnsCount)
            {
                for (int i = 0; i < matrix1.RowsCount; i++)
                {
                    for (int j = 0; j < matrix1.ColumnsCount; j++)
                    {
                        matrix1.InnerDoubles[i, j] *= matrix2.InnerDoubles[i, j];
                    }
                }
                return matrix1;
            }
            else
            {
                throw new Exception("Поэлементное умножение матриц различных размеров недопустимо");
            }

        }
        public static Func<int, double[]> DefaultRegression = new Func<int, double[]>((count) =>
        {
            var doubles = new double[count];
            for (var i = 0; i < count; i++)
            {
                doubles[i] = ((double)count - (double)i) / (double)count;
            }
            return doubles;
        });
        public static Func<int, double[]> ExponentialRegression = new Func<int, double[]>((count) =>
        {
            var doubles = new double[count];
            for (var i = 0; i < count; i++)
            {
                doubles[i] = Math.Pow(Math.E, -0.09d * ((double)i - 10d)) * 0.40657d;//e^{-0.09\left(x-10\right)}\cdot40.657
            }
            return doubles;
        });
        public static double GetMinForExponentialDiagonalic(Matrix m)
        {
            return m.InnerDoubles[0, m.ColumnsCount - 1];
        }
        public static double GetMaxForExponentialDiagonalic(Matrix m)
        {
            return m.InnerDoubles[0, 0];
        }
    }
}
