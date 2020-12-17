using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day16 : ASolution
    {

        public Day16() : base(16, 2020, "")
        {

        }

        public Tuple<int, string[]> ProcessTickets(string[] tickets, Dictionary<string, Tuple<int, int>[]> ruleset)
        {
            int err = 0;
            var validtickets = new List<string>();

            foreach (var ticket in tickets)
            {
                var values = ticket.Split(',').Select(int.Parse);
                var anyerr = false;
                foreach (var value in values)
                {
                    var found = false;

                    foreach (var rule in ruleset)
                    {
                        foreach (var range in rule.Value)
                        {
                            if (value >= range.Item1 && value <= range.Item2)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (found == true) break;
                    }

                    if (!found)
                    {
                        err += value;
                        anyerr = true;
                    }
                }

                if (!anyerr)
                {
                    validtickets.Add(ticket);
                }
            }

            return Tuple.Create(err, validtickets.ToArray());
        }

        public Dictionary<string, Tuple<int, int>[]> CreateRuleset(string[] rules)
        {
            var ruleset = new Dictionary<string, Tuple<int, int>[]>(); // maps a field to the set of ranges
            foreach (var rule in rules)
            {
                var name = rule.Substring(0, rule.IndexOf(':'));
                ruleset[name] = rule
                    .Substring(rule.IndexOf(':') + 1)
                    .Split("or", 2, StringSplitOptions.TrimEntries)
                    .Select(x => x.Split('-').Select(int.Parse).ToArray())
                    .Select(x => Tuple.Create(x[0], x[1]))
                    .ToArray();
            }

            return ruleset;
        }

        protected override string SolvePartOne()
        {
            var lines = Input.SplitByNewline();
            var rules = lines.AsSpan(0, Array.IndexOf(lines, "your ticket:")).ToArray();
            var tickets = lines.AsSpan(Array.IndexOf(lines, "nearby tickets:") + 1).ToArray();
            var ruleset = CreateRuleset(rules);
            var res = ProcessTickets(tickets, ruleset);
            return res.Item1.ToString();
        }

        protected override string SolvePartTwo()
        {
            var lines = Input.SplitByNewline();
            var rules = lines.AsSpan(0, Array.IndexOf(lines, "your ticket:")).ToArray();
            var myticket = lines[Array.IndexOf(lines, "your ticket:") + 1].Split(',').Select(int.Parse).ToArray();
            var tickets = lines.AsSpan(Array.IndexOf(lines, "nearby tickets:") + 1).ToArray();
            var ruleset = CreateRuleset(rules);
            var validtix = ProcessTickets(tickets, ruleset).Item2.Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();
            var indexmap = new Dictionary<string, int[]>();

            foreach(var rule in ruleset)
            {
                var possibles = Enumerable.Repeat(true, validtix[0].Length).ToArray();
                int i = 0;
                while(possibles.Count(x => x == true) > 1 && i < possibles.Length)
                {
                    foreach(var ticket in validtix)
                    {
                        bool found = false;
                        foreach (var range in rule.Value)
                        {
                            var value = ticket[i];
                            if (value >= range.Item1 && value <= range.Item2)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            possibles[i] = false;
                            break;
                        }
                    }

                    i++;
                }

                indexmap[rule.Key] = possibles.Select((x, i) => x ? i : -1).Where(x => x != -1).ToArray();
            }

            var finalmap = new Dictionary<string, int>();
            IEnumerable<KeyValuePair<string, int[]>> m = indexmap;

            while(finalmap.Keys.Count < m.Count())
            {
                var uniq = m.First(x => x.Value.Length == 1);
                var v = uniq.Value.First();
                finalmap[uniq.Key] = v;

                // remove all v entries
                m = m.Select(x => KeyValuePair.Create(x.Key, x.Value.Where(i => i != v).ToArray()));
            }

            var total = finalmap.Where(x => x.Key.StartsWith("departure")).Select(x => (ulong)myticket[x.Value]).Aggregate(1ul, (x, y) => x * y);
            return total.ToString();
        }
    }
}
