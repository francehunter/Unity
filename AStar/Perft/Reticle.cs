using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AStar
{

    public class Reticle
    {
        int size;
        Point[,] data;
        public Point pointBegin;
        public Point pointEnd;

        public Reticle(int size)
        {
            this.size = size;

            data = new Point[size, size];
            for (int i = 0; i < size; i++)
                for (int k = 0; k < size; k++)
                    data[i, k] = new Point(i, k);

            HashNearest();
        }

        public Point this[int x, int y]
        {
            get
            {
                return data[x, y];
            }
            set
            {
                data[x, y] = value;
            }
        }

        void HashNearest()
        {

            // 8 dimensions
            for (int i = 0; i < size; i++)
                for (int k = 0; k < size; k++)
                    for (int x = -1; x <= 1; x++)
                        for (int y = -1; y <= 1; y++)
                            if (!((x == 0) && (y == 0)))
                                if (Point.IsValid(i + x, k + y, size))
                                    data[i, k].near.Add(data[i + x, k + y]);

            /*
            // 4 dimensions
            for (int i = 0; i < size; i++)
                for (int k = 0; k < size; k++)
                {
                    if (Point.IsValid(i - 1, k, size))
                        data[i, k].near.Add(data[i - 1, k]);

                    if (Point.IsValid(i + 1, k, size))
                        data[i, k].near.Add(data[i + 1, k]);

                    if (Point.IsValid(i, k - 1, size))
                        data[i, k].near.Add(data[i, k - 1]);

                    if (Point.IsValid(i, k + 1, size))
                        data[i, k].near.Add(data[i, k + 1]);
                }*/
        }

        public void Clear(State[] arr)
        {
            for (int i = 0; i < size; i++)
                for (int k = 0; k < size; k++)
                    for (int j = 0; j < arr.Length; j++)
                        if (data[i, k].state == arr[j])
                            data[i, k].state = State.Empty;
        }

        public void Randomize()
        {
            Random rnd = new Random();
            for (int i = 0; i < size; i++)
                for (int k = 0; k < size; k++)
                {
                    int dice = rnd.Next(0, 10);
                    if (dice > 4)
                        data[i, k].state = State.Wall;
                    else
                        data[i, k].state = State.Empty;
                }
        }

        public List<Point> FindPath()
        {
            if ((pointBegin == null) || (pointEnd == null))
                return null;

            DateTime st = DateTime.Now;

            pointBegin.WhereCome = null;
            pointBegin.LengthPathFromStart = 0;
            pointBegin.LengthPathHeuristic = pointBegin.Distance(pointEnd);

            List<Point> open = new List<Point>();
            open.Add(pointBegin);

            HashSet<Point> close = new HashSet<Point>();

            while (open.Count > 0)
            {
                if ((DateTime.Now - st).TotalSeconds > 3)
                    return null;


                int idx = 0;
                int min = int.MinValue;
                int len = open.Count;
                for (int i = 0; i < len; i++)
                {
                    int value = open[i].EstimateFullPathLength;
                    if (value < min)
                    {
                        min = value;
                        idx = i;
                    }
                }
                /*
                    if (open[i].EstimateFullPathLength <= open[idx].EstimateFullPathLength)
                        idx = i;*/

                Point root = open[idx];

                /*
                open.Sort(Comparator);
                Point root = open.Last();*/


                if (root == pointEnd)
                    return GetPath(root.WhereCome);

                //open.RemoveAt(open.Count - 1);
                open.RemoveAt(idx);
                close.Add(root);

                for (int i = 0; i < root.near.Count; i++)
                {
                    Point point = root.near[i];

                    if (point.state == State.Wall)
                        continue;

                    if (close.Contains(point))
                        continue;

                    point.WhereCome = root;
                    point.LengthPathFromStart = root.LengthPathFromStart + 1;
                    point.LengthPathHeuristic = point.Distance(pointEnd);
                    point.EstimateFullPathLength = point.LengthPathFromStart + point.LengthPathHeuristic;

                    open.Add(point);
                }
            }

            return null;
        }

        [MethodImpl(256)]
        private static int Comparator(Point p1, Point p2)
        {
            return p2.EstimateFullPathLength - p1.EstimateFullPathLength;
        }

        private static List<Point> GetPath(Point value)
        {
            List<Point> result = new List<Point>();
            while ((value != null) && (value.state != State.Begin))
            {
                result.Add(value);
                value = value.WhereCome;
            }
            return result;
        }

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < size; i++)
                for (int k = 0; k < size; k++)
                {
                    if (sb.Length > 0)
                        sb.Append(',');
                    sb.Append(((int)data[i, k].state).ToString());
                }
            return sb.ToString();
        }

        public void Deserialize(string value)
        {
            string[] arr = value.Split(',');
            int idx = 0;
            for (int i = 0; i < size; i++)
                for (int k = 0; k < size; k++)
                {
                    data[i, k].state = (State)(int.Parse(arr[idx]));
                    idx++;
                }
        }

    }
}


