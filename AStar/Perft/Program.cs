using AStar;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Perft
{
    class Program
    {
        static void Main(string[] args)
        {
            const int size = 50;
            Reticle ret = new Reticle(size);
            ret.Randomize();
            ret.pointBegin = ret[0, 0];
            ret.pointEnd = ret[size - 1, size - 1];

            if (args.Length > 0)
            {
                string s = System.IO.File.ReadAllText(@args[0]);
                s = Regex.Replace(s, @"\t|\n|\r", "");
                int newSize = ret.Deserialize(s);
                if (newSize > 0)
                    Console.WriteLine(String.Format("loaded {0}x{0} grid from {1}", newSize, args[0]));
                else
                    Console.WriteLine("loading error");
            }

            DateTime st = DateTime.Now;
            List<Point> path = ret.FindPath();
            if (path != null)
            {
                string msg = String.Format(
                    "found time {0:F4} sec with path length {1} point(s)", 
                    (DateTime.Now - st).TotalSeconds, path.Count);
                Console.WriteLine(msg);                                        
                foreach (Point p in path)
                    ret[p.x, p.y].state = State.Path;
            }
            else
                Console.WriteLine("not found");

            Console.ReadLine();
        }
    }
}
