using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day13 : ASolution
    {

        public Day13() : base(13, 2020, "")
        {
            //DebugInput = "939\n7,13,x,x,59,x,31,19\n";
        }

        protected override string SolvePartOne()
        {
            var lines = Input.SplitByNewline();
            var start = int.Parse(lines[0]);
            var now = start;
            var busses = lines[1].Split(',').Where(s => s != "x").Select(s => int.Parse(s)).ToImmutableSortedSet();
            var targetBus = 0;

            while(targetBus == 0)
            {
                targetBus = busses.Where(b => now % b == 0).FirstOrDefault();
                now++;
            }

            return ((now-1-start)*targetBus).ToString();
        }

        protected override string SolvePartTwo()
        {
            var lines = Input.SplitByNewline();
            var busses = lines[1]
                .Split(',')
                .Select((s, i) => s == "x" ? KeyValuePair.Create(-1, 0) : KeyValuePair.Create(int.Parse(s), i))
                .Where(kv => kv.Key != -1)
                .ToImmutableSortedDictionary();

            var keyBus = busses.Last();
            ulong now = (ulong)keyBus.Key;
            var searchBusses = busses.Remove(keyBus.Key).Reverse();

            var stride = (ulong)keyBus.Key;

            foreach(var bus in searchBusses)
            {
                while ((now - (ulong)keyBus.Value + (ulong)bus.Value) % (ulong)bus.Key != 0)
                {
                    now += stride;
                }
                stride *= (ulong)bus.Key;
            }

            return (now - (ulong)keyBus.Value).ToString();
        }
    }
}
