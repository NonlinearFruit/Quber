using System.Collections.Generic;
using Xunit;

namespace Quber.Tests
{
    public class FaceTests
    {

        public static IEnumerable<object[]> ContainablePieces
        {
            get
            {
                var FR3x3 = new Piece(1, 1, 0);
                var R5x5 = new Piece(1, 2, 0);
                var L5x5 = new Piece(-1,-2,0);

                return new List<object[]>
                {
                    new object[]{Face.Front, FR3x3, true},
                    new object[]{Face.Left, FR3x3, false},
                    new object[]{Face.Front, R5x5, false},
                    new object[]{Face.Left, R5x5, false},
                    new object[]{Face.Front, L5x5, false},
                    new object[]{Face.Left, L5x5, true},
                };
            }
        }

        [Theory]
        [MemberData(nameof(ContainablePieces))]
        public void Contains(Face face, Piece piece, bool shouldContain)
        {
            Assert.Equal(shouldContain, face.Contains(piece));
        }
    }
}
