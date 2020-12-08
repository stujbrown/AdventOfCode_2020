using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
{
    class Program
    {
        static bool RunProgram(string[] instructions, int[] args, out int acc)
        {
            acc = 0;
            int ic = 0;

            var instructionsTouched = new BitArray(instructions.Length);

            while (ic >= 0 && ic < instructions.Length)
            {
                if (instructionsTouched.Get(ic))
                {
                    Console.WriteLine("Loop detected. acc = {0}", acc);
                    return false;
                }

                int icAdjust = 1;
                switch (instructions[ic])
                {
                    case "acc":
                        acc += args[ic];
                        break;
                    case "jmp":
                        icAdjust = args[ic];
                        break;
                }

                instructionsTouched.Set(ic, true);
                ic += icAdjust;
            }

            return ic == instructions.Length;
        }

        static void Main(string[] args)
        {
            string[] instructionsAndArgs = File.ReadAllLines("Input.txt");
            var instructions = new string[instructionsAndArgs.Length];
            var instructionArgs = new int[instructionsAndArgs.Length];
            int fillIndex = 0;
            instructionsAndArgs.ToList().ForEach(x =>
            {
                string[] instructionAndArg = x.Trim().Split(' ');
                instructions[fillIndex] = instructionAndArg[0];
                instructionArgs[fillIndex] = int.Parse(instructionAndArg[1]);
                ++fillIndex;
            });

            int acc;
            RunProgram(instructions.ToArray(), instructionArgs.ToArray(), out acc);

            for (int instructionIndex = 0; instructionIndex < instructions.Length; ++instructionIndex)
            {
                string[] modifiedInstructions = new string[instructions.Length];
                instructions.CopyTo(modifiedInstructions, 0);
                
                if (modifiedInstructions[instructionIndex].Equals("nop"))
                {
                    modifiedInstructions[instructionIndex] = "jmp";
                }
                else if (modifiedInstructions[instructionIndex].Equals("jmp"))
                {
                    modifiedInstructions[instructionIndex] = "nop";
                }
                else
                {
                    continue;
                }

                if (RunProgram(modifiedInstructions, instructionArgs, out acc))
                {
                    Console.WriteLine("Program completed. acc = {0}", acc);
                    break;
                }

            }
        }
    }
}