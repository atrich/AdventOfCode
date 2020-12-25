using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day24 : ASolution
    {
        Dictionary<int, Dictionary<int, bool>> map;
        int minx = 0, miny = 0, maxx = 0, maxy = 0;

        public Day24() : base(24, 2020, "")
        {
            //DebugInput = "sesenwnenenewseeswwswswwnenewsewsw\nneeenesenwnwwswnenewnwwsewnenwseswesw\nseswneswswsenwwnwse\nnwnwneseeswswnenewneswwnewseswneseene\nswweswneswnenwsewnwneneseenw\neesenwseswswnenwswnwnwsewwnwsene\nsewnenenenesenwsewnenwwwse\nwenwwweseeeweswwwnwwe\nwsweesenenewnwwnwsenewsenwwsesesenwne\nneeswseenwwswnwswswnw\nnenwswwsewswnenenewsenwsenwnesesenew\nenewnwewneswsewnwswenweswnenwsenwsw\nsweneswneswneneenwnewenewwneswswnese\nswwesenesewenwneswnwwneseswwne\nenesenwswwswneneswsenwnewswseenwsese\nwnwnesenesenenwwnenwsewesewsesesew\nnenewswnwewswnenesenwnesewesw\neneswnwswnwsenenwnwnwwseeswneewsenese\nneswnwewnwnwseenwseesewsenwsweewe\nwseweeenwnesenwwwswnew";
        }

        void BuildMap()
        {
            map = new Dictionary<int, Dictionary<int, bool>>();
            var lines = Input.SplitByNewline();

            foreach (var line in lines)
            {
                int i = 0, x = 0, y = 0;
                char prev = ' ';
                while (i < line.Length)
                {
                    switch (line[i])
                    {
                        case 'n':
                            y--;
                            break;

                        case 's':
                            y++;
                            break;

                        case 'e':
                            if (prev != 'n')
                            {
                                x++;
                            }
                            break;

                        case 'w':
                            if (prev != 's')
                            {
                                x--;
                            }
                            break;
                    }
                    prev = line[i++];
                }

                Flip(map, x, y);
                minx = Math.Min(x, minx);
                miny = Math.Min(y, miny);
                maxx = Math.Max(x, maxx);
                maxy = Math.Max(y, maxy);
            }
        }

        protected override string SolvePartOne()
        {
            BuildMap();
            return map.SelectMany(x => x.Value.Where(y => y.Value)).Count().ToString();
        }

        bool Chk(int x, int y)
        {
            return map.ContainsKey(x) && map[x].ContainsKey(y) && map[x][y];
        }

        void Flip(Dictionary<int, Dictionary<int, bool>> n, int x, int y)
        {
            if (!n.ContainsKey(x)) n[x] = new Dictionary<int, bool>();
            if (!n[x].ContainsKey(y)) n[x][y] = true;
            else n[x][y] = !n[x][y];
        }

        Dictionary<int, Dictionary<int, bool>> Clone(Dictionary<int, Dictionary<int, bool>> m)
        {
            var n = new Dictionary<int, Dictionary<int, bool>>();
            foreach (var dx in m)
            {
                var p = new Dictionary<int, bool>();
                foreach (var dy in dx.Value) p[dy.Key] = dy.Value;
                n[dx.Key] = p;
            }
            return n;
        }

        protected override string SolvePartTwo()
        {
            BuildMap();

            for (int i = 0; i < 100; i++)
            {
                var n = Clone(map);

                for (int x = minx - 1; x <= maxx + 1; x++)
                    for (int y = miny - 1; y <= maxy + 1; y++)
                    {
                        var blk = 0;
                        if (Chk(x, y - 1)) blk++; // NE
                        if (Chk(x + 1, y)) blk++; // E
                        if (Chk(x + 1, y + 1)) blk++; // SE
                        if (Chk(x, y + 1)) blk++; // SW
                        if (Chk(x - 1, y)) blk++; // W
                        if (Chk(x - 1, y - 1)) blk++; // NW

                        var t = Chk(x, y);
                        if (t && (blk == 0 || blk > 2))
                        {
                            Flip(n, x, y);
                        }
                        else if (!t && blk == 2)
                        {
                            Flip(n, x, y);
                        }
                    }

                map = n;
                minx--;
                miny--;
                maxx++;
                maxy++;
            }

            return map.SelectMany(x => x.Value.Where(y => y.Value)).Count().ToString();
        }
    }
}
