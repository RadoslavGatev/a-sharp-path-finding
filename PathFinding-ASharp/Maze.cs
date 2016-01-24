using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

namespace PathFinding_ASharp
{
    public class Maze
    {
        private readonly Comparison<Node> PathComparison = (x, y) =>
         {
             var comparison = x.FCost.CompareTo(y.FCost);
             if (comparison == 0)
             {
                 return x.HeuristicCost.CompareTo(y.HeuristicCost);
             }

             return comparison;
         };

        private IList<IList<Node>> Map { get; set; }

        public Maze(string fileName)
        {
            readMap(fileName);
        }

        public IEnumerable<Point> FindPath(Point start, Point goal)
        {
            var startNode = Map[start.Row][start.Column];
            var targetNode = Map[goal.Row][goal.Column];

            var openNodes = new OrderedBag<Node>(PathComparison);
            var closedNodes = new HashSet<Node>();
            openNodes.Add(startNode);

            while (openNodes.Count > 0)
            {
                var currentNode = openNodes.RemoveFirst();
                closedNodes.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return tracePath(targetNode);
                }

                var neighbours = getNeighbours(currentNode);
                foreach (var neighbour in neighbours)
                {
                    if (!neighbour.IsWalkable || closedNodes.Contains(neighbour))
                    {
                        continue;
                    }

                    var newCost = currentNode.GainedCost + getCost(currentNode, neighbour);
                    if (newCost < neighbour.GainedCost || !openNodes.Contains(neighbour))
                    {
                        neighbour.GainedCost = newCost;
                        neighbour.HeuristicCost = getDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!openNodes.Contains(neighbour))
                        {
                            openNodes.Add(neighbour);
                        }
                    }
                }
            }

            //a path doesn't exist
            return new List<Point>();
        }

        private void readMap(string fileName)
        {
            Map = new List<IList<Node>>();

            var reader = new StreamReader(File.OpenRead(fileName));
            var rowCount = 0;
            while (!reader.EndOfStream)
            {
                var columnCount = 0;
                var line = reader.ReadLine();
                var values = line.Split(',')
                    .Select(str => new Node()
                    {
                        Display = str.First(),
                        Location = new Point(rowCount, columnCount++)
                    }).ToList();
                Map.Add(values);

                rowCount++;
            }
        }

        private IEnumerable<Node> getNeighbours(Node node)
        {
            var neighbours = new List<Node>();
            for (int row = -1; row <= 1; row++)
            {
                for (int col = -1; col <= 1; col++)
                {
                    //skip the current node
                    if (row == 0 && col == 0)
                    {
                        continue;
                    }

                    var currentRow = node.Location.Row + row;
                    var currentCol = node.Location.Column + col;

                    //check bounds
                    if (currentRow < 0 || currentCol < 0 ||
                        currentRow >= Map.Count || currentCol >= Map[currentRow].Count())
                    {
                        continue;
                    }

                    neighbours.Add(Map[currentRow][currentCol]);
                }
            }

            return neighbours;
        }

        private double getDistance(Node firstNode, Node secondNode)
        {
            var rowDistance = Math.Abs(firstNode.Location.Row - secondNode.Location.Row);
            var colDistance = Math.Abs(firstNode.Location.Column - secondNode.Location.Column);

            if (rowDistance < colDistance)
            {
                return MapConstants.DiagonalMoveCost * rowDistance +
                    MapConstants.StraightMoveCost * (colDistance - rowDistance);
            }

            return MapConstants.DiagonalMoveCost * colDistance +
                MapConstants.StraightMoveCost * (rowDistance - colDistance);
        }

        private double getCost(Node from, Node to)
        {
            var cost = 0.0;
            if (from.SpecialCost != 0)
            {
                cost = from.SpecialCost;
            }

            var areOnTheSameLine = from.Location.Row == to.Location.Row ||
                from.Location.Column == to.Location.Column;
            if (areOnTheSameLine)
            {
                cost += MapConstants.StraightMoveCost;
            }
            else
            {
                cost += MapConstants.DiagonalMoveCost;
            }

            return cost;
        }

        private IEnumerable<Point> tracePath(Node lastNode)
        {
            var path = new Stack<Point>();
            path.Push(lastNode.Location);
            while (lastNode.Parent != null)
            {
                lastNode = lastNode.Parent;
                path.Push(lastNode.Location);
            }

            return path.AsEnumerable();
        }
    }
}
