using System;
using System.Collections;
using System.IO;

namespace Day3
{
    class Program
    {
        static string _FILE = "input.csv";
        static ArrayList read = new ArrayList();

        static void Main(string[] args)
        {
            Parse_Input();

            Console.WriteLine("Final value: {0}");
            Console.ReadLine();
        }

        static void Parse_Input()
        {
            using (StreamReader reader = new StreamReader(Path.GetFullPath(_FILE)))
            {
                while (!reader.EndOfStream)
                {
                    read.Add(reader.ReadLine());
                }
            }
        }
    }
}
