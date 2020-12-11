using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AdventOfCode.Solutions.Year2020
{

    class Day11 : ASolution
    {

        public Day11() : base(11, 2020, "")
        {
            // DebugInput = "L.LL.LL.LL\nLLLLLLL.LL\nL.L.L..L..\nLLLL.LL.LL\nL.LL.LL.LL\nL.LLLLL.LL\n..L.L.....\nLLLLLLLLLL\nL.LLLLLL.L\nL.LLLLL.LL\n";
        }

        public char[][] dupe(char[][] input)
        {
            var output = new char[input.Length][];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (char[])input[i].Clone();
            }

            return output;
        }

        public bool equals(char[][] lhs, char[][] rhs)
        {
            for(int i = 0; i < lhs.Length; i++)
            {
                for(int j = 0; j < lhs[0].Length; j++)
                {
                    if (lhs[i][j] != rhs[i][j]) return false;
                }
            }

            return true;
        }

        public char[][] process(char[][] input)
        {
            var output = dupe(input);

            for(int x = 0; x < input[0].Length; x++)
            {
                for(int y = 0; y < input.Length; y++)
                {
                    var occ = 0;
                    if (y >= 1)
                    {
                        // XXX
                        // -0-
                        // ---
                        if (x >= 1 && input[y-1][x-1] == '#') occ++;
                        if (input[y-1][x] == '#') occ++;
                        if (x < input[y].Length-1 && input[y-1][x+1] == '#') occ++;
                    }

                    // ---
                    // X0X
                    // ---
                    if (x >= 1 && input[y][x-1] == '#') occ++;
                    if (x < input[y].Length-1 && input[y][x+1] == '#') occ++;

                    if (y < input.Length-1)
                    {
                        // ---
                        // -0-
                        // XXX
                        if (x >= 1 && input[y + 1][x-1] == '#') occ++;
                        if (input[y + 1][x] == '#') occ++;
                        if (x < input[y].Length-1 && input[y + 1][x+1] == '#') occ++;
                    }

                    if (input[y][x] == 'L' && occ == 0) { output[y][x] = '#'; }
                    else if (input[y][x] == '#' && occ >= 4) { output[y][x] = 'L'; }
                }
            }

            return output;
        }

        public char[][] process2(char[][] input)
        {
            var output = dupe(input);

            for (int x = 0; x < input[0].Length; x++)
            {
                for (int y = 0; y < input.Length; y++)
                {
                    var occ = 0;
                    var w = input[y].Length;
                    var l = input.Length;

                    // search N
                    for (int i = y-1; i >= 0; i--)
                    {
                        if (input[i][x] == '#') { occ++; break; }
                        if (input[i][x] == 'L') break;
                    }

                    // search E
                    for (int i = x+1; i < w; i++)
                    {
                        if (input[y][i] == '#') { occ++; break; }
                        if (input[y][i] == 'L') { break; }
                    }

                    // search S
                    for (int i = y+1; i < l; i++)
                    {
                        if (input[i][x] == '#') { occ++; break; }
                        if (input[i][x] == 'L') { break; }
                    }

                    // search W
                    for (int i = x-1; i >= 0; i--)
                    {
                        if (input[y][i] == '#') { occ++; break; }
                        if (input[y][i] == 'L') { break; }
                    }

                    // search NE
                    for (int i = 1; y-i >= 0 && x+i < w; i++)
                    {
                        if (input[y-i][x+i] == '#') { occ++; break; }
                        if (input[y-i][x+i] == 'L') { break; }
                    }

                    // search NW
                    for (int i = 1; y-i >= 0 && x-i >= 0; i++)
                    {
                        if (input[y-i][x-i] == '#') { occ++; break; }
                        if (input[y-i][x-i] == 'L') { break; }
                    }

                    // search SE
                    for (int i = 1; y+i < l && x+i < w; i++)
                    {
                        if (input[y+i][x+i] == '#') { occ++; break; }
                        if (input[y+i][x+i] == 'L') { break; }
                    }

                    // search SW
                    for (int i = 1; y+i < l && x-i >= 0; i++)
                    {
                        if (input[y+i][x-i] == '#') { occ++; break; }
                        if (input[y+i][x-i] == 'L') { break; }
                    }

                    if (input[y][x] == 'L' && occ == 0) { output[y][x] = '#'; }
                    else if (input[y][x] == '#' && occ >= 5) { output[y][x] = 'L'; }
                }
            }

            return output;
        }

        protected override string SolvePartOne()
        {
            var map = Input.SplitByNewline().Select((x) => x.Trim().ToCharArray()).ToArray();
            var next = process(map);
            
            while (!equals(map, next))
            {
                map = dupe(next);
                next = process(map);
            }

            // now count occupied seats
            var count = next.Aggregate(0, (sum, p) => sum + p.Count((c) => c == '#'));
            return count.ToString();
        }

        protected override string SolvePartTwo()
        {
            var map = Input.SplitByNewline().Select((x) => x.Trim().ToCharArray()).ToArray();
            var next = process2(map);

            while (!equals(map, next))
            {
                map = dupe(next);
                next = process2(map);
            }

            // now count occupied seats
            var count = next.Aggregate(0, (sum, p) => sum + p.Count((c) => c == '#'));
            return count.ToString();
        }
    }
}
