using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;

namespace Quber
{
    public class CubePrinter
    {
        public void Print(Cube cube)
        {
            var blankLine = new String(' ', cube.Size * 2 + 3) + Environment.NewLine;
            var blankFace = string.Concat(Enumerable.Repeat(blankLine, cube.Size * 2 + 1));

            var layerTop = Concat(blankFace, blankFace, GetFlattenedFace(cube, Face.Up));
            var layerMiddle = Concat(blankFace, GetFlattenedFace(cube, Face.Left), GetFlattenedFace(cube, Face.Front), GetFlattenedFace(cube, Face.Right), GetFlattenedFace(cube, Face.Back));
            var layerBottom = Concat(blankFace, blankFace, GetFlattenedFace(cube, Face.Down));
            var flattened = Stack(layerTop, layerMiddle, layerBottom);

            Print(flattened.ToCharArray());
        }

        private string Concat(params string[] items)
        {
            var result = items[0];
            for (int i = 1; i < items.Length; i++)
                result = Concat(result, items[i]);
            return result;
        }

        private string Concat(string left, string right)
        {
            var leftLines = left.Split(new[] { Environment.NewLine }, StringSplitOptions.None); ;
            var rightLines = right.Split(new[] { Environment.NewLine }, StringSplitOptions.None); ;
            var result = "";
            for (int i = 0; i < leftLines.Length - 1; i++)
            {
                result += leftLines[i].Substring(0, leftLines[i].Length-1) + rightLines[i] + Environment.NewLine;
            }
            return result;
        }

        private string Stack(params string[] items)
        {
            var result = items[0];
            for (int i = 1; i < items.Length; i++)
                result = Stack(result, items[i]);
            return result;
        }
        private string Stack(string top, string bottom)
        {
            var lastLineOfTop = top.Substring(top.TrimEnd().LastIndexOf(Environment.NewLine)).Trim();
            var firstLineOfBottom = bottom.Substring(0, bottom.IndexOf(Environment.NewLine)).Trim();
            if (lastLineOfTop.Length > firstLineOfBottom.Length)
                return top.TrimEnd() + Environment.NewLine + bottom.Substring(bottom.IndexOf(Environment.NewLine)+Environment.NewLine.Length);
            return top.Remove(top.TrimEnd().LastIndexOf(Environment.NewLine)) + Environment.NewLine + bottom;
        }

        private string GetFlattenedFace(Cube cube, Face face)
        {
            switch (face.Value)
            {
                case 'F':
                    return GetFlattenedFace(cube, face, p => -p.Z, p => p.Y);
                case 'B':
                    return GetFlattenedFace(cube, face, p => -p.Z, p => p.Y);
                case 'U':
                    return GetFlattenedFace(cube, face, p => p.X, p => p.Y);
                case 'D':
                    return GetFlattenedFace(cube, face, p => -p.X, p => p.Y);
                case 'R':
                    return GetFlattenedFace(cube, face, p => -p.Z, p => -p.X);
                case 'L':
                    return GetFlattenedFace(cube, face, p => -p.Z, p => p.X);
                default:
                    return "";
            }
        }

        private string GetFlattenedFace(Cube cube, Face face, Func<Piece, int> firstOrder, Func<Piece, int> secondOrder)
        {
            return flattenFace(
                cube[face]
                .OrderBy(firstOrder)
                .ThenBy(secondOrder)
                .Select(p => face.GetColor(p).Value.ToString())
                .ToList()
            );
        }

        private string flattenFace(IList<string> stickers)
        {
            var size = (int) Math.Sqrt(stickers.Count);
            var eol = Environment.NewLine;
            var firstLastLine = $"+{new String('-', size * 2 + 1)}+";
            var spacerLine = $"|{new String(' ', size * 2 + 1)}|";
            var lineStart = "| ";
            var lineEnd = "|";
            var spacer = " ";

            var flattened = $"{firstLastLine}{eol}";
            for (int i = 0; i < size; i++)
            {
                var line = lineStart;
                for (int j = 0; j < size; j++)
                {
                    line += $"{stickers[i * size + j]}{spacer}";
                }
                line += lineEnd;
                flattened += $"{line}{eol}";
                if (i < size-1)
                    flattened += $"{spacerLine}{eol}";
            }
            flattened += $"{firstLastLine}{eol}";

            return flattened;
        }

        private void Print(char[] cube)
        {
            foreach (var piece in cube)
            {
                if (Char.IsLetter(piece))
                    SetColor(piece);
                Console.Write(piece);
                Console.ResetColor();
            }
        }

        private void SetColor(char piece)
        {
            switch (piece)
            {
                case 'F':
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case 'B':
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case 'U':
                    Console.BackgroundColor = ConsoleColor.White;
                    break;
                case 'D':
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case 'L':
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case 'R':
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;
            }
        }
    }
}
