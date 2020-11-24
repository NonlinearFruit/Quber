using System.Collections.Generic;
using Xunit;

namespace Quber.Tests
{
    public class RotationTests
    {
        private static readonly int[][] UpperPositions =
            {new[] {1, 1, 1}, new[] {1, -1, 1}, new[] {-1, -1, 1}, new[] {-1, 1, 1}};
        private static readonly int[][] DownPositions =
            {new[] {-1, 1, -1}, new[] {-1, -1, -1}, new[] {1, -1, -1}, new[] {1, 1, -1}};

        private static readonly int[][] RightPositions =
            {new[] {1, 1, 1}, new[] {-1, 1, 1}, new[] {-1, 1, -1}, new[] {1, 1, -1}};
        private static readonly int[][] LeftPositions =
            {new[] {1, -1, -1}, new[] {-1, -1, -1}, new[] {-1, -1, 1}, new[] {1, -1, 1}};

        private static readonly int[][] FrontPositions =
            {new[] {1, 1, 1}, new[] {1, 1, -1 }, new[] {1, -1, -1}, new[] {1, -1, 1}};
        private static readonly int[][] BackPositions =
            { new[] {-1, -1, 1}, new[] {-1, -1, -1}, new[] {-1, 1, -1 }, new[] {-1, 1, 1}};

        public static IEnumerable<object[]> ClockwiseRotationSequences => new List<object[]>
        {
            new object[]{new Rotation(Face.Up, Rotation.Type.Clockwise), UpperPositions},
            new object[]{new Rotation(Face.Right, Rotation.Type.Clockwise), RightPositions},
            new object[]{new Rotation(Face.Front, Rotation.Type.Clockwise), FrontPositions},
            new object[]{new Rotation(Face.Down, Rotation.Type.Clockwise), DownPositions},
            new object[]{new Rotation(Face.Left, Rotation.Type.Clockwise), LeftPositions},
            new object[]{new Rotation(Face.Back, Rotation.Type.Clockwise), BackPositions},
        };

        public static IEnumerable<object[]> WiddershinsRotationSequences => new List<object[]>
        {
            new object[]{new Rotation(Face.Up, Rotation.Type.Widdershins), UpperPositions},
            new object[]{new Rotation(Face.Right, Rotation.Type.Widdershins), RightPositions},
            new object[]{new Rotation(Face.Front, Rotation.Type.Widdershins), FrontPositions},
            new object[]{new Rotation(Face.Down, Rotation.Type.Widdershins), DownPositions},
            new object[]{new Rotation(Face.Left, Rotation.Type.Widdershins), LeftPositions},
            new object[]{new Rotation(Face.Back, Rotation.Type.Widdershins), BackPositions},
        };

        public static IEnumerable<object[]> DoubleRotationSequences => new List<object[]>
        {
            new object[]{new Rotation(Face.Up, Rotation.Type.Double), UpperPositions},
            new object[]{new Rotation(Face.Right, Rotation.Type.Double), RightPositions},
            new object[]{new Rotation(Face.Front, Rotation.Type.Double), FrontPositions},
            new object[]{new Rotation(Face.Down, Rotation.Type.Double), DownPositions},
            new object[]{new Rotation(Face.Left, Rotation.Type.Double), LeftPositions},
            new object[]{new Rotation(Face.Back, Rotation.Type.Double), BackPositions},
        };

        [Theory]
        [MemberData(nameof(ClockwiseRotationSequences))]
        public void ClockwiseRotations_RotateClockwise(Rotation rotation, int[][] valuesList)
        {
            for (var i = 0; i < valuesList.Length; i++)
            {
                var values = valuesList[i];
                var nextValues = valuesList[(i + 1) % valuesList.Length];
                var position = GetPosition(values);

                var result = rotation.Rotate(position);

                VerifyValuesEqualPosition(nextValues, result);
            }
        }

        [Theory]
        [MemberData(nameof(WiddershinsRotationSequences))]
        public void WiddershinsRotations_RotateWiddershins(Rotation rotation, int[][] valuesList)
        {
            for (var i = 0; i < valuesList.Length; i++)
            {
                var values = valuesList[i];
                var nextValues = valuesList[(valuesList.Length + i - 1) % valuesList.Length];
                var position = GetPosition(values);

                var result = rotation.Rotate(position);

                VerifyValuesEqualPosition(nextValues, result);
            }
        }

        [Theory]
        [MemberData(nameof(DoubleRotationSequences))]
        public void DoubleRotations_RotatesTwice(Rotation rotation, int[][] valuesList)
        {
            for (var i = 0; i < valuesList.Length; i++)
            {
                var values = valuesList[i];
                var nextValues = valuesList[(i + 2) % valuesList.Length];
                var position = GetPosition(values);

                var result = rotation.Rotate(position);

                VerifyValuesEqualPosition(nextValues, result);
            }
        }

        [Theory]
        [InlineData("U", Rotation.Type.Clockwise)]
        [InlineData("U'", Rotation.Type.Widdershins)]
        [InlineData("U2", Rotation.Type.Double)]
        public void GetType_GetsType(string abbreviation, Rotation.Type type)
        {
            var resultType = Rotation.GetType(abbreviation);

            Assert.Equal(type, resultType);
        }

        private void VerifyValuesEqualPosition(int[] values, Matrix position)
        {
            Assert.NotNull(position);
            Assert.Equal(values[0], position[0,0]);
            Assert.Equal(values[1], position[1,0]);
            Assert.Equal(values[2], position[2,0]);
        }

        private Matrix GetPosition(int[] values)
        {
            var position = new Matrix(3, 1)
            {
                [0, 0] = values[0],
                [1, 0] = values[1],
                [2, 0] = values[2]
            };
            return position;
        }
    }
}
