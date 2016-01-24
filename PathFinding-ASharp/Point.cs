using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding_ASharp
{
    public class Point
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Point()
        {
        }

        public Point(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            var rowNumber = Row + 1;
            var columnLetter = (char)('A' + Column);
            return string.Format("({0}, {1})", columnLetter, rowNumber);
        }
    }
}
