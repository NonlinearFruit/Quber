using System.Linq;
using Xunit;

namespace Quber.Tests
{
    public class CubeTests
    {
        private Cube _cube;

        public CubeTests()
        {
            _cube = new Cube();
        }

        [Fact]
        public void Constructor_CreatesFaces()
        {
            foreach (var face in Face.Options)
                VerifyFaceHasSoManyOriginalColors(face, 9);
        }

        [Fact]
        public void Rotate_RotatesFace()
        {
            _cube.Rotate(Face.Up, Rotation.Type.Double);

            var pieces = _cube.Pieces.Where(p => p.Position[0, 0] == 1 && p.Position[2, 0] == 1);
            Assert.Equal(3, pieces.Count());
            foreach (var piece in pieces)
                Assert.Equal(Face.Back, piece.ColorX);
        }

        [Fact]
        public void GetPiece_GetsPiece()
        {
            var piece = _cube[0, 0, 1];

            Assert.NotNull(piece);
            Assert.Equal(Piece.Type.Center, piece.GetType());
            Assert.Equal(Face.Up, piece.ColorZ);
        }

        [Fact]
        public void GetFace_GetsFace()
        {
            var pieces = _cube[Face.Up];

            Assert.Equal(9, pieces.Count);
            foreach (var piece in pieces)
                Assert.Equal(1, piece.Position[2,0]);
        }

        [Fact]
        public void RotateString_RotatesFace()
        {
            _cube.Rotate("U");

            VerifyFaceHasSoManyOriginalColors(Face.Back, 6);
        }

        [Fact]
        public void RotateString_RotatesMultipleFaces()
        {
            _cube.Rotate("U2 D2 R2 L2 F2 B2");

            foreach (var face in Face.Options)
                VerifyFaceHasSoManyOriginalColors(face, 5);
        }

        [Fact]
        public void RotateString_HandlesNotation()
        {
            _cube.Rotate("M"); // MES udrlfb XYZ 2F 3F NF

            var faces = new[] {Face.Front, Face.Up, Face.Back, Face.Down};
            for (int i = 0; i < faces.Length; i++)
            {
                var face = faces[i];
                var color = faces[(i + 1) % faces.Length];
                VerifyFaceHasSoManyOriginalColors(face, 6);
                VerifyFaceHasSoManyOfAColor(face, 3, color);
            }
        }

        private void VerifyFaceHasSoManyOriginalColors(Face face, int count)
        {
            VerifyFaceHasSoManyOfAColor(face, count, face);
        }
        private void VerifyFaceHasSoManyOfAColor(Face face, int count, Face color)
        {
            var pieces = _cube[face];
            var found = 0;
            foreach (var piece in pieces)
                if (face.GetColor(piece) == color)
                    found++;
         
            Assert.Equal(count, found);
        }
    }
}
