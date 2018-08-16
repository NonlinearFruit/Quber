﻿using System.Collections.Generic;

namespace Quber
{
    public class Face
    {
        public static readonly List<Face> Options = new List<Face>();
        public static readonly Face Up = new Face('U', 0, 0, 1);
        public static readonly Face Down = new Face('D', 0, 0, -1);
        public static readonly Face Right = new Face('R', 0, 1, 0);
        public static readonly Face Left = new Face('L', 0, -1, 0);
        public static readonly Face Front = new Face('F', 1, 0, 0);
        public static readonly Face Back = new Face('B', -1, 0, 0);

        public static Face GetFace(string abbreviation)
        {
            return Options.Find(x => abbreviation.StartsWith(x.Value.ToString()));
        }

        public char Value { get; set; }

        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        private Face(char character, int x, int y, int z)
        {
            Value = character;
            _x = x;
            _y = y;
            _z = z;
            Options.Add(this);
        }

        public bool Contains(Piece piece)
        {
            var position = piece.Position;
            var result = _x * position[0, 0] + _y * position[1, 0] + _z * position[2, 0];
            return result > 0;
        }

        public Face GetColor(Piece piece)
        {
            if (_x != 0)
                return piece.ColorX;
            if (_y != 0)
                return piece.ColorY;

            return piece.ColorZ;
        }
    }
}