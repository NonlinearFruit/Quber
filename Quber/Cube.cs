using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Quber
{
    public class Cube
    {
        public IList<Piece> this[Face face] => GetPieces(face);
        public Piece this[int x, int y, int z] => GetPiece(x, y, z);

        public IList<Piece> Pieces { get; }
        public int Size { get; }
        public int MaxLayer { get; }

        public Cube() : this(3)
        {
        }

        public Cube(int size)
        {
            Size = size;
            Pieces = new List<Piece>();
            MaxLayer = Size / 2;
            CreatePieces(Pieces);
        }

        private void CreatePieces(IList<Piece> pieces)
        {
            for (var i = -MaxLayer; i <= MaxLayer; i++)
                for (var j = -MaxLayer; j <= MaxLayer; j++)
                    for (var k = -MaxLayer; k <= MaxLayer; k++)
                        if (Math.Abs(i) == MaxLayer || Math.Abs(j) == MaxLayer || Math.Abs(k) == MaxLayer)
                            if (Size%2 == 1 || (i != 0 && j != 0 && k != 0))
                                pieces.Add(CreatePiece(i,j,k));
                
        }

        private Piece CreatePiece(int i, int j, int k)
        {
            var position = new Matrix(3, 1)
            {
                [0, 0] = i,
                [1, 0] = j,
                [2, 0] = k
            };

            var colorX = GetColorX(i);
            var colorY = GetColorY(j);
            var colorZ = GetColorZ(k);

            return new Piece(position, colorX, colorY, colorZ);
        }

        private Face GetColorZ(int i)
        {
            i = Math.Sign(i);
            switch (i)
            {
                case -1:
                    return Face.Down;
                case 1:
                    return Face.Up;
                default:
                    return null;
            }
        }

        private Face GetColorY(int i)
        {
            i = Math.Sign(i);
            switch (i)
            {
                case -1:
                    return Face.Left;
                case 1:
                    return Face.Right;
                default:
                    return null;
            }
        }

        private Face GetColorX(int i)
        {
            i = Math.Sign(i);
            switch (i)
            {
                case -1:
                    return Face.Back;
                case 1:
                    return Face.Front;
                default:
                    return null;
            }
        }

        private IList<Piece> GetPieces(Face face)
        {
            return Pieces.Where(face.Contains).ToList();
        }

        private Piece GetPiece(int x, int y, int z)
        {
            return Pieces.First(p => p.X == x && p.Y == y && p.Z == z);
        }


        public void Rotate(Face face, Rotation.Type type)
        {
            Rotate(new Rotation(face, type), this[face]);
        }

        public void Rotate(string shorthand)
        {
            foreach (var abbreviation in shorthand.Split(' '))
            {
                var face = GetFace(abbreviation);
                var type = Rotation.GetType(abbreviation);
                var pieces = GetPieces(abbreviation);
                Rotate(new Rotation(face, type), pieces);
            }
        }

        private Face GetFace(string abbreviation)
        {
            var letter = GetLetter(abbreviation);
            var face = Face.GetFace(letter.ToString());
            if (face != null)
                return face;
            return GetWeirdFace(letter);
        }

        private static char GetLetter(string abbreviation)
        {
            var index = abbreviation.ToUpper().IndexOfAny("UDFBLRMESXYZ".ToCharArray());
            var letter = abbreviation[index];
            return letter;
        }

        private static bool IsWideRotation(string abbreviation)
        {
            return abbreviation.Contains('w');
        }

        private Face GetWeirdFace(char letter)
        {
            Face face;
            switch (letter)
            {
                case 'X':
                    face = Face.Right;
                    break;
                case 'Y':
                    face = Face.Front;
                    break;
                case 'Z':
                    face = Face.Up;
                    break;
                case 'M':
                    face = Face.Left;
                    break;
                case 'E':
                    face = Face.Down;
                    break;
                case 'S':
                    face = Face.Front;
                    break;
                default:
                    face = Face.GetFace(Char.ToUpper(letter).ToString());
                    break;
            }

            return face;
        }

        private IList<Piece> GetPieces(string abbreviation)
        {
            var face = GetLetter(abbreviation);
            var layer = GetLayer(abbreviation);
            var isWide = IsWideRotation(abbreviation);
            Func<int, int, bool> compare = (x, y) => isWide ? x >= y : x == y;
            IList<Piece> pieces;
            switch (face)
            {
                case 'S':
                    pieces = Pieces.Where(p => p.X == 0).ToList();
                    break;
                case 'E':
                    pieces = Pieces.Where(p => p.Z == 0).ToList();
                    break;
                case 'M':
                    pieces = Pieces.Where(p => p.Y == 0).ToList();
                    break;
                case 'u':
                    pieces = Pieces.Where(p => p.Z == (MaxLayer - layer) * 1).ToList();
                    break;
                case 'd':
                    pieces = Pieces.Where(p => p.Z == (MaxLayer - layer) * -1).ToList();
                    break;
                case 'r':
                    pieces = Pieces.Where(p => p.Y == (MaxLayer - layer) * 1).ToList();
                    break;
                case 'l':
                    pieces = Pieces.Where(p => p.Y == (MaxLayer - layer) * -1).ToList();
                    break;
                case 'f':
                    pieces = Pieces.Where(p => p.X == (MaxLayer - layer) * 1).ToList();
                    break;
                case 'b':
                    pieces = Pieces.Where(p => p.X == (MaxLayer - layer) * -1).ToList();
                    break;
                case 'U':
                    pieces = Pieces.Where(p => compare(p.Z, (MaxLayer - layer + 1) * 1)).ToList();
                    break;
                case 'D':
                    pieces = Pieces.Where(p => compare(p.Z, (MaxLayer - layer + 1) * -1)).ToList();
                    break;
                case 'R':
                    pieces = Pieces.Where(p => compare(p.Y, (MaxLayer - layer + 1) * 1)).ToList();
                    break;
                case 'L':
                    pieces = Pieces.Where(p => compare(p.Y, (MaxLayer - layer + 1) * -1)).ToList();
                    break;
                case 'F':
                    pieces = Pieces.Where(p => compare(p.X, (MaxLayer - layer + 1) * 1)).ToList();
                    break;
                case 'B':
                    pieces = Pieces.Where(p => compare(p.X, (MaxLayer - layer + 1) * -1)).ToList();
                    break;
                case 'X':
                case 'Y':
                case 'Z':
                    pieces = Pieces;
                    break;
                default:
                    pieces = this[Face.GetFace(face.ToString())];
                    break;
            }

            return pieces;
        }

        private int GetLayer(string abbreviation)
        {
            var result = Regex.Replace(abbreviation, @"\D+\d*", "");
            return result == "" ? 1 : int.Parse(result);
        }

        public void Rotate(Rotation rotation, IList<Piece> pieces)
        {
            foreach (var piece in pieces)
                piece.Rotate(rotation);
        }
    }
}
