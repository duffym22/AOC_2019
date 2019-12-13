﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Day7
{
    class Program
    {
        static string _FILE = "sample.csv";
        static ArrayList read = new ArrayList();
        static int _LARGEST = 0;
        static int _WORKING_VAR = 0;
        static int[] INITIAL_OPCODE;
        static int[] WORKING_OPCODE;

        static int[] THRUSTER_INPUT = new int[]
        {
            01234,
            01243,
            01324,
            01342,
            01423,
            01432,
            02134,
            02143,
            02314,
            02341,
            02413,
            02431,
            03124,
            03142,
            03214,
            03241,
            03412,
            03421,
            04123,
            04132,
            04213,
            04231,
            04312,
            04321,
            10234,
            10243,
            10324,
            10342,
            10423,
            10432,
            12034,
            12043,
            12304,
            12340,
            12403,
            12430,
            13024,
            13042,
            13204,
            13240,
            13402,
            13420,
            14023,
            14032,
            14203,
            14230,
            14302,
            14320,
            20134,
            20143,
            20314,
            20341,
            20413,
            20431,
            21034,
            21043,
            21304,
            21340,
            21403,
            21430,
            23014,
            23041,
            23104,
            23140,
            23401,
            23410,
            24013,
            24031,
            24103,
            24130,
            24301,
            24310,
            30124,
            30142,
            30214,
            30241,
            30412,
            30421,
            31024,
            31042,
            31204,
            31240,
            31402,
            31420,
            32014,
            32041,
            32104,
            32140,
            32401,
            32410,
            34012,
            34021,
            34102,
            34120,
            34201,
            34210,
            40123,
            40132,
            40213,
            40231,
            40312,
            40321,
            41023,
            41032,
            41203,
            41230,
            41302,
            41320,
            42013,
            42031,
            42103,
            42130,
            42301,
            42310,
            43012,
            43021,
            43102,
            43120,
            43201,
            43210
        };

        static void Main(string[] args)
        {
            Stopwatch tmr = new Stopwatch();
            tmr.Start();

            Read_Input();
            Parse_Input();
            Calculate_P1();

            Console.WriteLine();
            Display_Runtime(tmr);
        }

        private static void Calculate_P1()
        {
            int
                A_AMP = 0,
                B_AMP = 0,
                C_AMP = 0,
                D_AMP = 0,
                E_AMP = 0;

            for (int i = 0; i <= THRUSTER_INPUT.Length; i++)
            {
                WORKING_OPCODE = new int[INITIAL_OPCODE.Length];
                Array.Copy(INITIAL_OPCODE, WORKING_OPCODE, INITIAL_OPCODE.Length);
                int[] digits = GetDigits(THRUSTER_INPUT[i]);
                WORKING_OPCODE[1] = digits[0];
                WORKING_OPCODE[2] = digits[4];
                Calculate_Results();
                A_AMP = WORKING_OPCODE[0];

                //OUTPUT FROM AMPLIFIER A --> INPUT FOR AMPLIFIER B
                WORKING_OPCODE = new int[INITIAL_OPCODE.Length];
                Array.Copy(INITIAL_OPCODE, WORKING_OPCODE, INITIAL_OPCODE.Length);
                WORKING_OPCODE[1] = digits[1];
                WORKING_OPCODE[2] = A_AMP;
                Calculate_Results();
                B_AMP = WORKING_OPCODE[0];

                //OUTPUT FROM AMPLIFIER B --> INPUT FOR AMPLIFIER C
                WORKING_OPCODE = new int[INITIAL_OPCODE.Length];
                Array.Copy(INITIAL_OPCODE, WORKING_OPCODE, INITIAL_OPCODE.Length);
                WORKING_OPCODE[1] = digits[2];
                WORKING_OPCODE[2] = B_AMP;
                Calculate_Results();
                C_AMP = WORKING_OPCODE[0];

                //OUTPUT FROM AMPLIFIER C --> INPUT FOR AMPLIFIER D
                WORKING_OPCODE = new int[INITIAL_OPCODE.Length];
                Array.Copy(INITIAL_OPCODE, WORKING_OPCODE, INITIAL_OPCODE.Length);
                WORKING_OPCODE[1] = digits[3];
                WORKING_OPCODE[2] = C_AMP;
                Calculate_Results();
                D_AMP = WORKING_OPCODE[0];

                //OUTPUT FROM AMPLIFIER D --> INPUT FOR AMPLIFIER E
                WORKING_OPCODE = new int[INITIAL_OPCODE.Length];
                Array.Copy(INITIAL_OPCODE, WORKING_OPCODE, INITIAL_OPCODE.Length);
                WORKING_OPCODE[1] = digits[4];
                WORKING_OPCODE[2] = D_AMP;
                Calculate_Results();
                E_AMP = WORKING_OPCODE[0];

                Console.WriteLine("FINAL OUTPUT SIGNAL FROM E AMPLIFIER: {0}", E_AMP);
                if (E_AMP > _LARGEST)
                {
                    _LARGEST = E_AMP;
                    Console.WriteLine("HIGHER THRUST VALUE FOUND: {0}", _LARGEST);
                }
            }
            Console.WriteLine("Largest value: {0}", _LARGEST);
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

        private static int[] GetDigits(int num)
        {
            List<int> listOfInts = new List<int>();
            while (num > 0)
            {
                listOfInts.Add(num % 10);
                num = num / 10;
            }
            listOfInts.Reverse();
            //add leading 0
            if (listOfInts.Count.Equals(4))
            {
                List<int> fiveDigits = new List<int>(5);
                fiveDigits.Add(0);
                fiveDigits.Add(listOfInts[0]);
                fiveDigits.Add(listOfInts[1]);
                fiveDigits.Add(listOfInts[2]);
                fiveDigits.Add(listOfInts[3]);
                listOfInts = fiveDigits;
            }
            return listOfInts.ToArray();
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
