using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding_ASharp
{
    internal class Node
    {
        public char Display { get; set; }

        public double SpecialCost
        {
            get
            {
                if (Display == MapConstants.FloodedArea)
                {
                    return MapConstants.FloodedAreaCost;
                }

                return 0.0;
            }
        }
        public double GainedCost { get; set; }

        private double heuristicCost = 0.0;
        public double HeuristicCost
        {
            get
            {
                double specialCost = (SpecialCost != 0) ? double.Epsilon : 0.0;
                return heuristicCost + specialCost;
            }

            set
            {
                heuristicCost = value;
            }
        }
        public double FCost
        {
            get
            {
                return GainedCost + HeuristicCost;
            }
        }
        public bool IsWalkable
        {
            get
            {
                return Display != MapConstants.Wall;
            }
        }
        public Point Location { get; set; }
        public Node Parent { get; set; }
    }
}
