using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var navigation = File
            .ReadAllLines("input")
            .ToArray();

        Console.WriteLine("Part1 = " + Part1(navigation));
        Console.WriteLine("Part2 = " + Part2(navigation));
    }

    static readonly Dictionary<char, int> part1ScoreTable = new()
    {
        { ')', 3 },
        { ']', 57 },
        { '}', 1197 },
        { '>', 25137 }
    };

    static readonly char[] openingSymbols = new[] { '(', '[', '{', '<' };
    static readonly char[] closingSymbols = new[] { ')', ']', '}', '>' };

    private static long Part1(string[] navigation)
    {
        long score = 0;

        foreach (var line in navigation)
        {
            var symbolStack = new Stack<char>();
            foreach (var symbol in line)
            {
                if (openingSymbols.Contains(symbol))
                {
                    symbolStack.Push(symbol);
                }
                else
                {
                    var openingSymbol = openingSymbols[Array.IndexOf(closingSymbols, symbol)];
                    if (openingSymbol != symbolStack.Pop())
                    {
                        score += part1ScoreTable[symbol];
                        break;
                    }
}
            }
        }

        return score;
    }

    private static long Part2(string[] navigation)
    {
        var scores = new List<long>();

        foreach (var line in navigation)
        {
            var symbolStack = new Stack<char>();
            var isCorrupt = false;

            foreach (var symbol in line)
            {
                if (openingSymbols.Contains(symbol))
                {
                    symbolStack.Push(symbol);
                }
                else
                {
                    var openingSymbol = openingSymbols[Array.IndexOf(closingSymbols, symbol)];
                    if (openingSymbol != symbolStack.Pop())
                    {
                        isCorrupt = true;
                        break;
                    }
                }
            }

            // Complete an incomplete line and score it
            if (!isCorrupt) {
                long score = 0;
                while (symbolStack.Any()) {
                    var symbol = symbolStack.Pop();
                    var symbolIndex = Array.IndexOf(openingSymbols, symbol);
                    var closingSymbol = closingSymbols[symbolIndex];
                    score *= 5;
                    score += (symbolIndex + 1);
                }
                scores.Add(score);
            }
        }

        return scores.OrderBy(s => s).Skip(scores.Count / 2).First();
    }
}