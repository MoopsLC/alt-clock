#region License
// Copyright 2012 deweyvm, see also AUTHORS file.
// Licenced under GPL v3
// see COPYING file for more information or visit http://www.gnu.org/licenses/gpl-3.0.txt
#endregion Licence

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AltClock.Data
{
    public struct Point2i : IEquatable<Point2i>
    {
        public static readonly Point2i Zero = new Point2i(0, 0);
        public static readonly Point2i One = new Point2i(1, 1);
        public static readonly Point2i UnitX = new Point2i(1, 0);
        public static readonly Point2i UnitY = new Point2i(0, 1);


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

        public int Min()
        {
            return Math.Min(X, Y);
        }

        public int Max()
        {
            return Math.Max(X, Y);
        }

        public int Range()
        {
            return Math.Abs(X - Y);
        }

        public Point2i Transpose()
        {
            return new Point2i(Y, X);
        }

        public Point2i Move(int dx, int dy)
        {
            return new Point2i(X + dx, Y + dy);
        }

        public static Point2i operator *(Point2i left, Point2i right)
        {
            return new Point2i(left.X * right.X, left.Y * right.Y);
        }

        public static Point2i operator +(Point2i left, Point2i right)
        {
            return new Point2i(left.X + right.X, left.Y + right.Y);
        }

        public static Point2i operator -(Point2i self)
        {
            return new Point2i(-self.X, -self.Y);
        }

        public static Point2i operator -(Point2i left, Point2i right)
        {
            return new Point2i(left.X - right.X, left.Y - right.Y);
        }

        public static Point2i operator *(int scale, Point2i self)
        {
            return new Point2i(scale * self.X, scale * self.Y);
        }

        public static Point2i operator /(Point2i self, int scale)
        {
            return new Point2i(self.X / scale, self.Y / scale);
        }

        public static bool operator ==(Point2i p1, Point2i p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Point2i p1, Point2i p2)
        {
            return !p1.Equals(p2);
        }

        public static int TaxiDist(Point2i p0, Point2i p1)
        {
            return Math.Abs(p0.X - p1.X) + Math.Abs(p0.Y - p1.Y);
        }

        public static Tuple<Point2i, Point2i> GetSpan(IEnumerable<Point2i> points)
        {
            Option<int> minX = Option<int>.None;
            Option<int> maxX = Option<int>.None;
            Option<int> minY = Option<int>.None;
            Option<int> maxY = Option<int>.None;
            foreach (Point2i sector in points)
            {
                if (!minX.HasValue)
                {
                    minX = new Option<int>(sector.X);
                }
                if (!maxX.HasValue)
                {
                    maxX = new Option<int>(sector.X);
                }
                if (!minY.HasValue)
                {
                    minY = new Option<int>(sector.Y);
                }
                if (!maxY.HasValue)
                {
                    maxY = new Option<int>(sector.Y);
                }

                if (sector.X < minX.Value)
                {
                    minX = new Option<int>(sector.X);
                }
                if (sector.X > maxX.Value)
                {
                    maxX = new Option<int>(sector.X);
                }
                if (sector.Y < minY.Value)
                {
                    minY = new Option<int>(sector.Y);
                }
                if (sector.Y > maxY.Value)
                {
                    maxY = new Option<int>(sector.Y);
                }
            }
            Point2i span = new Point2i(maxX.Value - minX.Value + 1,
                                       maxY.Value + minY.Value + 1);
            Point2i start = new Point2i(minX.Value, minY.Value);
            return new Tuple<Point2i, Point2i>(start, span);
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
