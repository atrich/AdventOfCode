using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day15 : ASolution
    {

        public Day15() : base(15, 2020, "")
        {
            // DebugInput = "0,3,6";
        }

        ulong playRounds(string input, ulong lastRound)
        {
            var nums = Input.Split(',').Select(int.Parse);
            var counts = new Dictionary<ulong, ulong>();
            var round = 0ul;

            foreach (var n in nums)
            {
                counts[(ulong)n] = round++;
            }

            var num = (ulong)nums.Last();
            var next = 0ul;

            while (round < lastRound)
            {
                num = next;

                if (counts.ContainsKey(num))
                {
                    next = round - counts[num];
                }
                else
                {
                    next = 0;
                }

                counts[num] = round++;
            }

            return num;
        }

        protected override string SolvePartOne()
        {
            return playRounds(Input, 2020).ToString();
        }

        protected override string SolvePartTwo()
        {
            return playRounds(Input, 30000000).ToString();
        }
    }
}
