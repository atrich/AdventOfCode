using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day01 : ASolution
    {

        public Day01() : base(01, 2020, "")
        {

        }

        protected override string SolvePartOne()
        {
            string[] lines = Input.SplitByNewline();
            var values = lines.Select(x => int.Parse(x)).ToImmutableSortedSet();
            
            for (int i = 0; i < values.Count; i++)
            {
                int target = 2020 - values[i];
                if (values.Contains(target))
                {
                    return (target * values[i]).ToString();
                }
            }

            return null;
        }

        protected override string SolvePartTwo()
        {
            string[] lines = Input.SplitByNewline();
            var values = lines.Select(x => int.Parse(x)).ToImmutableSortedSet();

            for (int i = 0; i < values.Count; i++)
            {
                for (int j = i+1; j < values.Count; j++)
                {
                    int target = 2020 - (values[i] + values[j]);
                    {
                        if (values.Contains(target) && values.IndexOf(target) != i && values.IndexOf(target) != j)
                        {
                            return (target * values[i] * values[j]).ToString();
                        }
                    } 
                }
            }

            return null;
        }
    }
}
