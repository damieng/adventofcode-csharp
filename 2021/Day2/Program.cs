using System;
using System.IO;
using System.Linq;

class Program
{
    enum Direction { forward, up, down };

    struct Movement
    {
        public Direction Direction;
        public int Quantity;
    }

    static void Main()
    {
        var parsedInput = File
            .ReadAllLines("input")
            .Select(i => i.Split(' '))
            .Select(i => new Movement
            {
                Direction = (Direction)Enum.Parse(typeof(Direction), i[0]),
                Quantity = int.Parse(i[1])
            })
            .ToArray();

        Console.WriteLine("Part 1 = " + Part1(parsedInput));
        Console.WriteLine("Part 2 = " + Part2(parsedInput));
    }

    static long Part1(Movement[] input)
    {
        long horizontal = 0;
        long depth = 0;

        foreach (var move in input)
        {
            switch (move.Direction)
            {
                case Direction.forward:
                    horizontal += move.Quantity;
                    break;
                case Direction.up:
                    depth -= move.Quantity;
                    break;
                case Direction.down:
                    depth += move.Quantity;
                    break;
            }
        }

        return horizontal * depth;
    }

    static long Part2(Movement[] input)
    {
        long horizontal = 0;
        long depth = 0;
        long aim = 0;

        foreach (var move in input)
        {
            switch (move.Direction)
            {
                case Direction.forward:
                    horizontal += move.Quantity;
                    depth += aim * move.Quantity;
                    break;
                case Direction.up:
                    aim -= move.Quantity;
                    break;
                case Direction.down:
                    aim += move.Quantity;
                    break;
            }
        }

        return horizontal * depth;
    }
}
