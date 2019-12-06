using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Day6
{
    class Program
    {
        static string _FILE = "sample.csv";
        static ArrayList read = new ArrayList();
        static SortedDictionary<string, List<string>> suborbital = new SortedDictionary<string, List<string>>();
        static SortedDictionary<string, int> subSuborbital = new SortedDictionary<string, int>();

        static void Main(string[] args)
        {
            Stopwatch tmr = new Stopwatch();
            tmr.Start();

            Read_Input();
            Parse_Input();
            Collect_Obj_Orbit_Count();
            Print_Direct_Orbits();
             
            //Answer #1 - 103697 - too low
            //Answer #2 - 107946 - too high
            int direct = Calculate_Direct_Orbits();
            int indirect = Calculate_Indirect_Orbits();
            Console.WriteLine("Total Direct: {0} | Total Indirect orbits: {1} | Total overall: {2}", direct, indirect, direct + indirect);

            Console.WriteLine();
            Display_Runtime(tmr);
        }

        private static void Print_Direct_Orbits()
        {
            int total = 0;
            foreach (KeyValuePair<string, int> item in subSuborbital)
            {
                total += item.Value;
            }
            Console.WriteLine(total);
        }

        private static int Calculate_Indirect_Orbits()
        {
            int total = 0;
            foreach (KeyValuePair<string, List<string>> item in suborbital)
            {
                List<string> te = item.Value;
                total += TraverseOrbits(te);
            }
            return total;
        }

        private static int TraverseOrbits(List<string> key)
        {
            int orb = 1;
            foreach (string item in key)
            {
                if (suborbital.ContainsKey(item))
                {
                    orb += TraverseOrbits(suborbital[item]);
                }
                else
                {
                    orb++;
                }
            }
            return orb;
        }

        private static int Calculate_Direct_Orbits()
        {
            int total = 0;
            foreach (KeyValuePair<string, List<string>> item in suborbital)
            {
                List<string> te = item.Value;
                total += te.Count;
            }
            return total;
        }

        private static void Display_Runtime(Stopwatch tmr)
        {
            Console.WriteLine(string.Format("{0,-20}:{1,22}", "Days", tmr.Elapsed.Days));
            Console.WriteLine(string.Format("{0,-20}:{1,22}", "Hours", tmr.Elapsed.Hours));
            Console.WriteLine(string.Format("{0,-20}:{1,22}", "Minutes", tmr.Elapsed.Minutes));
            Console.WriteLine(string.Format("{0,-20}:{1,22}", "Seconds", tmr.Elapsed.Seconds));
            Console.WriteLine(string.Format("{0,-20}:{1,22}", "Milliseconds", tmr.Elapsed.Milliseconds));
            Console.WriteLine(string.Format("{0,-20}:{1,22}", "Ticks", tmr.Elapsed.Ticks));
            Console.WriteLine(string.Format("{0,-20}:{1,22}", "TotalDays", tmr.Elapsed.TotalDays));
            Console.WriteLine(string.Format("{0,-20}:{1,22}", "TotalHours", tmr.Elapsed.TotalHours));
            Console.WriteLine(string.Format("{0,-20}:{1,22}", "TotalMinutes", tmr.Elapsed.TotalMinutes));
            Console.WriteLine(string.Format("{0,-20}:{1,22}", "TotalSeconds", tmr.Elapsed.TotalSeconds));
            Console.WriteLine(string.Format("{0,-20}:{1,22}", "TotalMilliseconds", tmr.Elapsed.TotalMilliseconds));
        }

        private static int[] GetIntArray(int num)
        {
            List<int> listOfInts = new List<int>();
            while (num > 0)
            {
                listOfInts.Add(num % 10);
                num = num / 10;
            }
            //listOfInts.Reverse();
            return listOfInts.ToArray();
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

        static void Collect_Obj_Orbit_Count()
        {
            foreach (string item in read)
            {
                string[] obj = item.Split(')');    //split on the orbit delimiter
                foreach (string planet in obj)
                {
                    if (!subSuborbital.ContainsKey(planet))
                    {
                        subSuborbital.Add(planet, 1);
                    }
                    else
                    {
                        subSuborbital[planet]++;
                    }
                }
            }
        }

        static void Parse_Input()
        {
            foreach (string item in read)
            {
                string[] obj = item.Split(')');    //split on the orbit delimiter
                if (!suborbital.ContainsKey(obj[0]))
                {
                    suborbital.Add(obj[0], new List<string>() { obj[1] });
                }
                else
                {
                    List<string> temp = suborbital[obj[0]];
                    temp.Add(obj[1]);
                    suborbital[obj[0]] = temp;
                }
            }
        }

    }
}
