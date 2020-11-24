using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Quber.Tests
{
    public class CubeTests
    {// UDRLFB MES udrlfb XYZ 2F 3F NF 2Fw 3Fw NFw
        private Cube _cube;

        public static IEnumerable<object[]> SectionRotations => new List<object[]>
        {
            new object[]{"M", new[] {Face.Front, Face.Up, Face.Back, Face.Down}},
            new object[]{"E", new[] {Face.Front, Face.Left, Face.Back, Face.Right}},
            new object[]{"S", new[] {Face.Up, Face.Left, Face.Down, Face.Right}},
            new object[]{"u", new[] {Face.Front, Face.Right, Face.Back, Face.Left}},
            new object[]{"d", new[] {Face.Front, Face.Left, Face.Back, Face.Right}},
            new object[]{"r", new[] {Face.Front, Face.Down, Face.Back, Face.Up}},
            new object[]{"l", new[] {Face.Front, Face.Up, Face.Back, Face.Down}},
            new object[]{"f", new[] {Face.Up, Face.Left, Face.Down, Face.Right}},
            new object[]{"b", new[] {Face.Up, Face.Right, Face.Down, Face.Left}},
        };

        public static IEnumerable<object[]> SamplePieces => new List<object[]>
        {
            new object[]{"M", new[] {0,0,1}},
            new object[]{"E", new[] {1,0,0}},
            new object[]{"S", new[] {0,0,1}},
            new object[]{"u", new[] {1,0,0}},
            new object[]{"d", new[] {1,0,0}},
            new object[]{"r", new[] {1,0,0}},
            new object[]{"l", new[] {1,0,0}},
            new object[]{"f", new[] {0,0,1}},
            new object[]{"b", new[] {0,0,1}},
        };

        public static IEnumerable<object[]> CubeRotations => new List<object[]>
        {
            new object[]{"X", Face.Front, Face.Down},
            new object[]{"Y", Face.Left, Face.Front},
            new object[]{"Z", Face.Up, Face.Right},
        };

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

            var pieces = _cube.Pieces.Where(p => p.X == 1 && p.Z == 1);
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
                Assert.Equal(1, piece.Z);
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

        [Theory]
        [MemberData(nameof(SectionRotations))]
        public void RotateString_HandlesSingleSectionRotations(string shorthand, Face[] faces)
        {
            _cube.Rotate(shorthand);

            for (var i = 0; i < faces.Length; i++)
            {
                var face = faces[i];
                var color = faces[(i + 1) % faces.Length];
                VerifyFaceHasSoManyOriginalColors(face, 6);
                VerifyFaceHasSoManyOfAColor(face, 3, color);
            }
        }

        [Theory]
        [MemberData(nameof(SamplePieces))]
        public void RotateString_AffectsProperPiece(string shorthand, int[] pieceValues)
        {
            var piece = _cube[pieceValues[0], pieceValues[1], pieceValues[2]];
            var originalColor = piece.ColorX ?? piece.ColorY ?? piece.ColorZ;

            _cube.Rotate(shorthand);

            var newPiece = _cube[pieceValues[0], pieceValues[1], pieceValues[2]];
            var newColor = newPiece.ColorX ?? newPiece.ColorY ?? newPiece.ColorZ;
            Assert.NotEqual(originalColor, newColor);
        }

        [Theory]
        [MemberData(nameof(CubeRotations))]
        public void RotateString_HandlesCubeRotations(string shorthand, Face newUp, Face newFront)
        {
            _cube.Rotate(shorthand);

            VerifyFaceHasSoManyOfAColor(Face.Up, 9, newUp);
            VerifyFaceHasSoManyOfAColor(Face.Front, 9, newFront);
        }

        [Fact]
        public void RotateString_HandlesBigCubeNotation()
        {
            var size = 3;
            var depth = 1;
            var face = "F";
            var layer = size / 2 - depth;
            var shorthand = $"{depth+1}{face}";

            _cube.Rotate(shorthand);

            var rotatedPieces = _cube[Face.Up].Where(p => p.X == layer).ToList();
            Assert.Equal(3, rotatedPieces.Count);
            var unrotatedPieces = _cube[Face.Up].Where(p => p.X != layer).ToList();
            Assert.Equal(6, unrotatedPieces.Count);
            foreach (var piece in rotatedPieces)
                Assert.Equal(Face.Left, piece.ColorZ);
            foreach (var piece in unrotatedPieces)
                Assert.Equal(Face.Up, piece.ColorZ);
        }

        [Fact]
        public void RotateString_HandlesWideRotations()
        {
            var size = 3;
            var depth = 1;
            var face = "F";
            var layer = size / 2 - depth;
            var shorthand = $"{depth+1}{face}w";

            _cube.Rotate(shorthand);

            var rotatedPieces = _cube[Face.Up].Where(p => p.X >= layer).ToList();
            Assert.Equal(size*(depth+1), rotatedPieces.Count);
            var unrotatedPieces = _cube[Face.Up].Where(p => p.X < layer).ToList();
            Assert.Equal(size*size - size*(depth+1), unrotatedPieces.Count);
            foreach (var piece in rotatedPieces)
                Assert.Equal(Face.Left, piece.ColorZ);
            foreach (var piece in unrotatedPieces)
                Assert.Equal(Face.Up, piece.ColorZ);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Constructor_NxN(int N)
        {
            var size = N;
            _cube = new Cube(size);

            Assert.Equal(6*size*size - 12*size + 8, _cube.Pieces.Count);
            Assert.Equal(size*size, _cube[Face.Up].Count);
        }

        private void VerifyFaceHasSoManyOriginalColors(Face face, int count)
        {
            VerifyFaceHasSoManyOfAColor(face, count, face);
        }
        private void VerifyFaceHasSoManyOfAColor(Face face, int count, Face color)
        {
            var pieces = _cube[face];
            var found = pieces.Count(piece => face.GetColor(piece) == color);
            Assert.Equal(count, found);
        }
    }
}
