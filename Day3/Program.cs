using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day3
{
    class Program
    {
        static string _FILE = "input.csv";
        static int _ORIGIN = 24000;
        static int _SMALLEST_MHD = 24000;
        static int _SMALLEST_STEPS = 500000;
        static ArrayList read = new ArrayList();
        static HashSet<string> ans;
        static List<int> wire1_Steps = new List<int>();
        static List<int> wire2_Steps = new List<int>();
        static string[] wire1;
        static string[] wire2;

        static int
                wire1_U_MAX = 0,
                wire1_R_MAX = 0,
                wire1_L_MAX = 0,
                wire1_D_MAX = 0,
                wire2_U_MAX = 0,
                wire2_R_MAX = 0,
                wire2_L_MAX = 0,
                wire2_D_MAX = 0;

        static HashSet<string> wire1_grid = new HashSet<string>();
        static HashSet<string> wire2_grid = new HashSet<string>();

        static void Main(string[] args)
        {
            Stopwatch tmr = new Stopwatch();
            tmr.Start();
            Read_Input();
            Parse_Input();
            //Determine_Largest_Value(wire1, out wire1_U_MAX, out wire1_R_MAX, out wire1_D_MAX, out wire1_L_MAX);
            //Console.WriteLine("Wire 1 | U max: {0} | R max: {1} | D max: {2} | L max: {3}", wire1_U_MAX, wire1_R_MAX, wire1_D_MAX, wire1_L_MAX);
            //Determine_Largest_Value(wire2, out wire2_U_MAX, out wire2_R_MAX, out wire2_D_MAX, out wire2_L_MAX);
            //Console.WriteLine("Wire 2 | U max: {0} | R max: {1} | D max: {2} | L max: {3}", wire2_U_MAX, wire2_R_MAX, wire2_D_MAX, wire2_L_MAX);
            Plot_Grid(wire1, wire1_grid);
            Plot_Grid(wire2, wire2_grid);
            Determine_Intersects(wire1_grid, wire2_grid);
            Determine_Manhattan_Distance();
            Console.WriteLine(_SMALLEST_MHD);

            Calculate_Shortest_Steps(wire1, wire1_Steps);
            Calculate_Shortest_Steps(wire2, wire2_Steps);

            Determine_Fewest_Steps();
            Console.WriteLine(_SMALLEST_STEPS);
            tmr.Stop();
            Console.WriteLine(string.Format("Total elapsed: {0}", tmr.Elapsed.ToString()));

            Console.ReadLine();
        }

        private static void Determine_Fewest_Steps()
        {
            for (int i = 0; i < wire1_Steps.Count; i++)
            {
                if (wire1_Steps[i] + wire2_Steps[i] < _SMALLEST_STEPS)
                {
                    _SMALLEST_STEPS = wire1_Steps[i] + wire2_Steps[i];
                }
            }
        }

        private static void Calculate_Shortest_Steps(string[] wire, List<int> stepper)
        {
            bool
                quit = false;

            int
                steps = 0,
                gridX = _ORIGIN,
                gridY = _ORIGIN;

            foreach (string item in ans)
            {
                string[] sp = item.Split(',');
                int.TryParse(sp[0], out int x);
                int.TryParse(sp[1], out int y);
                Tuple<int, int> intersectionPt = new Tuple<int, int>(x, y);

                quit = false;
                steps = 0;
                gridX = _ORIGIN;
                gridY = _ORIGIN;
                foreach (string item2 in wire)
                {
                    if (!quit)
                    {
                        string dir = item2.Substring(0, 1);
                        int.TryParse(item2.Substring(1, item2.Length - 1), out int dist);

                        switch (dir.ToUpper())
                        {
                            case "U":
                                for (int i = 0; i < dist; i++)
                                {
                                    gridY++;
                                    steps++;
                                    Tuple<int, int> t = new Tuple<int, int>(gridX, gridY);
                                    if (t.Equals(intersectionPt))
                                    {
                                        stepper.Add(steps);
                                        quit = true;
                                        break;
                                    }
                                }
                                break;
                            case "D":
                                for (int i = 0; i < dist; i++)
                                {
                                    gridY--;
                                    steps++;
                                    Tuple<int, int> t = new Tuple<int, int>(gridX, gridY);
                                    if (t.Equals(intersectionPt))
                                    {
                                        stepper.Add(steps);
                                        quit = true;
                                        break;
                                    }
                                }
                                break;
                            case "L":
                                for (int i = 0; i < dist; i++)
                                {
                                    gridX--;
                                    steps++;
                                    Tuple<int, int> t = new Tuple<int, int>(gridX, gridY);
                                    if (t.Equals(intersectionPt))
                                    {
                                        stepper.Add(steps);
                                        quit = true;
                                        break;
                                    }
                                }
                                break;
                            case "R":
                                for (int i = 0; i < dist; i++)
                                {
                                    gridX++;
                                    steps++;
                                    Tuple<int, int> t = new Tuple<int, int>(gridX, gridY);
                                    if (t.Equals(intersectionPt))
                                    {
                                        stepper.Add(steps);
                                        quit = true;
                                        break;
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static void Determine_Manhattan_Distance()
        {
            foreach (string item in ans)
            {
                string[] sp = item.Split(',');
                int.TryParse(sp[0], out int x);
                int.TryParse(sp[1], out int y);
                int MD = Math.Abs(x - _ORIGIN) + Math.Abs(y - _ORIGIN);
                if (MD < _SMALLEST_MHD)
                {
                    _SMALLEST_MHD = MD;
                    Console.WriteLine(string.Format("Pt 1: {0} | Pt 2: {1} | MD: {2}", x, y, _SMALLEST_MHD));
                }
            }
        }

        private static void Determine_Intersects(HashSet<string> grid1, HashSet<string> grid2)
        {
            ans = new HashSet<string>(grid1);
            ans.IntersectWith(grid2);

            // THIS CODE IS SUUUUUPER SLOW (20 mins) 
            //for (int i = 0; i < grid1.Count; i++)
            //{
            //    for (int j = 0; j < grid2.Count; j++)
            //    {
            //        if (grid1[i].Equals(grid2[j]))
            //            intersectPts.Add(grid2[j]);
            //    }
            //}

        }

        private static void Plot_Grid(string[] wire, HashSet<string> grid)
        {
            int
                gridX = _ORIGIN,
                gridY = _ORIGIN;

            foreach (string item in wire)
            {
                string dir = item.Substring(0, 1);
                int.TryParse(item.Substring(1, item.Length - 1), out int dist);

                switch (dir.ToUpper())
                {
                    case "U":
                        for (int i = 0; i < dist; i++)
                        {
                            gridY++;
                            grid.Add(new string(gridX + "," + gridY));
                        }
                        break;
                    case "D":
                        for (int i = 0; i < dist; i++)
                        {
                            gridY--;
                            grid.Add(new string(gridX + "," + gridY));
                        }
                        break;
                    case "L":
                        for (int i = 0; i < dist; i++)
                        {
                            gridX--;
                            grid.Add(new string(gridX + "," + gridY));
                        }
                        break;
                    case "R":
                        for (int i = 0; i < dist; i++)
                        {
                            gridX++;
                            grid.Add(new string(gridX + "," + gridY));
                        }
                        break;
                }
            }
        }

        private static void Determine_Largest_Value(string[] wire, out int uMax, out int rMax, out int dMax, out int lMax)
        {
            uMax = 0;
            rMax = 0;
            dMax = 0;
            lMax = 0;

            foreach (string item in wire)
            {
                string dir = item.Substring(0, 1);
                int.TryParse(item.Substring(1, item.Length - 1), out int dist);
                switch (dir.ToUpper())
                {
                    case "U":
                        uMax += dist;
                        break;
                    case "D":
                        dMax += dist;
                        break;
                    case "L":
                        lMax += dist;
                        break;
                    case "R":
                        rMax += dist;
                        break;
                }
            }
        }

        static void Read_Input()
        {
            using (StreamReader reader = new StreamReader(Path.GetFullPath(_FILE)))
            {
                while (!reader.EndOfStream)
                {
                    read.Add(reader.ReadLine());
                }
            }
        }

        static void Parse_Input()
        {
            wire1 = read[0].ToString().Split(',');
            wire2 = read[1].ToString().Split(',');
        }
    }
}
