using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day22 : ASolution
    {
        List<uint> p = new List<uint>();
        List<uint> o = new List<uint>();

        public Day22() : base(22, 2020, "")
        {
            //DebugInput = "Player 1:\n9\n2\n6\n3\n1\nPlayer 2:\n5\n8\n4\n7\n10\n";
        }

        public uint Pop(List<uint> deck)
        {
            var top = deck[0];
            deck.RemoveAt(0);
            return top;
        }

        public void ParseInput()
        {
            p.Clear(); o.Clear();
            var lines = Input.SplitByNewline();
            bool p2 = false;

            foreach (var line in lines.Skip(1))
            {
                if (line.Contains("Player"))
                {
                    p2 = true;
                    continue;
                }

                if (!p2)
                {
                    p.Add(uint.Parse(line));
                }
                else
                {
                    o.Add(uint.Parse(line));
                }
            }

        }

        public long CalcScore(List<uint> w)
        {
            long score = 0;
            for (uint i = 0; i < w.Count; i++)
                score += w[(int)i] * (w.Count - i);
            return score;
        }

        public bool Play(List<uint> p1, List<uint> p2)
        {
            var prev = new HashSet<string>();

            while(p1.Count > 0 && p2.Count > 0)
            {
                // check for infinite game
                var roundid = string.Join(',', p1.ToArray()) + "|" + string.Join(',', p2.ToArray());
                if (prev.Contains(roundid)) return true;
                prev.Add(roundid);

                var a = Pop(p1);
                var b = Pop(p2);

                if (a <= p1.Count && b <= p2.Count)
                {
                    var r = Play(new List<uint>(p1.GetRange(0, (int)a)), new List<uint>(p2.GetRange(0, (int)b)));
                    if (r)
                    {
                        p1.Add(a); p1.Add(b);
                    }
                    else
                    {
                        p2.Add(b); p2.Add(a);
                    }
                }
                else if (a > b)
                {
                    p1.Add(a); p1.Add(b);
                }
                else
                {
                    p2.Add(b); p2.Add(a);
                }
            }

            p = p1;
            o = p2;

            if (p1.Count > 0) return true;
            return false;
        }

        protected override string SolvePartOne()
        {
            ParseInput();
            while (p.Count > 0 && o.Count > 0)
            {
                var a = Pop(p);
                var b = Pop(o);

                if (a > b)
                {
                    p.Add(a); p.Add(b);
                }
                else
                {
                    o.Add(b); o.Add(a);
                }
            }

            var win = p.Count > 0 ? p : o;
            return CalcScore(win).ToString();
        }

        protected override string SolvePartTwo()
        {
            ParseInput();
            if (Play(new List<uint>(p), new List<uint>(o)))
            {
                return CalcScore(p).ToString();
            }
            return CalcScore(o).ToString();
        }
    }
}
