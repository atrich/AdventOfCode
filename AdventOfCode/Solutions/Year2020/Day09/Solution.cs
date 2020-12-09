using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;


namespace AdventOfCode.Solutions.Year2020
{

    class Day09 : ASolution
    {

        public Day09() : base(09, 2020, "")
        {

        }

        bool hasSum(List<long> values, long target)
        {
            var v = values.ToImmutableSortedSet();
            int i = 0;
            int j = v.Count - 1;
            long sum = v[i] + v[j];

            while(sum != target && i < j)
            {
                if (sum < target) i++;
                else if (sum > target) j--;
                sum = v[i] + v[j];
            }

            return sum == target;
        }

        protected override string SolvePartOne()
        {
            var values = Input.SplitByNewline().Select(x => long.Parse(x)).ToArray();
            var last25 = values[0..25].ToList(); // read preamble

            for(int i = 25; i < values.Length; i++)
            {
                if (!hasSum(last25, values[i]))
                {
                    return values[i].ToString();
                }

                last25.RemoveAt(0);
                last25.Add(values[i]);
            }

            return ":(";
        }

        protected override string SolvePartTwo()
        {
            var target = long.Parse(SolvePartOne());
            var values = Input.SplitByNewline().Select(x => long.Parse(x)).ToArray();

            for(int i = 0; i < values.Length - 2; i++)
            {
                for (int j = i+2; j < values.Length; j++)
                {
                    if (values[i..j].Sum() == target)
                    {
                        var r = values[i..j].ToImmutableSortedSet();
                        return (r[0] + r[r.Count-1]).ToString();
                    }
                }
            }

            return ":(";
        }
    }
}
