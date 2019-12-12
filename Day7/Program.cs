using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Day7
{
    class Program
    {
        static string _FILE = "input.csv";
        static ArrayList read = new ArrayList();
        static int _EXPECTED = 19690720;
        static int _WORKING_VAR = 5;
        static int[] INITIAL_OPCODE;
        static int[] WORKING_OPCODE;

        static void Main(string[] args)
        {
            Stopwatch tmr = new Stopwatch();
            tmr.Start();

            Read_Input();
            Parse_Input();
            Calculate_P1();
            //Calculate_P2(out int noun, out int verb);

            Console.WriteLine();
            Display_Runtime(tmr);
        }

        private static void Calculate_P1()
        {
            WORKING_OPCODE = INITIAL_OPCODE;
            Calculate_Results();
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
            Console.WriteLine("Final value: {0}", (100 * noun) + verb);
        }

        private static void Calculate_Results()
        {
            int i = 0;
            while (i < WORKING_OPCODE.Length)
            {
                int[] opcode = GetIntArray(WORKING_OPCODE[i]);
                PARAMETER_MODES[] modesy = Get_Modes(opcode);
                int p1_Val = -1;
                int p2_Val = -1;

                int sw_Val = opcode.Length < 3 ? WORKING_OPCODE[i] : opcode[0];

                switch (sw_Val)
                {
                    case 1:
                        //ADD - DEFAULT CASE IS POSITIONAL
                        p1_Val = modesy[2] == PARAMETER_MODES.IMMEDIATE ? WORKING_OPCODE[i + 1] : WORKING_OPCODE[WORKING_OPCODE[i + 1]];
                        p2_Val = modesy[3] == PARAMETER_MODES.IMMEDIATE ? WORKING_OPCODE[i + 2] : WORKING_OPCODE[WORKING_OPCODE[i + 2]];
                        WORKING_OPCODE[WORKING_OPCODE[i + 3]] = p1_Val + p2_Val;
                        i += 4;
                        break;
                    case 2:
                        //MULTIPLY - DEFAULT CASE IS POSITIONAL
                        p1_Val = modesy[2] == PARAMETER_MODES.IMMEDIATE ? WORKING_OPCODE[i + 1] : WORKING_OPCODE[WORKING_OPCODE[i + 1]];
                        p2_Val = modesy[3] == PARAMETER_MODES.IMMEDIATE ? WORKING_OPCODE[i + 2] : WORKING_OPCODE[WORKING_OPCODE[i + 2]];
                        WORKING_OPCODE[WORKING_OPCODE[i + 3]] = p1_Val * p2_Val;
                        i += 4;
                        break;
                    case 3:
                        //INPUT - TAKE INPUT & SAVE @ ADDRESS
                        WORKING_OPCODE[WORKING_OPCODE[i + 1]] = _WORKING_VAR;
                        i += 2;
                        break;
                    case 4:
                        //OUTPUT - TAKE VALUE @ ADDRESS & PRINT TO SCREEN
                        Console.WriteLine(string.Format("OUTPUTTING VALUE [{0}] @ INDEX [{1}]", WORKING_OPCODE[WORKING_OPCODE[i + 1]], i + 1));
                        i += 2;
                        break;
                    case 5:
                        //JUMP-IF-TRUE
                        p1_Val = modesy[2] == PARAMETER_MODES.IMMEDIATE ? WORKING_OPCODE[i + 1] : WORKING_OPCODE[WORKING_OPCODE[i + 1]];
                        if (p1_Val != 0)
                        {
                            p2_Val = modesy[3] == PARAMETER_MODES.IMMEDIATE ? WORKING_OPCODE[i + 2] : WORKING_OPCODE[WORKING_OPCODE[i + 2]];
                            i = p2_Val;
                        }
                        else
                        {
                            i += 3;
                        }
                        break;
                    case 6:
                        //JUMP-IF-FALSE
                        p1_Val = modesy[2] == PARAMETER_MODES.IMMEDIATE ? WORKING_OPCODE[i + 1] : WORKING_OPCODE[WORKING_OPCODE[i + 1]];
                        if (p1_Val == 0)
                        {
                            p2_Val = modesy[3] == PARAMETER_MODES.IMMEDIATE ? WORKING_OPCODE[i + 2] : WORKING_OPCODE[WORKING_OPCODE[i + 2]];
                            i = p2_Val;
                        }
                        else
                        {
                            i += 3;
                        }
                        break;
                    case 7:
                        //LESS THAN
                        p1_Val = modesy[2] == PARAMETER_MODES.IMMEDIATE ? WORKING_OPCODE[i + 1] : WORKING_OPCODE[WORKING_OPCODE[i + 1]];
                        p2_Val = modesy[3] == PARAMETER_MODES.IMMEDIATE ? WORKING_OPCODE[i + 2] : WORKING_OPCODE[WORKING_OPCODE[i + 2]];
                        WORKING_OPCODE[WORKING_OPCODE[i + 3]] = p1_Val < p2_Val ? 1 : 0;
                        i += 4;
                        break;
                    case 8:
                        //EQUIVALENCE
                        p1_Val = modesy[2] == PARAMETER_MODES.IMMEDIATE ? WORKING_OPCODE[i + 1] : WORKING_OPCODE[WORKING_OPCODE[i + 1]];
                        p2_Val = modesy[3] == PARAMETER_MODES.IMMEDIATE ? WORKING_OPCODE[i + 2] : WORKING_OPCODE[WORKING_OPCODE[i + 2]];
                        WORKING_OPCODE[WORKING_OPCODE[i + 3]] = p1_Val == p2_Val ? 1 : 0;
                        i += 4;
                        break;
                    case 99:
                        //HALT
                        Console.WriteLine(string.Format("HALTING @ INDEX [{0}]", i));
                        i = WORKING_OPCODE.Length;
                        break;
                    default:
                        //UNKNOWN
                        Console.WriteLine(string.Format("BAD CASE! | Index: [{0}] | Value: [{1}]", i, WORKING_OPCODE[i]));
                        i = WORKING_OPCODE.Length;
                        break;
                }
            }
        }

        private static PARAMETER_MODES[] Get_Modes(int[] opcode)
        {
            //just always declare it a size of 5
            PARAMETER_MODES[] modes = new PARAMETER_MODES[5];
            switch (opcode.Length)
            {
                case 1:
                case 2:
                    //opcode only
                    modes[0] = PARAMETER_MODES.OPCODE;
                    modes[1] = PARAMETER_MODES.OPCODE;
                    modes[2] = PARAMETER_MODES.UNKNOWN;
                    modes[3] = PARAMETER_MODES.UNKNOWN;
                    modes[4] = PARAMETER_MODES.UNKNOWN;
                    break;
                case 3:
                    //Opcode + 1 parameter
                    modes[0] = PARAMETER_MODES.OPCODE;
                    modes[1] = PARAMETER_MODES.OPCODE;
                    modes[2] = opcode[2] == 1 ? PARAMETER_MODES.IMMEDIATE : PARAMETER_MODES.POSITION;
                    modes[3] = PARAMETER_MODES.UNKNOWN;
                    modes[4] = PARAMETER_MODES.UNKNOWN;
                    break;
                case 4:
                    //Opcode + 2 parameters
                    modes[0] = PARAMETER_MODES.OPCODE;
                    modes[1] = PARAMETER_MODES.OPCODE;
                    modes[2] = opcode[2] == 1 ? PARAMETER_MODES.IMMEDIATE : PARAMETER_MODES.POSITION;
                    modes[3] = opcode[3] == 1 ? PARAMETER_MODES.IMMEDIATE : PARAMETER_MODES.POSITION;
                    modes[4] = PARAMETER_MODES.UNKNOWN;
                    break;
                case 5:
                    //Opcode + 3 parameters
                    modes[0] = PARAMETER_MODES.OPCODE;
                    modes[1] = PARAMETER_MODES.OPCODE;
                    modes[2] = opcode[2] == 1 ? PARAMETER_MODES.IMMEDIATE : PARAMETER_MODES.POSITION;
                    modes[3] = opcode[3] == 1 ? PARAMETER_MODES.IMMEDIATE : PARAMETER_MODES.POSITION;
                    modes[4] = opcode[4] == 1 ? PARAMETER_MODES.IMMEDIATE : PARAMETER_MODES.POSITION;
                    break;
            }
            return modes;
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

        static void Parse_Input()
        {
            string[] str_opcode = read[0].ToString().Split(',');
            INITIAL_OPCODE = new int[str_opcode.Length];
            for (int i = 0; i < str_opcode.Length; i++)
            {
                INITIAL_OPCODE[i] = int.Parse(str_opcode[i]);
            }
        }

        enum PARAMETER_MODES
        {
            OPCODE,
            POSITION,
            IMMEDIATE,
            UNKNOWN
        }
    }
}
