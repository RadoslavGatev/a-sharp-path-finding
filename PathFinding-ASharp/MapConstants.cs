using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding_ASharp
{
    internal static class MapConstants
    {
        public const char Wall = 'N';
        public const char FloodedArea = '~';

        public const double StraightMoveCost = 1;
        public const double DiagonalMoveCost = 1.5;
        public const double FloodedAreaCost = 2;
    }
}
