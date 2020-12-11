using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace AdventOfCode.Solutions.Year2020
{

    class Day10 : ASolution
    {

        public Day10() : base(10, 2020, "")
        {
            //DebugInput = "16\n10\n15\n5\n1\n11\n7\n19\n6\n12\n4\n";
        }

        protected override string SolvePartOne()
        {
            var jolts = Input.SplitByNewline().Select(x => int.Parse(x)).ToImmutableSortedSet();
            int prev = 0;
            var diff = Enumerable.Repeat(0, 3).ToArray();

            foreach(var jolt in jolts)
            {
                diff[jolt - prev - 1]++;
                prev = jolt;
            }
            diff[2]++; // account for final +3`

            return (diff[0]*diff[2]).ToString();
        }

        protected override string SolvePartTwo()
        {
            var jolts = Input.SplitByNewline().Select(x => int.Parse(x)).ToImmutableSortedSet();
            var combos = Enumerable.Repeat(0L, jolts.Count).ToArray();
            combos[jolts.Count - 1] = 1;

            for (int i = jolts.Count - 2; i >= 0; i--)
            {
                int target = jolts[i] + 3;
                for (int j = i+1; j < jolts.Count && jolts[j] <= target; j++)
                {
                    combos[i] += combos[j]; // any combos j has are combos for me too
                }
            }

            long totalCombos = 0;
            int k = 0;
            while (jolts[k] <= 3)
            {
                totalCombos += combos[k];
                k++;
            }

            return totalCombos.ToString();
        }
    }
}
