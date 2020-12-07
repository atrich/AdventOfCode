using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day06 : ASolution
    {

        public Day06() : base(06, 2020, "")
        {

        }

        protected override string SolvePartOne()
        {
            string[] lines = Input.Split('\n');
            bool[] answers = Enumerable.Repeat(false, 26).ToArray();
            int total = 0;

            foreach (var line in lines)
            {
                if(!String.IsNullOrEmpty(line))
                {
                    foreach(char c in line)
                    {
                        answers[c - 'a'] = true;
                    }
                }
                else
                {
                    // end of group, tabulate scores
                    total += answers.Count(x => x == true);
                    answers = Enumerable.Repeat(false, 26).ToArray();
                }
            }

            // tabluate score of last group
            total += answers.Count(x => x == true);

            return total.ToString();
        }

        protected override string SolvePartTwo()
        {
            string[] lines = Input.Split('\n');
            int[] answers = Enumerable.Repeat(0, 26).ToArray();
            int groupCount = 0;
            int total = 0;

            foreach (var line in lines)
            {
                if (!String.IsNullOrEmpty(line))
                {
                    foreach (char c in line)
                    {
                        answers[c - 'a']++;
                    }
                    groupCount++;
                }
                else
                {
                    // end of group, tabulate scores
                    total += answers.Count(x => x == groupCount);
                    answers = Enumerable.Repeat(0, 26).ToArray();
                    groupCount = 0;
                }
            }

            // tabluate score of last group
            total += answers.Count(x => x == groupCount);

            return total.ToString();
        }
    }
}
