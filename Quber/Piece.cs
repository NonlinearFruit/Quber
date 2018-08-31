using System;
using System.Collections.Generic;

namespace Quber
{
    public class Piece
    {
        public enum Type { Edge, Corner, Center }

        public int X => _position[0,0];
        public int Y => _position[1,0];
        public int Z => _position[2,0];

        public Face ColorX => _lookup[Math.Abs(_colorPosition[0, 0])];
        public Face ColorY => _lookup[Math.Abs(_colorPosition[1, 0])];
        public Face ColorZ => _lookup[Math.Abs(_colorPosition[2, 0])];

        private Matrix _position; 
        private Matrix _colorPosition;
        private IDictionary<int, Face> _lookup;

        public Piece(Matrix position, Face colorX, Face colorY, Face colorZ)
        {
            _position = position;
            SetupColors(colorX, colorY, colorZ);
        }

        public Piece(int x, int y, int z) : this(new Matrix(3, 1) {[0, 0] = x, [1, 0] = y, [2, 0] = z}, Face.Front,
            Face.Right, Face.Up)
        {
        }

        private void SetupColors(Face colorX, Face colorY, Face colorZ)
        {
            _lookup = new Dictionary<int, Face>
            {
                {'x', colorX},
                {'y', colorY},
                {'z', colorZ}
            };
            _colorPosition = new Matrix(3, 1)
            {
                [0, 0] = 'x',
                [1, 0] = 'y',
                [2, 0] = 'z'
            };
        }

        public void Rotate(Rotation rotation)
        {
            _position = rotation.Rotate(_position);
            _colorPosition = rotation.Rotate(_colorPosition);
        }

        public Type GetType()
        {
            var sumOfPositions = Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
            if (sumOfPositions == 3)
                return Type.Corner;
            if (sumOfPositions == 2)
                return Type.Edge;
            return Type.Center;
        }
    }
}
