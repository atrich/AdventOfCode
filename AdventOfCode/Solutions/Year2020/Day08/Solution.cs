using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day08 : ASolution
    {
        public enum Operation 
        {
            acc,
            jmp,
            nop
        }

        public class Instruction
        {
            public Operation operation;
            public int param;
            public bool visited;
        }

        public Instruction[] ReadInput()
        {
            var lines = Input.SplitByNewline();
            var ret = new List<Instruction>();

            foreach (var line in lines)
            {
                var inst = line.Substring(0, 3);
                var param = line.Substring(4);
                ret.Add(
                    new Instruction
                    {
                        operation = (Operation)Enum.Parse(typeof(Operation), inst),
                        param = int.Parse(param),
                        visited = false
                    });
            }

            return ret.ToArray();
        }

        public Day08() : base(08, 2020, "")
        {

        }

        public int? ExecuteProgram(Instruction[] pgm, bool cyclesAllowed)
        {
            int pc = 0;
            int acc = 0;

            while (pc < pgm.Length && !pgm[pc].visited)
            {
                pgm[pc].visited = true;

                // read current instruction
                switch (pgm[pc].operation)
                {
                    case Operation.acc:
                        acc += pgm[pc].param;
                        pc++;
                        break;

                    case Operation.jmp:
                        pc += pgm[pc].param;
                        break;

                    case Operation.nop:
                        pc++;
                        break;
                }
            }

            Array.ForEach(pgm, o => o.visited = false);
            return (cyclesAllowed || pc >= pgm.Length) ? acc : null;
        }

        protected override string SolvePartOne()
        {
            var pgm = ReadInput();
            return ExecuteProgram(pgm, true).Value.ToString();
        }

        protected override string SolvePartTwo()
        {
            var pgm = ReadInput();

            for (int i = 0; i < pgm.Length; i++)
            {
                var backup = pgm[i].operation;
                if (pgm[i].operation != Operation.acc)
                {
                    pgm[i].operation = backup == Operation.jmp ? Operation.nop : Operation.jmp;
                }
                var res = ExecuteProgram(pgm, false);

                if (res.HasValue)
                {
                    return res.Value.ToString();
                }
                pgm[i].operation = backup;
            }

            return "nope :(";
        }
    }
}
