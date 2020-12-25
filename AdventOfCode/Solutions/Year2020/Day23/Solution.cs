using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day23 : ASolution
    {
        class Card
        {
            public int val;
            public int next;
        }

        public Day23() : base(23, 2020, "")
        {
            // DebugInput = "389125467";
        }

        int WrapInc<T>(int i, List<T> coll)
        {
            i++;
            if (i >= coll.Count) return 0;
            return i;
        }

        protected override string SolvePartOne()
        {
            var deck = Input.ToCharArray().Select(x => int.Parse(x.ToString())).ToList();
            var cur = 0;

            for(int t = 0; t < 100; t++)
            {
                var curc = deck[cur];
                var hand = new List<int>();
                var pick = WrapInc(cur, deck);
                for (int i = 0; i < 3; i++)
                {
                    hand.Add(deck[pick]);
                    deck.RemoveAt(pick);
                    if (pick >= deck.Count) pick = 0;
                }

                var x = curc - 1;
                if (x == 0) x = 9;
                while (hand.Contains(x))
                {
                    x--;
                    if (x == 0) x = 9;
                }

                deck.InsertRange(deck.IndexOf(x) + 1, hand);
                cur = WrapInc(deck.IndexOf(curc), deck);
            }

            var p = WrapInc(deck.IndexOf(1), deck);

            var sb = new StringBuilder();
            do
            {
                sb.Append(deck[p]);
                p = WrapInc(p, deck);
            } while (p != deck.IndexOf(1));

            return sb.ToString();
        }

        protected override string SolvePartTwo()
        {
            var inp = Input.ToCharArray().Select(x => int.Parse(x.ToString())).ToList();
            var deck = new Card[1000000];
            var dl = new Dictionary<int, Card>();

            for(int i = 0; i < inp.Count; i++)
            {
                deck[i] = new Card { val = inp[i], next = i + 1 };
                dl[inp[i]] = deck[i];
            }

            for(int i = 9; i < 1000000; i++)
            {
                deck[i] = new Card { val = i + 1, next = i + 1 };
                dl[i + 1] = deck[i];
            }

            deck[999999].next = 0;

            var curc = deck[0];
            var max = deck.Length;

            for (long t = 0; t < 10000000; t++)
            {
                var sh = curc.next;
                var eh = deck[deck[deck[sh].next].next];
                curc.next = eh.next;

                var fv = curc.val - 1;
                if (fv < 1) fv = max;
                 
                while(deck[sh].val == fv || deck[deck[sh].next].val == fv || deck[deck[deck[sh].next].next].val == fv)
                {
                    fv--;
                    if (fv < 1) fv = max;
                }

                var ip = dl[fv];

                eh.next = ip.next;
                ip.next = sh;
                curc = deck[curc.next];
            }

            var p = dl[1];
            long tot = 1;

            for(int i = 0; i < 2; i++)
            {
                p = deck[p.next];
                tot *= p.val;
            }

            return tot.ToString();
        }
    }
}
