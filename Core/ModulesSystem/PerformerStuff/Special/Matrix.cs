using System;
using System.Text;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.Special
{
    
    public class Matrix
    {
        public Matrix(Matrix m)
        {
            this.Innerfloats = (float[,])m.Innerfloats.Clone();
            this.RowsCount = m.RowsCount;
            this.ColumnsCount = m.ColumnsCount;
        }
        public Matrix(float[,] from)
        {
            var rows = from.GetLength(0);
            var cols = from.GetLength(1);
            Innerfloats = (float[,])from.Clone();
            RowsCount = rows;
            ColumnsCount = cols;
        }
        public float[,] Innerfloats { get;}
        public int RowsCount { get;}
        public int ColumnsCount { get;}
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
                    sb.Append($"{Innerfloats[i, j],6:F1}");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
        public static Matrix CreateDiagonalic(Func<int, float[]> seriesGenerator, int rows, int cols)
        {
            var seriesLength = Math.Max(rows, cols);//ex: 100 90 80 70 60 50 40 30 20 10
            var series = seriesGenerator.Invoke(seriesLength);
            var raw = new float[rows, cols];
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
                        if(j<rows && i<cols)
                            raw[i, j] = raw[j, i];
                    }
                }
            }

            return new Matrix(raw);
        }
        public static Matrix operator *(Matrix matrix, float coeff)
        {
            var m = new Matrix(matrix);
            for (int i = 0; i < m.RowsCount; i++)
            {
                for (int j = 0; j < m.ColumnsCount; j++)
                {
                    m.Innerfloats[i, j] *= coeff;
                }
            }
            return m;
        }
        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.RowsCount == matrix2.RowsCount && matrix2.ColumnsCount == matrix1.ColumnsCount)
            {
                for (var i = 0; i < matrix1.RowsCount; i++)
                {
                    for (var j = 0; j < matrix1.ColumnsCount; j++)
                    {
                        matrix1.Innerfloats[i, j] *= matrix2.Innerfloats[i, j];
                    }
                }
                return matrix1;
            }
            else
            {
                throw new Exception("Поэлементное умножение матриц различных размеров недопустимо");
            }

        }
        public static readonly Func<int, float[]> DefaultRegression = new Func<int, float[]>((count) =>
        {
            var floats = new float[count];
            for (var i = 0; i < count; i++)
            {
                floats[i] = ((float)count - (float)i) / (float)count;
            }
            return floats;
        });
        public static readonly Func<int, float[]> ExponentialRegression = new((count) =>
        {
            var floats = new float[count];
            for (var i = 0; i < count; i++)
            {
                floats[i] = MathF.Pow(MathF.E, -0.09f * ((float)i - 10f)) * 0.40657f;//e^{-0.09\left(x-10\right)}\cdot40.657
            }
            return floats;
        });
        public static float GetMinForExponentialDiagonalic(Matrix m)
        {
            return m.Innerfloats[0, m.ColumnsCount - 1];
        }
        public static float GetMaxForExponentialDiagonalic(Matrix m)
        {
            return m.Innerfloats[0, 0];
        }
    }
}
