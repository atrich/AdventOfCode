using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day21 : ASolution
    {
        static Dictionary<string, HashSet<string>> all2ings = new Dictionary<string, HashSet<string>>();
        List<string> inglist = new List<string>();

        public Day21() : base(21, 2020, "")
        {
            //DebugInput = "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)\ntrh fvjkl sbzzf mxmxvkd(contains dairy)\nsqjhc fvjkl(contains soy)\nsqjhc mxmxvkd sbzzf(contains fish)\n";
        }

        public void ParseInput()
        {
            var lines = Input.SplitByNewline();

            foreach (var line in lines)
            {
                var parts = line.Split('(');
                var ings = parts[0].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var alls = parts[1].Substring(9, parts[1].Length - 10).Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                inglist.AddRange(ings);

                foreach (var all in alls)
                {
                    if (!all2ings.ContainsKey(all))
                    {
                        all2ings[all] = ings.ToHashSet();
                    }
                    else
                    {
                        all2ings[all].IntersectWith(ings);
                    }
                }
            }
        }

        protected override string SolvePartOne()
        {
            ParseInput();
            var safe = inglist.ToHashSet().Where(n => !all2ings.Any(kv => kv.Value.Contains(n)));
            return safe.Select(x => inglist.Count(s => s == x)).Sum().ToString();
        }

        protected override string SolvePartTwo()
        {
            ParseInput();
            var bads = inglist.ToHashSet().Where(n => all2ings.Any(kv => kv.Value.Contains(n))).ToArray();
            var knowns = new SortedDictionary<string, string>();

            while(knowns.Count < bads.Length)
            {
                var uniq = all2ings.First(kv => kv.Value.Count == 1);
                var r = uniq.Value.First();
                knowns[uniq.Key] = r;

                foreach (var key in all2ings.Keys)
                {
                    all2ings[key].Remove(r);
                }
            }

            return string.Join(',', knowns.Values);
        }
    }
}
