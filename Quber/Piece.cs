using System;
using System.Collections.Generic;
using System.Drawing;

namespace Quber
{
    public class Piece
    {
        public enum Type { Edge, Corner, Center }
        public Matrix Position { get; set; }

        public Face ColorX => _lookup[Math.Abs(_colorPosition[0, 0])];
        public Face ColorY => _lookup[Math.Abs(_colorPosition[1, 0])];
        public Face ColorZ => _lookup[Math.Abs(_colorPosition[2, 0])];

        private Matrix _colorPosition;
        private IDictionary<int, Face> _lookup;

        public Piece(Matrix position, Face colorX, Face colorY, Face colorZ)
        {
            Position = position;
            SetupColors(colorX, colorY, colorZ);
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
            Position = rotation.Rotate(Position);
            _colorPosition = rotation.Rotate(_colorPosition);
        }

        public Type GetType()
        {
            var sumOfPositions = Math.Abs(Position[0, 0]) + Math.Abs(Position[1, 0]) + Math.Abs(Position[2, 0]);
            if (sumOfPositions == 3)
                return Type.Corner;
            if (sumOfPositions == 2)
                return Type.Edge;
            return Type.Center;
        }
    }
}
