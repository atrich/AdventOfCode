using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day20 : ASolution
    {
        public static Dictionary<int, Tile> tiles;
        public static List<int> corners;
        public int[,] ocean;

        public class Tile
        {
            int id;
            bool[,] map;
            public List<int> matches = new List<int>();

            public int Dim {  get { return map.GetLength(1); } }

            public static Tile Parse(int id, ArraySegment<string> input)
            {
                var tile = new Tile();
                tile.id = id;
                tile.map = new bool[input[0].Length, input[0].Length];
                int x = 0, y = 0;
                foreach(var line in input)
                {
                    x = 0;
                    foreach(char c in line)
                    {
                        tile.map[x, y] = c == '#';
                        x++;
                    }
                    y++;
                }
                return tile;
            }

            static int EdgeCompare(bool[] edge1, bool[] edge2)
            {
                bool match = true;
                for(int i = 0; i < edge1.Length; i++)
                {
                    if (edge1[i] != edge2[i])
                    {
                        match = false;
                        break;
                    }
                }

                if (match) return 1;
                
                for(int i = 0; i < edge1.Length; i++)
                {
                    if (edge1[i] != edge2[edge2.Length - 1 - i])
                    {
                        return 0;
                    }
                }

                return -1;
            }

            bool[] GetEdge(int dir)
            {
                int startx, starty, dx, dy;
                int max = map.GetLength(0) - 1;

                switch (dir)
                {
                    // 0 = N, 1 = E, 2 = S, 3 = W
                    case 0:
                        startx = 0; starty = 0; dx = 1; dy = 0;
                        break;

                    case 1:
                        startx = max; starty = 0; dx = 0; dy = 1;
                        break;

                    case 2:
                        startx = max; starty = max; dx = -1; dy = 0;
                        break;

                    case 3:
                        startx = 0; starty = max; dx = 0; dy = -1;
                        break;

                    default:
                        throw new ArgumentException();
                }

                var ret = new List<bool>();

                for(int i = 0; i <= max; i++)
                {
                    ret.Add(map[startx, starty]);
                    startx += dx; starty += dy;
                }

                return ret.ToArray();
            }

            public Tuple<int, int, bool> Match(Tile other)
            {
                for(int i = 0; i < 4; i++)
                    for(int j = 0; j < 4; j++)
                    {
                        var e = EdgeCompare(this.GetEdge(i), other.GetEdge(j));
                        if (e != 0)
                        {
                            return Tuple.Create(i, j, e > 0);
                        }
                    }

                return null;
            }

            public int Fit(int dir)
            {
                int matcher = 0;
                foreach(var match in matches)
                {
                    var m = Match(tiles[match]);
                    if (m.Item1 == dir)
                    {
                        matcher = match;
                        tiles[match].Rotate((2 + dir - m.Item2) % 4);
                        if (m.Item3) tiles[match].Flip(dir % 2 == 1);
                        break;
                    }
                }

                matches.Remove(matcher);
                tiles[matcher].matches.Remove(id);
                return matcher;
            }

            public void Rotate(int cw)
            {
                map = Day20.Rotate(cw, map);
            }

            public void Flip(bool dir)
            {
                map = Day20.Flip(dir, map);
            }

            public void Print(int y)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.Write(map[x, y] ? "#" : ".");
                }
            }

            public char[,] Render()
            {
                var dim = map.GetLength(0);
                var r = new char[dim, dim];

                for (int x = 0; x < dim; x++)
                    for (int y = 0; y < dim; y++)
                        r[x, y] = map[x, y] ? '#' : '.';

                return r;
            }
        }

        public static T[,] Rotate<T>(int cw, T[,] map)
        {
            var dim = map.GetLength(0);

            while (cw > 0)
            {
                var o = new T[dim, dim];
                for (int x = 0; x < dim; x++)
                    for (int y = 0; y < dim; y++)
                        o[dim - y - 1, x] = map[x, y];
                cw--;
                map = o;
            }

            return map;
        }

        public static T[,] Flip<T>(bool dir, T[,] map)
        {
            var dim = map.GetLength(0);
            var o = new T[dim, dim];
            // true = NS, false = EW
            for (int x = 0; x < dim; x++)
                for (int y = 0; y < dim; y++)
                    if (dir)
                        o[x, dim - y - 1] = map[x, y];
                    else
                        o[dim - x - 1, y] = map[x, y];

            return o;
        }

        public Day20() : base(20, 2020, "")
        {
            ParseTiles();
            SortTiles();
        }

        public void ParseTiles()
        {
            tiles = new Dictionary<int, Tile>();
            corners = new List<int>();

            var lines = Input.Split('\n', StringSplitOptions.None);
            int i = 0;
            while(i < lines.Length)
            {
                var st = lines[i].IndexOf(' ') + 1;
                var num = int.Parse(lines[i].Substring(st, lines[i].Length - st - 1));
                i++;

                var arr = new ArraySegment<string>(lines, i, lines[i].Length);
                tiles[num] = Tile.Parse(num, arr);
                i += lines[i].Length + 1;
            }
        }

        public void SortTiles()
        {
            // for each tile, try matching against every other tile while counting matches
            foreach(var id in tiles.Keys)
            {
                int matches = 0;
                foreach(var otherid in tiles.Keys)
                {
                    if (otherid == id) continue;
                    if (tiles[id].Match(tiles[otherid]) != null)
                    {
                        tiles[id].matches.Add(otherid);
                        matches++;
                    }
                }

                if (matches == 2)
                {
                    corners.Add(id);
                }
            }
        }

        public void BuildOcean()
        {
            var dim = (int)Math.Sqrt((double)tiles.Count);
            ocean = new int[dim, dim];

            // grab a corner and insert it at 0,0
            var origin = corners[0];
            ocean[0, 0] = origin;

            // check the matching edges of the two neighboring tiles
            var n1 = tiles[origin].matches[0];
            var n2 = tiles[origin].matches[1];
            var m1 = tiles[origin].Match(tiles[n1]);
            var m2 = tiles[origin].Match(tiles[n2]);

            // force the adjacent edges to be E and S
            // possibilities are {0, 1} {1, 2} {2, 3} {3, 0}
            if (Math.Abs(m1.Item1 - m2.Item1) == 3)
            {
                tiles[origin].Rotate(2);
            }
            else
            {
                tiles[origin].Rotate((2 - Math.Max(m1.Item1, m2.Item1)) % 4);
            }

            // now build the western edge
            for(int y = 0; y < dim - 1; y++)
            {
                ocean[0, y+1] = tiles[ocean[0, y]].Fit(2);
            }

            // build each row from W to E
            for (int y = 0; y < dim; y++)
            {
                for (int x = 0; x < dim - 1; x++)
                {
                    ocean[x + 1, y] = tiles[ocean[x, y]].Fit(1);
                }
            }
        }

        public void PrintOcean()
        {
            for (int y = 0; y < ocean.GetLength(1); y++)
                for (int iy = 0; iy < tiles[ocean[0, 0]].Dim; iy++)
                    for (int x = 0; x < ocean.GetLength(0); x++)
                    {
                        if (ocean[x, y] == 0) continue;
                        tiles[ocean[x, y]].Print(iy);
                    }
                    Console.Write("\n");

            Console.Write("\n");
        }

        public void PrintRendered(char[,] o)
        {
            for (int y = 0; y < o.GetLength(1); y++)
            {
                for (int x = 0; x < o.GetLength(0); x++)
                    Console.Write(o[x, y]);
                Console.WriteLine();
            }
        }

        public char[,] RenderOcean()
        {
            var odim = ocean.GetLength(0);
            var tdim = tiles[ocean[0, 0]].Dim - 2;
            var ro = new char[odim*tdim, odim*tdim];

            for (int x = 0; x < odim; x++)
                for(int y = 0; y < odim; y++)
                {
                    var t = tiles[ocean[x, y]].Render();

                    for (int ix = 1; ix < tdim + 1; ix++)
                        for (int iy = 1; iy < tdim + 1; iy++)
                            ro[x*tdim + ix - 1, y*tdim + iy - 1] = t[ix, iy];
                }

            return ro;
        }

        public char[,] FindNessie(char[,] o)
        {
            var ro = new char[o.GetLength(0),o.GetLength(1)];

            /*
             *   01234567890123456789
             *  0                  # 
             *  1#    ##    ##    ###
             *  2 #  #  #  #  #  #   
            */
            var nessie = new List<Tuple<int, int>>();

            nessie.Add(Tuple.Create(18, 0));
            nessie.Add(Tuple.Create(0, 1));
            nessie.Add(Tuple.Create(5, 1));
            nessie.Add(Tuple.Create(6, 1));
            nessie.Add(Tuple.Create(11, 1));
            nessie.Add(Tuple.Create(12, 1));
            nessie.Add(Tuple.Create(17, 1));
            nessie.Add(Tuple.Create(18, 1));
            nessie.Add(Tuple.Create(19, 1));
            nessie.Add(Tuple.Create(1, 2));
            nessie.Add(Tuple.Create(4, 2));
            nessie.Add(Tuple.Create(7, 2));
            nessie.Add(Tuple.Create(10, 2));
            nessie.Add(Tuple.Create(13, 2));
            nessie.Add(Tuple.Create(16, 2));

            bool everfound = false;
            for (int x = 0; x < o.GetLength(0); x++)
                for (int y = 0; y < o.GetLength(1); y++)
                {
                    ro[x, y] = o[x, y];
                }

            for (int x = 0; x < o.GetLength(0) - 21; x++)
                for (int y = 0; y < o.GetLength(1) - 4; y++)
                {
                    var found = true;
                    foreach (var pt in nessie)
                    {
                        if (o[x + pt.Item1, y + pt.Item2] != '#')
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        everfound = true;
                        foreach (var pt in nessie)
                        {
                            ro[x + pt.Item1, y + pt.Item2] = 'O';
                        }
                    }
                }

            if (everfound) return ro;
            else return null;
        }

        protected override string SolvePartOne()
        {
            return corners.Aggregate(1L, (i, x) => i * x).ToString();
        }

        protected override string SolvePartTwo()
        {
            BuildOcean();
            var ro = RenderOcean();

            var no = FindNessie(ro);
            var flip = false;

            while(no == null)
            {
                // try rotations
                for(int i = 1; i <= 4; i++)
                {
                    ro = Rotate(1, ro);
                    no = FindNessie(ro);
                    if (no != null)
                    {
                        break;
                    }
                }

                // try flips
                if (no == null)
                {
                    ro = Flip(false, ro);
                    flip = !flip;
                }
            }

            var count = 0;
            for (int x = 0; x < no.GetLength(0); x++)
                for (int y = 0; y < no.GetLength(0); y++)
                    if (no[x, y] == '#') count++;

            return count.ToString();
        }
    }
}
