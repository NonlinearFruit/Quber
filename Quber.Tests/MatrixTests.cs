using System;
using Quber;
using Xunit;

namespace Quber.Tests
{
    public class MatrixTests
    {
        [Fact]
        public void Multiplication_MultipliesMatricesOfSameSize()
        {
            var size = 2;
            var A = new Matrix(size,size);
            A[0, 0] = 1;
            A[0, 1] = 2;
            A[1, 0] = 4;
            A[1, 1] = 8;
            var B = new Matrix(size, size);
            B[0, 1] = 1;
            B[1, 0] = 1;

            var C = A * B;

            VerifyIsProductOf(A, B, C);
        }

        private static void VerifyIsProductOf(Matrix A, int scalar, Matrix C)
        {
            for (int row = 0; row < A.Rows; row++)
                for (int column = 0; column < A.Columns; column++)
                {
                    var cellValue = A[row, column] * scalar;
                    Assert.Equal(cellValue, C[row, column]);
                }
        }

        private static void VerifyIsProductOf(Matrix A, Matrix B, Matrix C)
        {
            for (int row = 0; row < A.Rows; row++)
                for (int column = 0; column < B.Columns; column++)
                {
                    var cellValue = 0;
                    for (int k = 0; k < A.Rows; k++)
                        cellValue += A[row, k] * B[k, column];
                    Assert.Equal(cellValue, C[row, column]);
                }
        }

        [Fact]
        public void Multiplication_MultipliesMatrixAndScalar()
        {
            var size = 2;
            var A = new Matrix(size,size);
            A[0, 0] = 1;
            A[0, 1] = 2;
            A[1, 0] = 4;
            A[1, 1] = 8;

            var scalar = 3;

            var C = A * scalar;

            VerifyIsProductOf(A, scalar, C);
        }

        [Fact]
        public void Multiplication_MultipliesMatricesOfDifferentSize()
        {
            var size = 2;
            var A = new Matrix(size,size);
            A[0, 0] = 1;
            A[0, 1] = 2;
            A[1, 0] = 4;
            A[1, 1] = 8;
            var B = new Matrix(size, size+1);
            B[0, 1] = 1;
            B[1, 0] = 1;

            var C = A * B;

            VerifyIsProductOf(A, B, C);
        }

        [Fact]
        public void Multiplication_WhenRowsDoNotMatchColumns_ThrowsException()
        {
            var size = 2;
            var A = new Matrix(size, size);
            var B = new Matrix(size+1,size);

            Assert.Throws<ArgumentException>(() => A * B);
        }
    }
}
