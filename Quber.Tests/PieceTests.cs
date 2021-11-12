using Xunit;

namespace Quber.Tests
{
    public class PieceTests
    {
        private Matrix _position;
        private Piece _piece;
        private Face _colorX;
        private Face _colorY;
        private Face _colorZ;

        public PieceTests()
        {
            _position = new Matrix(3, 1)
            {
                [0, 0] = 1,
                [1, 0] = 1,
                [2, 0] = 1
            };
            _colorX = Face.Up;
            _colorY = Face.Right;
            _colorZ = Face.Front;
            _piece = new Piece(_position, _colorX, _colorY, _colorZ);
        }

        [Fact]
        public void Rotate_RotatesPosition()
        {
            var rotation = new Rotation(Face.Up, Rotation.Type.Clockwise);

            _piece.Rotate(rotation);

            VerifyValuesEqualPosition(new []{1, -1, 1}, _piece);
        }

        [Fact]
        public void Rotate_RotatesColor()
        {
            var rotation = new Rotation(Face.Up, Rotation.Type.Clockwise);

            _piece.Rotate(rotation);

            Assert.Equal(_colorX, _piece.ColorY);
            Assert.Equal(_colorY, _piece.ColorX);
            Assert.Equal(_colorZ, _piece.ColorZ);
        }

        [Theory]
        [InlineData(1, 1, 1, Piece.Type.Corner)]
        [InlineData(1, 0, 0, Piece.Type.Center)]
        [InlineData(1, 1, 0, Piece.Type.Edge)]
        public void GetType_GetsType(int x, int y, int z, Piece.Type expectedType)
        {
            var position = new Matrix(3, 1)
            {
                [0, 0] = x,
                [1, 0] = y,
                [2, 0] = z
            };
            var piece = new Piece(position, Face.Up, Face.Right, Face.Front);

            var actualType = piece.GetType();

            Assert.Equal(expectedType, actualType);
        }


        private void VerifyValuesEqualPosition(int[] values, Piece piece)
        {
            Assert.NotNull(piece);
            Assert.Equal(values[0], piece.X);
            Assert.Equal(values[1], piece.Y);
            Assert.Equal(values[2], piece.Z);
        }

    }
}
