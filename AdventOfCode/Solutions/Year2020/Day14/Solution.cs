using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Sources;

namespace AdventOfCode.Solutions.Year2020
{

    class Day14 : ASolution
    {

        public Day14() : base(14, 2020, "")
        {
            //DebugInput = "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X\nmem[8] = 11\nmem[7] = 101\nmem[8] = 0\n";
            //DebugInput = "mask = 000000000000000000000000000000X1001X\nmem[42] = 100\nmask = 00000000000000000000000000000000X0XX\nmem[26] = 1\n";
        }

        long applyMask(long val, IEnumerable<KeyValuePair<int, char>> mask)
        {
            var bv = Convert.ToString(val, 2);
            foreach(var mv in mask)
            {
                if (mv.Key < bv.Length && mv.Value == bv[bv.Length - mv.Key - 1])
                {
                    continue;
                }

                var adj = (long)Math.Pow(2, mv.Key);

                if (mv.Value == '0' && mv.Key < bv.Length)
                {
                    val -= adj;
                }
                else if (mv.Value == '1')
                {
                    val += adj;
                }
            }

            Debug.Assert(val >= 0);

            return val;
        }

        List<long> permuteAddresses(long addr, IEnumerable<int> bits)
        {
            var l = new List<long>();
            if (bits.Count() > 0)
            {
                var cur = bits.Last();
                var adj = (long)Math.Pow(2, cur);
                var next = bits.SkipLast(1);

                l.AddRange(permuteAddresses(addr, next));

                if ((addr & adj) == adj)
                {
                    l.AddRange(permuteAddresses(addr - adj, next));
                }
                else
                {
                    l.AddRange(permuteAddresses(addr + adj, next));
                }
            }
            else
            {
                l.Add(addr);
            }

            return l;
        }

        long[] getTargetAddresses(long addr, Dictionary<int, char> mask)
        {
            var ma = applyMask(addr, mask.Where(x => x.Value == '1'));
            var ta = permuteAddresses(ma, mask.Where(x => x.Value == 'X').Select(x => x.Key));
            return ta.ToArray();
        }

        protected override string SolvePartOne()
        {
            var lines = Input.SplitByNewline();
            var memory = new Dictionary<int, long>(); // map of mem addresses to values
            var mask = new Dictionary<int, char>(); // map of bit positions to overwrite value

            foreach(var line in lines)
            {
                var parts = line.Split('=', StringSplitOptions.TrimEntries);
                var command = parts[0];

                if (command == "mask")
                {
                    mask.Clear();
                    var len = parts[1].Length;
                    for (int i = 0; i < len; i++)
                    {
                        var c = parts[1][len - i - 1];
                        if(c != 'X')
                        {
                            mask[i] = c;
                        }
                    }
                }
                else
                {
                    var address = int.Parse(command.Substring(command.IndexOf('[') + 1, command.IndexOf(']') - command.IndexOf('[') - 1));
                    memory[address] = applyMask(long.Parse(parts[1]), mask);
                }
            }

            return memory.Sum(kv => kv.Value).ToString();
        }

        protected override string SolvePartTwo()
        {
            var lines = Input.SplitByNewline();
            var memory = new Dictionary<long, long>(); // map of mem addresses to values
            var mask = new Dictionary<int, char>(); // map of bit positions to overwrite value

            foreach(var line in lines)
            {
                var parts = line.Split('=', StringSplitOptions.TrimEntries);
                var command = parts[0];

                if (command == "mask")
                {
                    mask.Clear();
                    var len = parts[1].Length;
                    for (int i = 0; i < len; i++)
                    {
                        var c = parts[1][len - i - 1];
                        mask[i] = c;
                    }
                }
                else
                {
                    var address = int.Parse(command.Substring(command.IndexOf('[') + 1, command.IndexOf(']') - command.IndexOf('[') - 1));
                    var addresses = getTargetAddresses(address, mask);
                    var val = long.Parse(parts[1]);

                    foreach (var a in addresses)
                    {
                        memory[a] = val;
                    }
                }
            }

            return memory.Sum(kv => kv.Value).ToString();
        }
    }
}
