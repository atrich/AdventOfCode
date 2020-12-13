using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day05 : ASolution
    {

        public Day05() : base(05, 2020, "")
        {
            // this.DebugInput = "BFFFBBFRRR";
        }

        protected override string SolvePartOne()
        {
            string[] lines = Input.SplitByNewline();
            int maxseatid = 0;
            foreach (var line in lines)
            {
                int front = 0;
                int back = 127;
                int left = 0;
                int right = 7;

                foreach (var c in line)
                {
                    int range = 0;
                    switch (c)
                    {
                        case 'F':
                            range = back - front + 1;
                            back -= (range / 2);
                            continue;

                        case 'B':
                            range = back - front + 1;
                            front += (range / 2);
                            continue;

                        case 'L':
                            range = right - left + 1;
                            right -= (range / 2);
                            continue;

                        case 'R':
                            range = right - left + 1;
                            left += (range / 2);
                            continue;
                    }
                }
                var seatid = front * 8 + left;
                maxseatid = Math.Max(seatid, maxseatid);
            }

            return maxseatid.ToString();
        }

        protected override string SolvePartTwo()
        {
            string[] lines = Input.SplitByNewline();
            var seats = new SortedSet<int>();

            foreach (var line in lines)
            {
                int front = 0;
                int back = 127;
                int left = 0;
                int right = 7;

                foreach (var c in line)
                {
                    int range = 0;
                    switch (c)
                    {
                        case 'F':
                            range = back - front + 1;
                            back -= (range / 2);
                            continue;

                        case 'B':
                            range = back - front + 1;
                            front += (range / 2);
                            continue;

                        case 'L':
                            range = right - left + 1;
                            right -= (range / 2);
                            continue;

                        case 'R':
                            range = right - left + 1;
                            left += (range / 2);
                            continue;
                    }
                }
                var seatid = front * 8 + left;
                seats.Add(seatid);
            }

            int prevseat = seats.First();
            foreach(var seat in seats)
            {
                if (seat - prevseat > 1) {
                    return (prevseat + 1).ToString();
                }
                prevseat = seat;
            }

            return "";
        }
    }
}
