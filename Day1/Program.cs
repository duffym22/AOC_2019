using System;
using System.Collections;
using System.IO;

namespace Day1
{
    class Program
    {
        static ArrayList lines = new ArrayList();
        static ArrayList mass = new ArrayList();
        static int _TOTAL_FUEL = 0;
        static string _FILE = @"C:\Users\Matthew\source\repos\AOC_2019\Day1\input.csv";
        static void Main(string[] args)
        {
            Parse_Input();
            Calculate_Mass();
            Calculate_Total_Fuel();
            Console.WriteLine(string.Format("Total Fuel: {0}", _TOTAL_FUEL));

            Console.ReadLine();
        }


        private static void Calculate_Mass()
        {
            int fuel = 0;

            foreach (string item in lines)
            {
                Double.TryParse(item, out double input);
                while (input > 0)
                {
                    fuel = Calculate_Fuel(input);
                    if (fuel > 0)
                        mass.Add(fuel);
                    input = fuel;
                }
            }
        }

        private static int Calculate_Fuel(double mass)
        {
            return (int)Math.Floor(mass / 3) - 2;
        }

        private static void Calculate_Total_Fuel()
        {
            foreach (int item in mass)
            {
                _TOTAL_FUEL += item;
            }
        }

        static void Parse_Input()
        {
            using (StreamReader reader = new StreamReader(_FILE))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }
        }
    }
}
