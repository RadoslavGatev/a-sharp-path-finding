using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding_ASharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var mapFileName = args[0];
            var maze = new Maze(mapFileName);

            var startLocation = new Point(int.Parse(args[1]), int.Parse(args[2]));
            var targetLocation = new Point(int.Parse(args[3]), int.Parse(args[4]));

            var path = maze.FindPath(startLocation, targetLocation);
            var pathStr = String.Join(", ", path);
            Console.WriteLine(pathStr);
        }
    }
}
