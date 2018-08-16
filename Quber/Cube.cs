﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Quber
{
    public class Cube
    {
        public IList<Piece> this[Face face] => GetPieces(face);
        public Piece this[int x, int y, int z] => GetPiece(x, y, z);

        public IList<Piece> Pieces;

        private int size = 3;

        public Cube()
        {
            Pieces = new List<Piece>();
            CreatePieces(Pieces);
        }

        private void CreatePieces(IList<Piece> pieces)
        {
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                    for (int k = -1; k < 2; k++)
                        if (i != 0 || j != 0 || k != 0)
                            pieces.Add(CreatePiece(i,j,k));
                
        }

        private Piece CreatePiece(int i, int j, int k)
        {
            var position = new Matrix(3,1);
            position[0, 0] = i;
            position[1, 0] = j;
            position[2, 0] = k;

            var colorX = GetColorX(i);
            var colorY = GetColorY(j);
            var colorZ = GetColorZ(k);

            return new Piece(position, colorX, colorY, colorZ);
        }

        private Face GetColorZ(int i)
        {
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
            var list = new List<Piece>();
            foreach (var piece in Pieces)
                if (face.Contains(piece))
                    list.Add(piece);

            return list;
        }

        private Piece GetPiece(int x, int y, int z)
        {
            return Pieces.First(p => p.Position[0, 0] == x && p.Position[1, 0] == y && p.Position[2, 0] == z);
        }


        public void Rotate(Face face, Rotation.Type type)
        {
            Rotate(new Rotation(face, type), this[face]);
        }

        public void Rotate(string shorthand)
        {
            foreach (var abbreviation in shorthand.Split(' '))
            {
                var face = Face.GetFace(abbreviation);
                var type = Rotation.GetType(abbreviation);
                if (face != null)
                    Rotate(face, type);
                else
                {
                    var face2 = GetFace(abbreviation);
                    var pieces = GetPieces(abbreviation);
                    Rotate(new Rotation(face2, type), pieces);
                }

            }
        }

        private Face GetFace(string abbreviation)
        {
            var index = abbreviation.ToUpper().IndexOfAny("UDFBLRMESXYZ".ToCharArray());
            var letter = abbreviation[index];
            var face = Face.GetFace(letter.ToString());
            if (face != null)
                return face;
            return GetWeirdFace(letter);
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
                    face = Face.Up;
                    break;
                case 'Z':
                    face = Face.Front;
                    break;
                case 'M':
                    face = Face.Left;
                    break;
                case 'E':
                    face = Face.Down;
                    break;
                case 'S':
                default:
                    face = Face.Front;
                    break;
            }

            return face;
        }

        private IList<Piece> GetPieces(string abbreviation)
        {
            var face = GetFace(abbreviation);
            IList<Piece> pieces;
            switch (face.Value)
            {
                case 'M':
                default:
                    pieces = Pieces.Where(p => p.Position[1, 0] == 0).ToList();
                    break;
            }

            return pieces;
        }

        public void Rotate(Rotation rotation, IList<Piece> pieces)
        {
            foreach (var piece in pieces)
                piece.Rotate(rotation);
        }
    }
}
