using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day03 : ASolution
    {

        public Day03() : base(03, 2020, "")
        {
        }

        private int CheckSlope(int x, int y)
        {
            string[] lines = Input.SplitByNewline();
            int width = lines[0].Length;
            int depth = lines.Length;
            int xpos = 0, ypos = 0;
            int trees = 0;

            while (ypos < depth - 1)
            {
                ypos += y;
                xpos += x;
                if (xpos >= width) xpos -= width;
                if (lines[ypos][xpos] == '#')
                {
                    trees++;
                }
            }

            return trees;
        }

        protected override string SolvePartOne()
        {
            return CheckSlope(3, 1).ToString();
        }

        protected override string SolvePartTwo()
        {
            int x = CheckSlope(1, 1) * CheckSlope(3, 1) * CheckSlope(5, 1) * CheckSlope(7, 1) * CheckSlope(1, 2);
            return x.ToString();
        }
    }
}
