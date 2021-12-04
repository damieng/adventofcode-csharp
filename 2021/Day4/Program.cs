using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var input = File.ReadAllLines("input");

        // Read all the numbers to be drawn
        var draws = input[0].Split(',').Select(i => int.Parse(i)).ToArray();
        var boards = BuildBoards(input);

        {
            // Let's play to win (Part 1)
            var (winningBoard, lastDraw) = PlayToWin(draws, boards);
            Console.WriteLine("Part 1 = " + winningBoard.GetUnmarkedTotal() * lastDraw);
        }

        boards.ForEach(board => board.Reset());

        {
            // Let's play to lose (Part 2)
            var (losingBoard, lastDraw) = PlayToLose(draws, boards);
            Console.WriteLine("Part 2 = " + losingBoard.GetUnmarkedTotal() * lastDraw);
        }
    }

    private static List<Board> BuildBoards(string[] input)
    {
        var boards = new List<Board>();
        for (int i = 1; i < input.Length; i += 6)
        {
            var grid = new int[5][];
            for (int l = 1; l < 6; l++)
                grid[l - 1] = input[i + l].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray();
            boards.Add(new Board(grid));
        }
        return boards;
    }

    private static (Board board, int lastDraw) PlayToWin(int[] draws, List<Board> boards)
    {
        foreach (var draw in draws)
            foreach (var board in boards)
                if (board.Play(draw))
                    return (board, draw);

        throw new InvalidOperationException("No winner");
    }

    private static (Board board, int lastDraw) PlayToLose(int[] draws, List<Board> boards)
    {
        var remaining = new List<Board>(boards);

        foreach (var draw in draws)
            foreach (var board in remaining.ToList())
                if (board.Play(draw))
                {
                    remaining.Remove(board);
                    if (remaining.Count == 0)
                        return (board, draw);
                }

        throw new InvalidOperationException("No loser");
    }
}

class Board
{
    readonly int[][] grid;
    bool[][] hits;
    int[] colHits;
    int[] rowHits;

    public Board(int[][] grid)
    {
        this.grid = grid;
        Reset();
    }

    public void Reset() {
        hits = grid.Select(g => g.Select(h => false).ToArray()).ToArray();
        colHits = new int[5];
        rowHits = new int[5];
    }

    public int GetUnmarkedTotal()
    {
        int score = 0;
        for (int y = 0; y < grid.Length; y++)
            for (int x = 0; x < grid[y].Length; x++)
                if (!hits[y][x])
                    score += grid[y][x];
        return score;
    }

    public bool Play(int draw) => Mark(draw) && IsBingo();

    bool IsBingo() => colHits.Any(c => c == 5) || rowHits.Any(c => c == 5);

    bool Mark(int draw)
    {
        for (int y = 0; y < grid.Length; y++)
            for (int x = 0; x < grid[y].Length; x++)
                if (grid[y][x] == draw)
                {
                    hits[y][x] = true;
                    colHits[x]++;
                    rowHits[y]++;
                    return true;
                }

        return false;
    }
}