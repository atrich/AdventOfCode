using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day18 : ASolution
    {

        public Day18() : base(18, 2020, "")
        {
        }

        public long operate(char op, long lhs, long rhs)
        {
            switch (op)
            {
                case ' ':
                    return rhs;

                case '+':
                    return lhs + rhs;

                case '*':
                    return lhs * rhs;
            }

            throw new ArgumentException();
        }

        public long SolveMath(string problem, bool precedence = false)
        {
            int i = 0;
            return SolveMath(0L, problem, ref i, precedence);
        }

        public long SolveMath(long total, string problem, ref int i, bool precedence)
        {
            char op = ' ';

            for (; i < problem.Length; i++)
            {
                switch (problem[i])
                {
                    case '(':
                        i++;
                        var subtotal = SolveMath(0L, problem, ref i, precedence);
                        if (precedence && op == '*')
                        {
                            i++;
                            return operate(op, total, SolveMath(subtotal, problem, ref i, precedence));
                        }
                        else
                        {
                            total = operate(op, total, subtotal);
                        }
                        break;

                    case ')':
                        return total;

                    case '+':
                    case '*':
                        op = problem[i];
                        break;

                    case ' ':
                        continue;

                    default:
                        if (precedence && op == '*')
                        {
                            return operate(op, total, SolveMath(0L, problem, ref i, precedence));
                        }
                        else
                        {
                            total = operate(op, total, problem[i] - '0');
                        }
                        break;
                }
            }

            return total;
        }

        protected override string SolvePartOne()
        {
            var lines = Input.SplitByNewline();
            return lines.Select(x => SolveMath(x)).Sum().ToString();
        }

        protected override string SolvePartTwo()
        {
            var lines = Input.SplitByNewline();
            return lines.Select(x => SolveMath(x, true)).Sum().ToString();
        }
    }
}
