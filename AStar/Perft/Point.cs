
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AStar
{

    public enum State
    {
        Begin,
        End,
        Wall,
        Empty,
        Path
    }

    public class Point
    {
        public State state;
        public int x;
        public int y;
        public List<Point> near = new List<Point>();

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.state = State.Empty;
        }

        public bool IsValid(int size)
        {
            return (x >= 0) && (x < size) && (y >= 0) && (y < size);
        }

        public static bool IsValid(int x, int y, int size)
        {
            return (x >= 0) && (x < size) && (y >= 0) && (y < size);
        }

        public Point WhereCome;

        public int LengthPathFromStart;

        public int LengthPathHeuristic;

        public int EstimateFullPathLength;

        [MethodImpl(256)]
        public int Distance(Point value)
        {
            return Math.Abs(x - value.x) + Math.Abs(y - value.y);
        }      

    }


}
