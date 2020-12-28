using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day25 : ASolution
    {
        public Day25() : base(25, 2020, "")
        {

        }

        int Loop(int subj, int ct)
        {
            long ac = 1;
            for(int i = 0; i < ct; i++)
            {
                ac = (ac * subj) % 20201227;
            }
            return (int)ac;
        }

        Tuple<int,int> FindSeven(Tuple<int, int> x)
        {
            long ac = 1;
            int i = 0;

            while(x.Item1 != ac && x.Item2 != ac)
            {
                ac = (ac * 7) % 20201227;
                i++;
            }

            if (x.Item1 == ac) return Tuple.Create(i, 0);
            return Tuple.Create(0, i);
        }

        protected override string SolvePartOne()
        {
            var lines = Input.SplitByNewline();
            var pk1 = int.Parse(lines[0]);
            var pk2 = int.Parse(lines[1]);

            var r = FindSeven(Tuple.Create(pk1, pk2));

            if(r.Item1 != 0)
            {
                return Loop(pk2, r.Item1).ToString();
            }

            return Loop(pk1, r.Item2).ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }
    }
}
