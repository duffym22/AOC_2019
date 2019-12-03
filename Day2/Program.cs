using System;
using System.Collections;
using System.IO;

namespace Day2
{
    class Program
    {
        static string _FILE = "input.csv";
        static int _EXPECTED = 19690720;
        static int[] INITIAL_OPCODE;
        static int[] WORKING_OPCODE;
        static ArrayList read = new ArrayList();

        static void Main(string[] args)
        {
            Parse_Input();
            Parse_Opcode_Int();
            //Calculate_P1();
            Calculate_P2(out int noun, out int verb);
            Console.WriteLine("Final value: {0}", (100 * noun) + verb);
            
            Console.ReadLine();
        }

        private static void Calculate_P1()
        {
            WORKING_OPCODE = INITIAL_OPCODE;
            WORKING_OPCODE[1] = 12;
            WORKING_OPCODE[2] = 2;
            Calculate_Results();
            Console.WriteLine(string.Format("Position 0 value: {0}", WORKING_OPCODE[0].ToString()));
        }

        private static void Calculate_P2(out int noun, out int verb)
        {
            bool quit = false;
            noun = 0;
            verb = 0;

            for (int i = 0; i <= 99; i++)
            {
                if (!quit)
                {

                    for (int j = 0; j <= 99; j++)
                    {
                        if (!quit)
                        {

                            WORKING_OPCODE = new int[INITIAL_OPCODE.Length];
                            Array.Copy(INITIAL_OPCODE, WORKING_OPCODE, INITIAL_OPCODE.Length);
                            WORKING_OPCODE[1] = i;
                            WORKING_OPCODE[2] = j;
                            Calculate_Results();
                            Console.WriteLine(string.Format("WORKING OPCODE | NOUN: {0} | VERB: {1} | Calc. value: {2}", WORKING_OPCODE[1], WORKING_OPCODE[2], WORKING_OPCODE[0]));
                            if (WORKING_OPCODE[0].Equals(_EXPECTED))
                            {
                                Console.WriteLine(string.Format("Found expected value!"));
                                noun = WORKING_OPCODE[1];
                                verb = WORKING_OPCODE[2];
                                Console.WriteLine(string.Format("NOUN: {0} | VERB: {1}", noun, verb));
                                quit = true;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private static void Calculate_Results()
        {
            bool quit = false;
            for (int i = 0; i < WORKING_OPCODE.Length; i += 4)
            {
                if (!quit)
                {
                    int
                        pos1_Loc = 0,
                        pos1_Val = 0,
                        pos2_Loc = 0,
                        pos2_Val = 0,
                        result_Loc = 0,
                        result_Val = 0;

                    switch (WORKING_OPCODE[i])
                    {
                        case 1:
                            //ADD
                            pos1_Loc = WORKING_OPCODE[i + 1];
                            pos1_Val = WORKING_OPCODE[pos1_Loc];
                            pos2_Loc = WORKING_OPCODE[i + 2];
                            pos2_Val = WORKING_OPCODE[pos2_Loc];
                            result_Loc = WORKING_OPCODE[i + 3];
                            result_Val = pos1_Val + pos2_Val;
                            WORKING_OPCODE[result_Loc] = result_Val;
                            break;
                        case 2:
                            //MULTIPLY
                            pos1_Loc = WORKING_OPCODE[i + 1];
                            pos1_Val = WORKING_OPCODE[pos1_Loc];
                            pos2_Loc = WORKING_OPCODE[i + 2];
                            pos2_Val = WORKING_OPCODE[pos2_Loc];
                            result_Loc = WORKING_OPCODE[i + 3];
                            result_Val = pos1_Val * pos2_Val;
                            WORKING_OPCODE[result_Loc] = result_Val;
                            break;
                        case 99:
                            //HALT
                            //Console.WriteLine(string.Format("HALTING @ INDEX [{0}]", i));
                            quit = true;
                            break;
                        default:
                            //UNKNOWN
                            Console.WriteLine(string.Format("BAD CASE! | Index: [{0}] | Value: [{1}]", i, WORKING_OPCODE[i]));
                            break;
                    }
                }
            }
        }

        private static void Parse_Opcode_Int()
        {
            string[] str_opcode = read[0].ToString().Split(',');
            INITIAL_OPCODE = new int[str_opcode.Length];
            for (int i = 0; i < str_opcode.Length; i++)
            {
                INITIAL_OPCODE[i] = int.Parse(str_opcode[i]);
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
