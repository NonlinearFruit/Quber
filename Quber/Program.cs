using System;

namespace Quber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("What size cube?");
            var size = int.Parse(Console.ReadLine());
            var cube = new Cube(size);
            //cube.Rotate("D2 F' B' U B L2 F R2 B2 R' F' R L' F2 L U2 L' F2 D2");
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
