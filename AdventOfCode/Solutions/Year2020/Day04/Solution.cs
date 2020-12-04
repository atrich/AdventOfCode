using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace AdventOfCode.Solutions.Year2020
{

    class Day04 : ASolution
    {
        // Yeah I know I should have used regexes

        public Day04() : base(04, 2020, "")
        {
            /*
            this.DebugInput = "ecl:gry pid:860033327 eyr: 2020 hcl:#fffffd" + "\n" +
                              "byr: 1937 iyr: 2017 cid: 147 hgt: 183cm" + "\n" +
                              "\n" +
                              "iyr:2013 ecl: amb cid:350 eyr: 2023 pid: 028048884" + "\n" +
                              "hcl:#cfa07d byr:1929" + "\n" +
                              "\n" +
                              "hcl:#ae17e1 iyr:2013" + "\n" +
                              "eyr: 2024" + "\n" +
                              "ecl: brn pid:760753108 byr: 1931" + "\n" +
                              "hgt: 179cm" + "\n" +
                              "\n" +
                              "hcl:#cfa07d eyr:2025 pid:166559648" + "\n" +
                              "iyr: 2011 ecl: brn hgt:59in" + "\n";
            */
        }

        protected override string SolvePartOne()
        {
            string[] requiredEntries = new string[]{ "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            string[] input = Input.Split("\n");
            List<string> fields = new List<string>();
            int validPasses = 0;

            foreach(var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    // validate the entries
                    bool foundall = true;
                    foreach(string r in requiredEntries)
                    {
                        if (!fields.Contains(r))
                        {
                            foundall = false;
                        }
                    }
                    if (foundall) validPasses++;
                    fields.Clear();
                }
                else
                {
                    var entries = line.Split();
                    foreach (var entry in entries)
                    {
                        fields.Add(entry.Split(':')[0]);
                    }
                }
            }

            // validate final entry
            bool f = true;
            foreach (string r in requiredEntries)
            {
                if (!fields.Contains(r))
                {
                    f = false;
                }
            }
            if (f) validPasses++;

            return validPasses.ToString();
        }

        protected override string SolvePartTwo()
        {
            var requiredEntries = new Dictionary<string, Func<string, bool>> {
                { "byr", (x) => { int y = int.Parse(x); return y >= 1920 && y <= 2002; } },
                { "iyr", (x) => { int y = int.Parse(x); return y >= 2010 && y <= 2020; } },
                { "eyr", (x) => { int y = int.Parse(x); return y >= 2020 && y <= 2030; } },
                { "hgt", (x) => {
                    string unit = x.Substring(x.Length-2, 2);
                    if (unit != "cm" && unit != "in") return false;
                    int val = int.Parse(x.Substring(0, x.Length - 2));
                    if (unit == "cm")
                    {
                        return val >= 150 && val <= 193;
                    }
                    else
                    {
                        return val >= 59 && val <= 76;
                    }
                }},
                { "hcl", (x) => {
                    char[] validchars = new char[] {'a', 'b', 'c', 'd', 'e', 'f'};
                    if (x[0] != '#') return false;
                    for(int i = 1; i < x.Length; i++)
                    {
                        if (!char.IsDigit(x[i]) && !validchars.Contains(x[i]))
                        {
                            return false;
                        }
                    }
                    return true;
                }},
                { "ecl", (x) => {
                    string[] valideye = new string[]{"amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                    return valideye.Contains(x);
                }},
                { "pid", (x) => {
                    if (x.Length != 9) return false;
                    foreach(char c in x)
                    {
                        if(!char.IsDigit(c)) return false;
                    }

                    return true;
                }}
            };

            string[] input = Input.Split("\n");
            List<string> fields = new List<string>();
            int validPasses = 0;

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    // validate the entries
                    bool foundall = true;
                    foreach (string r in requiredEntries.Keys)
                    {
                        if (!fields.Contains(r))
                        {
                            foundall = false;
                        }
                    }
                    if (foundall) validPasses++;
                    fields.Clear();
                }
                else
                {
                    var entries = line.Split();
                    foreach (var entry in entries)
                    {
                        var parts = entry.Split(':');
                        if (parts[0] == "cid") continue;
                        if (requiredEntries[parts[0]](parts[1]))
                        {
                            fields.Add(parts[0]);
                        }
                    }
                }
            }

            // validate final entry
            bool f = true;
            foreach (string r in requiredEntries.Keys)
            {
                if (!fields.Contains(r))
                {
                    f = false;
                }
            }
            if (f) validPasses++;

            return validPasses.ToString();
        }
    }
}
