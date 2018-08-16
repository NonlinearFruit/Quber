using System;

namespace Quber
{
    public class CubePrinter
    {
        public void Print(Cube cube)
        {
            var flattened = $"        +-------+\r\n" +
                            $"        | {cube[-1, -1, 1].ColorZ.Value} {cube[-1, 0, 1].ColorZ.Value} {cube[-1, 1, 1].ColorZ.Value} |\r\n" +
                            $"        |       |\r\n" +
                            $"        | {cube[0, -1, 1].ColorZ.Value} {cube[0, 0, 1].ColorZ.Value} {cube[0, 1, 1].ColorZ.Value} |\r\n" +
                            $"        |       |\r\n" +
                            $"        | {cube[1, -1, 1].ColorZ.Value} {cube[1, 0, 1].ColorZ.Value} {cube[1, 1, 1].ColorZ.Value} |\r\n" +
                            $"+-------+-------+-------+-------+\r\n" +
                            $"| {cube[-1, -1, 1].ColorY.Value} {cube[0, -1, 1].ColorY.Value} {cube[1, -1, 1].ColorY.Value} | {cube[1, -1, 1].ColorX.Value} {cube[1, 0, 1].ColorX.Value} {cube[1, 1, 1].ColorX.Value} | {cube[1, 1, 1].ColorY.Value} {cube[0, 1, 1].ColorY.Value} {cube[-1, 1, 1].ColorY.Value} | {cube[-1, 1, 1].ColorX.Value} {cube[-1, 0, 1].ColorX.Value} {cube[-1, -1, 1].ColorX.Value} |\r\n" +
                            $"|       |       |       |       |\r\n" +
                            $"| {cube[-1, -1, 0].ColorY.Value} {cube[0, -1, 0].ColorY.Value} {cube[1, -1, 0].ColorY.Value} | {cube[1, -1, 0].ColorX.Value} {cube[1, 0, 0].ColorX.Value} {cube[1, 1, 0].ColorX.Value} | {cube[1, 1, 0].ColorY.Value} {cube[0, 1, 0].ColorY.Value} {cube[-1, 1, 0].ColorY.Value} | {cube[-1, 1, 0].ColorX.Value} {cube[-1, 0, 0].ColorX.Value} {cube[-1, -1, 0].ColorX.Value} |\r\n" +
                            $"|       |       |       |       |\r\n" +
                            $"| {cube[-1, -1, -1].ColorY.Value} {cube[0, -1, -1].ColorY.Value} {cube[1, -1, -1].ColorY.Value} | {cube[1, -1, -1].ColorX.Value} {cube[1, 0, -1].ColorX.Value} {cube[1, 1, -1].ColorX.Value} | {cube[1, 1, -1].ColorY.Value} {cube[0, 1, -1].ColorY.Value} {cube[-1, 1, -1].ColorY.Value} | {cube[-1, 1, -1].ColorX.Value} {cube[-1, 0, -1].ColorX.Value} {cube[-1, -1, -1].ColorX.Value} |\r\n" +
                            $"+-------+-------+-------+-------+\r\n" +
                            $"        | {cube[1, -1, -1].ColorZ.Value} {cube[1, 0, -1].ColorZ.Value} {cube[1, 1, -1].ColorZ.Value} |\r\n" +
                            $"        |       |\r\n" +
                            $"        | {cube[0, -1, -1].ColorZ.Value} {cube[0, 0, -1].ColorZ.Value} {cube[0, 1, -1].ColorZ.Value} |\r\n" +
                            $"        |       |\r\n" +
                            $"        | {cube[-1, -1, -1].ColorZ.Value} {cube[-1, 0, -1].ColorZ.Value} {cube[-1, 1, -1].ColorZ.Value} |\r\n" +
                            $"        +-------+\r\n";

            Print(flattened.ToCharArray());
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
