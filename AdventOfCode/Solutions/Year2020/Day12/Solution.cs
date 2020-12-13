using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    class Day12 : ASolution
    {
        Dictionary<char, int> faceToDir = new Dictionary<char, int> { { 'N', 0 }, { 'E', 90 }, { 'S', 180 }, { 'W', 270 } };

        public Day12() : base(12, 2020, "")
        {
            //DebugInput = "F10\nN3\nF7\nR90\nF11\n";
        }

        public Point move(Point pos, char dir, int mag)
        {
            switch (dir)
            {
                case 'N':
                    return new Point(pos.X, pos.Y + mag);

                case 'E':
                    return new Point(pos.X + mag, pos.Y);

                case 'S':
                    return new Point(pos.X, pos.Y - mag);

                case 'W':
                    return new Point(pos.X - mag, pos.Y);
            }

            throw new ArgumentException();
        }

        public char rot(char face, int mag)
        {
            int dir = faceToDir[face];
            dir += mag;

            if (dir < 0) dir += 360;
            if (dir >= 360) dir -= 360;

            return faceToDir.Where(kv => kv.Value == dir).First().Key;
        }

        public Point rot(Point way, int mag)
        {
            if (mag < 0) mag += 360;

            switch (mag)
            {
                case 90:
                    return new Point(way.Y, -way.X);

                case 180:
                    return new Point(-way.X, -way.Y);

                case 270:
                    return new Point(-way.Y, way.X);
            }

            throw new ArgumentException();
        }

        protected override string SolvePartOne()
        {
            var dirs = Input.SplitByNewline();
            var face = 'E';
            var pos = new Point(0, 0);

            foreach (var line in dirs)
            {
                var dir = line[0];
                var mag = int.Parse(line.Substring(1));

                switch (dir)
                {
                    case 'L':
                        face = rot(face, -mag);
                        break;

                    case 'R':
                        face = rot(face, mag);
                        break;

                    case 'F':
                        pos = move(pos, face, mag);
                        break;

                    default:
                        pos = move(pos, dir, mag);
                        break;
                }
            }

            return (Math.Abs(pos.X) + Math.Abs(pos.Y)).ToString();
        }

        protected override string SolvePartTwo()
        {
            var dirs = Input.SplitByNewline();

            var pos = new Point(0, 0);
            var way = new Point(10, 1);

            foreach (var line in dirs)
            {
                var dir = line[0];
                var mag = int.Parse(line.Substring(1));

                switch (dir)
                {
                    case 'L':
                        way = rot(way, -mag);
                        break;

                    case 'R':
                        way = rot(way, mag);
                        break;

                    case 'F':
                        pos = new Point(pos.X + (way.X * mag), pos.Y + (way.Y * mag));
                        break;

                    default:
                        way = move(way, dir, mag);
                        break;
                }
            }

            return (Math.Abs(pos.X) + Math.Abs(pos.Y)).ToString();
        }
    }
}
