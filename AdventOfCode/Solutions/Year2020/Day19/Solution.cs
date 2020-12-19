using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    class Day19 : ASolution
    {
        public static Dictionary<int, Rule> rules;

        public interface Rule
        {
            public IEnumerable<int> Match(string s);
        }

        public class BasicRule : Rule
        {
            public static BasicRule Parse(string s)
            {
                return new BasicRule { c = s[s.IndexOf('\"') + 1] };
            }

            public virtual IEnumerable<int> Match(string s)
            {
                return s[0] == c ? new int[] { 0 } : new int[] { };
            }

            char c;
        }

        public class OrRule : Rule
        {
            public static OrRule Parse(string s)
            {
                var m = s.Split('|', StringSplitOptions.TrimEntries).Select(MultiRule.Parse);
                return new OrRule { a = m.First(), b = m.Last() };
            }

            public virtual IEnumerable<int> Match(string s)
            {
                return a.Match(s).Concat(b.Match(s));
            }

            MultiRule a;
            MultiRule b;
        }

        public class MultiRule : Rule
        {
            public static MultiRule Parse(string s)
            {
                return new MultiRule { ruleset = s.Split(' ').Select(int.Parse).ToArray() };
            }

            public virtual IEnumerable<int> TryMatch(int i, string s)
            {
                var m = rules[ruleset[i]].Match(s);

                if (m.Any() && i < ruleset.Length - 1)
                {
                    return m.Where(x => x < s.Length - 1).SelectMany(x => TryMatch(i + 1, s.Substring(x + 1)).Select(y => y + x + 1));
                }
                else
                {
                    return m;
                }
            }

            public virtual IEnumerable<int> Match(string s)
            {
                return TryMatch(0, s);
            }

            int[] ruleset;
        }

        public Day19() : base(19, 2020, "")
        {
        }

        public IEnumerable<string> ParseInput()
        {
            var lines = Input.Split("\n", StringSplitOptions.TrimEntries);

            var i = 0;
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

            return lines.Skip(i + 1);
        }

        protected override string SolvePartOne()
        {
            var lines = ParseInput();
            return lines.Select(x => Tuple.Create(x, rules[0].Match(x))).Where(x => x.Item2.Any(y => y == x.Item1.Length - 1)).Count().ToString();
        }

        protected override string SolvePartTwo()
        {
            var lines = ParseInput();

            rules[8] = OrRule.Parse("42 | 42 8");
            rules[11] = OrRule.Parse("42 31 | 42 11 31");

            return lines.Select(x => Tuple.Create(x, rules[0].Match(x))).Where(x => x.Item2.Any(y => y == x.Item1.Length - 1)).Count().ToString();
        }
    }
}
