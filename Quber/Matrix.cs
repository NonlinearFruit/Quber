using System;

namespace Quber
{
    public class Matrix
    {
        public int Rows { get; }

        public int Columns { get; }

        public int this[int i, int j]
        {
            get => GetValue(i, j);
            set => SetValue(i, j, value);
        }

        private readonly int[,] _values;

        private void SetValue(int i, int j, int value)
        {
            _values[i, j] = value;
        }

        private int GetValue(int i, int j)
        {
            return _values[i, j];
        }

        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _values = new int[Rows,Columns];
        }

        public static Matrix operator *(Matrix a, int scalar)
        {
            var c = new Matrix(a.Rows, a.Columns);
            for (var i = 0; i < c.Rows; i++)
            for (var j = 0; j < c.Columns; j++)
                c[i, j] = a[i, j] * scalar;

            return c;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Columns != b.Rows)
                throw new ArgumentException("Columns of matrix A must equal Rows of matrix B");

            var c = new Matrix(a.Rows, b.Columns);
            for (var i = 0; i < c.Rows; i++)
            for (var j = 0; j < c.Columns; j++)
            for (var k = 0; k < a.Columns; k++)
                c[i, j] = c[i, j] + a[i, k] * b[k, j];

            return c;
        }
    }
}