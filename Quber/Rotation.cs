using System.Collections.Generic;

namespace Quber
{
    public class Rotation
    {
        public enum Type { Clockwise, Widdershins, Double}

        public static readonly List<Rotation> Options = new List<Rotation>();

        private static bool IsWiddershins(string abbreviation)
        {
            return abbreviation.EndsWith("'");
        }

        private static bool IsDoubleRotation(string abbreviation)
        {
            return abbreviation.EndsWith("2");
        }

        private static bool IsClockwise(string abbreviation)
        {
            return !IsDoubleRotation(abbreviation) && !IsWiddershins(abbreviation);
        }

        public static Type GetType(string abbreviation)
        {
            var type = Type.Double;
            if (IsWiddershins(abbreviation))
                type = Type.Widdershins;
            else if (IsClockwise(abbreviation))
                type = Type.Clockwise;
            return type;
        }

        private Face _face;
        private Matrix _rotationMatrix;
        private Type _type;
        public Rotation(Face face, Type type)
        {
            _face = face;
            _type = type;

            _rotationMatrix = GetRotationMatrix(face, type);
        }

        public Matrix Rotate(Matrix position)
        {
            return _rotationMatrix * position;
        }

        private Matrix GetRotationMatrix(Face face, Type type)
        {
            var size = 3;
            int cos = 0, sin = 0, skip = 0;

            switch(type)
            {
                case Type.Widdershins:
                    cos = 0;
                    sin = 1;
                    break;
                case Type.Clockwise:
                    cos = 0;
                    sin = -1;
                    break;
                case Type.Double:
                    cos = -1;
                    sin = 0;
                    break;
            }

            if (face == Face.Front || face == Face.Back)
                skip = 0;
            if (face == Face.Right|| face == Face.Left)
                skip = 1;
            if (face == Face.Up || face == Face.Down)
                skip = 2;

            if (face == Face.Down|| face == Face.Right || face == Face.Back)
                sin *= -1;

            var values = new Stack<int>(new[] {cos, sin, -sin, cos});
            var matrix = new Matrix(size, size);
            for (var row = 0; row < size; row++)
            {
                for (var column = 0; column < size; column++)
                {
                    int value;
                    if (row == skip && column == skip)
                        value = 1;
                    else if (row == skip || column == skip)
                        value = 0;
                    else
                        value = values.Pop();

                    matrix[row, column] = value;
                }
            }

            return matrix;
        }
    }
}