using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AdventOfCode.Solutions.Year2020
{
    class Day19 : ASolution
    {
        public static Dictionary<int, Rule> rules;

        public interface Rule
        {
            public int[] Match(string s);
        }

        public class BasicRule : Rule
        {
            public static BasicRule Parse(string s)
            {
                var c = s[s.IndexOf('\"') + 1];
                return new BasicRule(c);
            }

            public BasicRule(char c)
            {
                this.c = c;
            }

            public virtual int[] Match(string s)
            {
                return s[0] == c ? new int[] { 0 } : new int[] { };
            }

            char c;
        }

        public class OrRule : Rule
        {
            public static OrRule Parse(string s)
            {
                var m = s.Split('|', StringSplitOptions.TrimEntries).Select(MultiRule.Parse).ToArray();
                return new OrRule(m[0], m[1]);
            }

            public OrRule(MultiRule a, MultiRule b)
            {
                this.a = a;
                this.b = b;
            }

            public virtual int[] Match(string s)
            {
                var ma = a.Match(s);
                var mb = b.Match(s);
                return ma.Concat(mb).ToArray();
            }

            MultiRule a;
            MultiRule b;
        }

        public class MultiRule : Rule
        {
            public static MultiRule Parse(string s)
            {
                return new MultiRule(s.Split(' ').Select(int.Parse).ToArray());
            }

            public MultiRule(int[] ruleset)
            {
                this.ruleset = ruleset;
            }

            public virtual int[] TryMatch(int i, string s)
            {
                var matches = new List<int>();
                var m = rules[ruleset[i]].Match(s);
                if (m.Length == 0) return m;

                if (i < ruleset.Length - 1)
                {
                    for (int j = 0; j < m.Length; j++)
                    {
                        if (m[j] < s.Length - 1)
                        {
                            var l = TryMatch(i + 1, s.Substring(m[j] + 1));
                            if (l.Length > 0) matches.AddRange(l.Select(x => x + m[j] + 1));
                        }
                    }
                }
                else
                {
                    matches.AddRange(m);
                }

                return matches.ToArray();
            }

            public virtual int[] Match(string s)
            {
                return TryMatch(0, s);
            }

            int[] ruleset;
        }

        public Day19() : base(19, 2020, "")
        {
        }

        public Tuple<Dictionary<int, Rule>, string[]> ParseInput()
        {
            var lines = Input.Split("\n", StringSplitOptions.TrimEntries);
            var i = 0;
            var rules = new Dictionary<int, Rule>();

            while (!string.IsNullOrEmpty(lines[i]))
            {
                var a = lines[i].Split(':', StringSplitOptions.TrimEntries);
                int r = int.Parse(a[0]);

                if (a[1].Contains('\"'))
                {
                    rules[r] = BasicRule.Parse(a[1]);
                }
                else if (a[1].Contains('|'))
                {
                    rules[r] = OrRule.Parse(a[1]);
                }
                else
                {
                    rules[r] = MultiRule.Parse(a[1]);
                }

                i++;
            }

            return Tuple.Create(rules, lines.Skip(i + 1).ToArray());
        }

        protected override string SolvePartOne()
        {
            var parse = ParseInput();
            rules = parse.Item1;
            return parse.Item2.Select(x => Tuple.Create(x, rules[0].Match(x))).Where(x => x.Item2.Length > 0 && x.Item2.Any(y => y == x.Item1.Length - 1)).Count().ToString();
        }

        protected override string SolvePartTwo()
        {
            var parse = ParseInput();
            rules = parse.Item1;

            rules[8] = OrRule.Parse("42 | 42 8");
            rules[11] = OrRule.Parse("42 31 | 42 11 31");

            var lines = parse.Item2;
            return lines.Select(x => Tuple.Create(x, rules[0].Match(x))).Where(x => x.Item2.Length > 0 && x.Item2.Any(y => y == x.Item1.Length - 1)).Count().ToString();
        }
    }
}
