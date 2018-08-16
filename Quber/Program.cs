using System;

namespace Quber
{
    class Program
    {
        static void Main(string[] args)
        {
            var cube = new Cube();
            var printer = new CubePrinter();
            while (true)
            {
                printer.Print(cube);
                var rotations = Console.ReadLine();
                cube.Rotate(rotations);
            }
        }
    }
}
