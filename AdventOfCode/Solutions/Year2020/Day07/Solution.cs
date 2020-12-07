using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{
    class Bag
    {
        public string type;
        public List<Tuple<string, int>> contains;
        public bool alreadyTraversed;
        public bool canContainTarget;
    }

    class Day07 : ASolution
    {

        public Day07() : base(07, 2020, "")
        {

        }

        public bool SearchBags(List<Bag> subbags, List<Bag> allBags, string targetBag)
        {
            bool foundtarget = false;
            foreach(var bag in subbags)
            {
                if (bag.alreadyTraversed)
                {
                    if (bag.canContainTarget) foundtarget = true;
                    continue;
                }

                bag.alreadyTraversed = true;

                if (bag.type == targetBag)
                {
                    bag.canContainTarget = false;
                    continue;
                }

                List<Bag> subBags = new List<Bag>();
                foreach(var bg in bag.contains)
                {
                    if (bg.Item1 == targetBag)
                    {
                        bag.canContainTarget = true;
                    }
                    else
                    {
                        subBags.Add(allBags.Find(b => b.type == bg.Item1));
                    }
                }

                bag.canContainTarget = bag.canContainTarget || SearchBags(subBags, allBags, targetBag);
                if (bag.canContainTarget) foundtarget = true;
            }

            return foundtarget;
        }

        public int CountBagsInside(string targetBag, List<Bag> allBags)
        {
            int total = 0;
            var bag = allBags.Find(b => b.type == targetBag);
            foreach(var bt in bag.contains)
            {
                total += CountBagsInside(bt.Item1, allBags) * bt.Item2;
            }

            return bag.contains.Sum(x => x.Item2) + total;
        }

        protected override string SolvePartOne()
        {
            string[] lines = Input.SplitByNewline();
            var bags = new List<Bag>();

            foreach(var line in lines)
            {
                // parse bag input
                string bagType = line.Substring(0, line.IndexOf(" bags"));
                var otherBags = new List<Tuple<string, int>>();
                var otherBagList = line.Substring(line.IndexOf("contain") + 8);

                if (!otherBagList.Contains("no other bags"))
                {
                    foreach (var s in otherBagList.Split(','))
                    {
                        var subBag = s.Trim();
                        var subBagCount = int.Parse(subBag.Substring(0, subBag.IndexOf(' ')));
                        var subBagType = subBag.Substring(subBag.IndexOf(' ') + 1, subBag.IndexOf(" bag") - subBag.IndexOf(' ') - 1);
                        otherBags.Add(Tuple.Create(subBagType, subBagCount));
                    }
                }
                bags.Add(new Bag { type = bagType, contains = otherBags, alreadyTraversed = false, canContainTarget = false });
            }

            SearchBags(bags, bags, "shiny gold");
            int total = bags.Count(b => b.canContainTarget);
            return total.ToString();
        }

        protected override string SolvePartTwo()
        {
            string[] lines = Input.SplitByNewline();
            var bags = new List<Bag>();

            foreach (var line in lines)
            {
                // parse bag input
                string bagType = line.Substring(0, line.IndexOf(" bags"));
                var otherBags = new List<Tuple<string, int>>();
                var otherBagList = line.Substring(line.IndexOf("contain") + 8);

                if (!otherBagList.Contains("no other bags"))
                {
                    foreach (var s in otherBagList.Split(','))
                    {
                        var subBag = s.Trim();
                        var subBagCount = int.Parse(subBag.Substring(0, subBag.IndexOf(' ')));
                        var subBagType = subBag.Substring(subBag.IndexOf(' ') + 1, subBag.IndexOf(" bag") - subBag.IndexOf(' ') - 1);
                        otherBags.Add(Tuple.Create(subBagType, subBagCount));
                    }
                }
                bags.Add(new Bag { type = bagType, contains = otherBags, alreadyTraversed = false, canContainTarget = false });
            }

            return CountBagsInside("shiny gold", bags).ToString();
        }
    }
}
