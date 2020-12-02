using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day02 : ASolution
    {

        public Day02() : base(02, 2020, "")
        {

        }

        protected override string SolvePartOne()
        {
            string[] lines = Input.SplitByNewline();
            int validPasses = 0;

            foreach(var line in lines)
            {
                var parts = line.Split(':');
                var conditions = parts[0].Split(' ');
                var conditionValue = conditions[1].ElementAt(0);
                var conditionNums = conditions[0].Split('-').Select(x => int.Parse(x)).ToArray();
                var password = parts[1];

                int count = password.Count(c => c == conditionValue);
                if (count >= conditionNums[0] && count <= conditionNums[1])
                {
                    validPasses++;
                }
            }

            return validPasses.ToString();
        }

        protected override string SolvePartTwo()
        {
            string[] lines = Input.SplitByNewline();
            int validPasses = 0;

            foreach (var line in lines)
            {
                var parts = line.Split(':');
                var conditions = parts[0].Split(' ');
                var conditionValue = conditions[1].ElementAt(0);
                var conditionNums = conditions[0].Split('-').Select(x => int.Parse(x)).ToArray();
                var password = parts[1].Trim();

                var amatch = password[conditionNums[0] - 1] == conditionValue;
                var bmatch = password[conditionNums[1] - 1] == conditionValue;

                if (amatch != bmatch)
                {
                    validPasses++;
                }
            }

            return validPasses.ToString();
        }
    }
}
