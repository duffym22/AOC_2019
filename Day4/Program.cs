using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Day4
{
    class Program
    {
        static string _FILE = "input.csv";
        static ArrayList read = new ArrayList();
        static List<int> all_Num = new List<int>();
        static List<int> regex_Num = new List<int>();
        static List<int> less_Regex_Num = new List<int>();
        static List<int> char_Num = new List<int>();
        static List<int> final_Nums = new List<int>();
        static Dictionary<int, Dictionary<int, int>> occs = new Dictionary<int, Dictionary<int, int>>();
        static int _LOWER_BOUND = 136760;
        static int _UPPER_BOUND = 595730;


        static void Main(string[] args)
        {
            Stopwatch tmr = new Stopwatch();
            tmr.Start();

            //Read_Input();
            //Parse_Input();

            Store_Numbers();
            Match_Regex_Incr_Digits();
            Create_Dict();
            Iterate_Dict();
            Examine_Dict();
            Console.WriteLine(final_Nums.Count);
            //Console.WriteLine(Match_Regex_Incr_Digits());
            //Console.WriteLine(Match_Char_Incr_Digits());

            Console.WriteLine();
            Display_Runtime(tmr);
        }

        private static void Examine_Dict()
        {
            foreach (KeyValuePair<int, Dictionary<int, int>> entry in occs)
            {
                bool hasRepeat = false;
                Dictionary<int, int> val = entry.Value;
                for (int i = 0; i < 10; i++)
                {
                    if (val[i] == 2)
                    {
                        hasRepeat = true;
                        break;
                    }
                }

                if (hasRepeat)
                    final_Nums.Add(entry.Key);
            }

        }

        private static int[] GetIntArray(int num)
        {
            List<int> listOfInts = new List<int>();
            while (num > 0)
            {
                listOfInts.Add(num % 10);
                num = num / 10;
            }
            listOfInts.Reverse();
            return listOfInts.ToArray();
        }

        private static void Iterate_Dict()
        {
            foreach (int item in regex_Num)
            {
                int[] digits = GetIntArray(item);
                //136760
                foreach (int number in digits)
                {
                    Dictionary<int, int> val = occs[item];
                    val[number]++;
                }

            }
        }

        private static void Create_Dict()
        {
            foreach (int item in regex_Num)
            {
                Dictionary<int, int> digits = new Dictionary<int, int>
                {
                    { 0, 0 },
                    { 1, 0 },
                    { 2, 0 },
                    { 3, 0 },
                    { 4, 0 },
                    { 5, 0 },
                    { 6, 0 },
                    { 7, 0 },
                    { 8, 0 },
                    { 9, 0 },
                };
                occs.Add(item, digits);
            };
        }

        private static int Match_Regex_Incr_Digits()
        {
            string pattern = @"^(?=\d{6}$)1*2*3*4*5*6*7*8*9*$";
            Regex reg = new Regex(pattern);
            foreach (int item in all_Num)
            {
                if (reg.IsMatch(item.ToString()))
                {
                    regex_Num.Add(item);
                }
            }
            return regex_Num.Count;
        }


        private static bool HasDouble(char[] chars, out int position)
        {
            bool check = false;
            position = -1;
            for (int i = 0; i < chars.Length - 1; i++)
            {
                if (chars[i] == chars[i + 1])
                {
                    check = true;
                    position = i;
                    break;
                }
            }
            return check;
        }

        private static int Match_Char_Incr_Digits()
        {
            foreach (int item in all_Num)
            {
                bool nope = false;
                char[] chars = item.ToString().ToCharArray();
                //136778
                if (chars[0] <= chars[1] &&
                    chars[1] <= chars[2] &&
                    chars[2] <= chars[3] &&
                    chars[3] <= chars[4] &&
                    chars[4] <= chars[5])
                {
                    char_Num.Add(item);
                }
            }
            return char_Num.Count;
        }

        private static void Store_Numbers()
        {
            for (int i = _LOWER_BOUND; i <= _UPPER_BOUND; i++)
            {
                all_Num.Add(i);
            }
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
            throw new NotImplementedException();
        }
    }
}
