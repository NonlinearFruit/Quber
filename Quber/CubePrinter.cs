using System;
using System.Collections.Generic;
using System.Linq;

namespace Quber
{
    public class CubePrinter
    {
        public void Print(Cube cube)
        {
            var blankLine = new string(' ', cube.Size * 2 + 3) + Environment.NewLine;
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
            for (var i = 1; i < items.Length; i++)
                result = Concat(result, items[i]);
            return result;
        }

        private string Concat(string left, string right)
        {
            var leftLines = left.Split(new[] { Environment.NewLine }, StringSplitOptions.None); ;
            var rightLines = right.Split(new[] { Environment.NewLine }, StringSplitOptions.None); ;
            var result = "";
            for (var i = 0; i < leftLines.Length - 1; i++)
            {
                result += leftLines[i].Substring(0, leftLines[i].Length-1) + rightLines[i] + Environment.NewLine;
            }
            return result;
        }

        private string Stack(params string[] items)
        {
            var result = items[0];
            for (var i = 1; i < items.Length; i++)
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
            return face.Value switch
            {
                'F' => GetFlattenedFace(cube, face, p => -p.Z, p => p.Y),
                'B' => GetFlattenedFace(cube, face, p => -p.Z, p => p.Y),
                'U' => GetFlattenedFace(cube, face, p => p.X, p => p.Y),
                'D' => GetFlattenedFace(cube, face, p => -p.X, p => p.Y),
                'R' => GetFlattenedFace(cube, face, p => -p.Z, p => -p.X),
                'L' => GetFlattenedFace(cube, face, p => -p.Z, p => p.X),
                _ => ""
            };
        }

        private string GetFlattenedFace(Cube cube, Face face, Func<Piece, int> firstOrder, Func<Piece, int> secondOrder)
        {
            return FlattenFace(
                cube[face]
                .OrderBy(firstOrder)
                .ThenBy(secondOrder)
                .Select(p => face.GetColor(p).Value.ToString())
                .ToList()
            );
        }

        private string FlattenFace(IList<string> stickers)
        {
            var size = (int) Math.Sqrt(stickers.Count);
            var eol = Environment.NewLine;
            var firstLastLine = $"+{new String('-', size * 2 + 1)}+";
            var spacerLine = $"|{new String(' ', size * 2 + 1)}|";
            var lineStart = "| ";
            var lineEnd = "|";
            var spacer = " ";

            var flattened = $"{firstLastLine}{eol}";
            for (var i = 0; i < size; i++)
            {
                var line = lineStart;
                for (var j = 0; j < size; j++)
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

        private void Print(IEnumerable<char> cube)
        {
            Console.Clear();
            foreach (var piece in cube)
            {
                if (char.IsLetter(piece))
                    SetColor(piece);
                Console.Write(piece);
                Console.ResetColor();
            }
        }

        private void SetColor(char piece)
        {
            Console.BackgroundColor = piece switch
            {
                'F' => ConsoleColor.Red,
                'B' => ConsoleColor.DarkGray,
                'U' => ConsoleColor.White,
                'D' => ConsoleColor.DarkYellow,
                'L' => ConsoleColor.Green,
                'R' => ConsoleColor.Blue,
                _ => Console.BackgroundColor
            };
        }
    }
}
