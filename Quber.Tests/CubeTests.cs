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

            for (int i = 0; i < faces.Length; i++)
            {
                var face = faces[i];
                var color = faces[(i + 1) % faces.Length];
                VerifyFaceHasSoManyOriginalColors(face, 6);
                VerifyFaceHasSoManyOfAColor(face, 3, color);
            }
        }

        [Theory]
        [MemberData(nameof(CubeRotations))]
        public void RotateString_HandlesCubeRotations(string shorthand, Face newUp, Face newFront)
        {
            _cube.Rotate(shorthand);

            VerifyFaceHasSoManyOfAColor(Face.Up, 9, newUp);
            VerifyFaceHasSoManyOfAColor(Face.Front, 9, newFront);
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
