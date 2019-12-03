using System;
using System.Collections;
using System.IO;

namespace Day2
{
    class Program
    {
        static string _FILE = "input.csv";
        static int _EXPECTED = 19690720;
        static int[] opcode;
        static ArrayList read = new ArrayList();

        static void Main(string[] args)
        {
            Parse_Input();
            Parse_Opcode_Int();

            opcode[1] = 12;
            opcode[2] = 2;

            Calculate_Results();

            Console.WriteLine(string.Format("Position 0 value: {0}", opcode[0].ToString()));
            Console.ReadLine();
        }

        private static void Calculate_Results()
        {
            bool quit = false;
            for (int i = 0; i < opcode.Length; i += 4)
            {
                if (!quit)
                {

                    int pos1_Loc = 0;
                    int pos1_Val = 0;
                    int pos2_Loc = 0;
                    int pos2_Val = 0;
                    int result_Loc = 0;
                    switch (opcode[i])
                    {
                        case 1:
                            //ADD
                            pos1_Loc = opcode[i + 1];
                            pos1_Val = opcode[pos1_Loc];
                            pos2_Loc = opcode[i + 2];
                            pos2_Val = opcode[pos2_Loc];
                            result_Loc = opcode[i + 3];
                            opcode[result_Loc] = pos1_Val + pos2_Val;
                            break;
                        case 2:
                            //MULTIPLY
                            pos1_Loc = opcode[i + 1];
                            pos1_Val = opcode[pos1_Loc];
                            pos2_Loc = opcode[i + 2];
                            pos2_Val = opcode[pos2_Loc];
                            result_Loc = opcode[i + 3];
                            opcode[result_Loc] = pos1_Val * pos2_Val;
                            break;
                        case 99:
                            //HALT
                            Console.WriteLine(string.Format("HALTING @ INDEX [{0}]", i));
                            quit = true;
                            break;
                        default:
                            //UNKNOWN
                            Console.WriteLine(string.Format("BAD CASE! | Index: [{0}] | Value: [{1}]", i, opcode[i]));
                            break;
                    }
                }
            }
        }

        private static void Parse_Opcode_Int()
        {
            string[] str_opcode = read[0].ToString().Split(',');
            opcode = new int[str_opcode.Length];
            for (int i = 0; i < str_opcode.Length; i++)
            {
                opcode[i] = int.Parse(str_opcode[i]);
            }
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
