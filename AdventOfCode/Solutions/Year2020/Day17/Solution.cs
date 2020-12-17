using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day17 : ASolution
    {
        public Day17() : base(17, 2020, "")
        {
            //DebugInput = ".#.\n..#\n###\n";
        }

        protected override string SolvePartOne()
        {
            var lines = Input.SplitByNewline();
            var map = new SortedSet<Tuple<int, int, int>>();
            int y = 0;

            foreach(var line in lines)
            {
                for(int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#') map.Add(Tuple.Create(0, x, y));
                }
                y++;
            }

            int minz = -1;
            int maxz = 1;
            int minx = -1;
            int maxx = lines[0].Length;
            int miny = -1;
            int maxy = lines.Length;

            for(int i = 0; i < 6; i ++)
            {
                var nextmap = new SortedSet<Tuple<int, int, int>>();

                for (int z = minz; z <= maxz; z++)
                {
                    for(y = miny; y <= maxy; y++)
                    {
                        for(int x = minx; x <= maxx; x++)
                        {
                            int nbr = 0;
                            for(int dx = x-1; dx <= x+1; dx++)
                            {
                                for(int dy = y-1; dy <= y+1; dy++)
                                {
                                    for(int dz = z-1; dz <= z+1; dz++)
                                    {
                                        if (x == dx && y == dy && z == dz) continue;
                                        if(map.Any(p => p.Item1 == dz && p.Item2 == dx && p.Item3 == dy))
                                        {
                                            nbr++;
                                        }
                                    }
                                }
                            }

                            if(map.Any(p => p.Item1 == z && p.Item2 == x && p.Item3 == y))
                            {
                                if (nbr == 2 || nbr == 3)
                                {
                                    nextmap.Add(Tuple.Create(z, x, y));
                                }
                            }
                            else
                            {
                                if (nbr == 3)
                                {
                                    nextmap.Add(Tuple.Create(z, x, y));
                                }
                            }
                        }
                    }
                }

                map = nextmap;
                maxx++;
                maxy++;
                maxz++;
                minx--;
                miny--;
                minz--;
            }

            return map.Count.ToString();
        }

        protected override string SolvePartTwo()
        {
            var lines = Input.SplitByNewline();
            var map = new Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>>();
            int y = 0;

            map[0] = new Dictionary<int, Dictionary<int, List<int>>>();
            map[0][0] = new Dictionary<int, List<int>>();

            foreach (var line in lines)
            {
                map[0][0][y] = new List<int>();
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#') map[0][0][y].Add(x);
                }
                y++;
            }

            int minw = -1;
            int maxw = 1;
            int minz = -1;
            int maxz = 1;
            int minx = -1;
            int maxx = lines[0].Length;
            int miny = -1;
            int maxy = lines.Length;

            for (int i = 0; i < 6; i++)
            {
                var nextmap = new Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>>();

                for (int w = minw; w <= maxw; w++)
                {
                    nextmap[w] = new Dictionary<int, Dictionary<int, List<int>>>();

                    for (int z = minz; z <= maxz; z++)
                    {
                        nextmap[w][z] = new Dictionary<int, List<int>>();
                        for (y = miny; y <= maxy; y++)
                        {
                            nextmap[w][z][y] = new List<int>();
                            for (int x = minx; x <= maxx; x++)
                            {
                                int nbr = 0;
                                for (int dx = x - 1; dx <= x + 1; dx++)
                                {
                                    for (int dy = y - 1; dy <= y + 1; dy++)
                                    {
                                        for (int dz = z - 1; dz <= z + 1; dz++)
                                        {
                                            for (int dw = w - 1; dw <= w + 1; dw++)
                                            {
                                                if (x == dx && y == dy && z == dz && w == dw) continue;
                                                if (map.ContainsKey(dw) && map[dw].ContainsKey(dz) && map[dw][dz].ContainsKey(dy) && map[dw][dz][dy].Contains(dx))
                                                {
                                                    nbr++;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (map.ContainsKey(w) && map[w].ContainsKey(z) && map[w][z].ContainsKey(y) && map[w][z][y].Contains(x))
                                {
                                    if (nbr == 2 || nbr == 3)
                                    {
                                        nextmap[w][z][y].Add(x);
                                    }
                                }
                                else
                                {
                                    if (nbr == 3)
                                    {
                                        nextmap[w][z][y].Add(x);
                                    }
                                }
                            }
                        }
                    }
                }

                map = nextmap;
                maxx++;
                maxy++;
                maxz++;
                maxw++;
                minx--;
                miny--;
                minz--;
                minw--;
            }

            return map.Aggregate(0, (s, w) => s + w.Value.Aggregate(0, (s, z) => s + z.Value.Aggregate(0, (s, y) => s + y.Value.Count))).ToString();
        }
    }
}
