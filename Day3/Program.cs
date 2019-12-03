using System;
using System.Collections;
using System.IO;

namespace Day3
{
    class Program
    {
        static string _FILE = "input.csv";
        static int _MAX_SIZE = 48000;
        static int _ORIGIN = 24000;
        static ArrayList read = new ArrayList();
        static ArrayList intersectPts = new ArrayList();
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

        static bool[,] wire1_grid = new bool[_MAX_SIZE, _MAX_SIZE];
        static bool[,] wire2_grid = new bool[_MAX_SIZE, _MAX_SIZE];

        static void Main(string[] args)
        {
            Read_Input();
            Parse_Input();
            Determine_Largest_Value(wire1, out wire1_U_MAX, out wire1_R_MAX, out wire1_D_MAX, out wire1_L_MAX);
            Console.WriteLine("Wire 1 | U max: {0} | R max: {1} | D max: {2} | L max: {3}", wire1_U_MAX, wire1_R_MAX, wire1_D_MAX, wire1_L_MAX);
            Determine_Largest_Value(wire2, out wire2_U_MAX, out wire2_R_MAX, out wire2_D_MAX, out wire2_L_MAX);
            Console.WriteLine("Wire 2 | U max: {0} | R max: {1} | D max: {2} | L max: {3}", wire2_U_MAX, wire2_R_MAX, wire2_D_MAX, wire2_L_MAX);

            Plot_Grid(wire1, wire1_grid);
            Plot_Grid(wire2, wire2_grid);
            GC.Collect();

            Determine_Intersects(wire1_grid, wire2_grid);
            GC.Collect();
            Determine_Manhattan_Distance();
            Console.ReadLine();
        }

        private static void Determine_Manhattan_Distance()
        {
            throw new NotImplementedException();
        }

        private static void Determine_Intersects(bool[,] grid1, bool[,] grid2)
        {
            for (int i = 0; i < _MAX_SIZE; i++)
            {
                for (int j = 0; j < _MAX_SIZE; j++)
                {
                    if(grid1[i,j] == true && grid2[i,j] == true)
                    {
                        intersectPts.Add(string.Format("{0},{1}", i, j));
                    }
                }
            }
        }

        private static void Plot_Grid(string[] wire, bool[,] grid)
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
                            grid[gridX, gridY] = true;
                        }
                        break;
                    case "D":
                        for (int i = 0; i < dist; i++)
                        {
                            gridY--;
                            grid[gridX, gridY] = true;
                        }
                        break;
                    case "L":
                        for (int i = 0; i < dist; i++)
                        {
                            gridX--;
                            grid[gridX, gridY] = true;
                        }
                        break;
                    case "R":
                        for (int i = 0; i < dist; i++)
                        {
                            gridX++;
                            grid[gridX, gridY] = true;
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
