#region License
// // Copyright 2012 deweyvm, see also AUTHORS file.
// // Licenced under GPL v3
// // see LICENCE file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Prod.Data
{
    /// <summary>
    /// An immutable two dimensional point of integers.
    /// </summary>
    public struct Point2i : IEquatable<Point2i>
    {
        public readonly int X;
        public readonly int Y;
        public Point2i(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point2i && this.Equals((Point2i)obj);
        }

        public override int GetHashCode()
        {
            return this.X ^ (this.Y >> 16);
        }

        public bool Equals(Point2i other)
        {
            return other.X == X && other.Y == Y;
        }

        /// <summary>
        /// Returns a string representation of "this" suitable for parsing 
        /// with Parse.
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0} {1}", X, Y);
        }
    }
}
