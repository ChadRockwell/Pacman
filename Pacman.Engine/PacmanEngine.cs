namespace Pacman.Engine;

using System;
using System.Collections.Generic;
using System.Linq;

public sealed class PacmanEngine
{
    private readonly string _commands;
    private readonly Point _initialPosition;
    private readonly HashSet<Point> _allWalls;
    private readonly bool _invalidInitialPosition;

    public PacmanEngine(
        int columns,
        int rows,
        string commands,
        Point initialPosition,
        HashSet<Point> walls)
    {
        _commands = commands;
        _initialPosition = initialPosition;

        _invalidInitialPosition = initialPosition.X > columns || initialPosition.Y > rows;

        // calculate the walls
        _allWalls = CalculateAllWalls(columns, rows, walls);
    }

    public (Point FinalPosition, int CountOfCoins) Execute()
    {
        if (_invalidInitialPosition)
        {
            return (Point.InvalidPoint, 0);
        }

        var current = _initialPosition;

        HashSet<Point> coins = new();

        // run the board
        foreach (var command in _commands)
        {
            var next = command switch
            {
                'N' => new Point(current.X, current.Y + 1),
                'S' => new Point(current.X, current.Y - 1),
                'E' => new Point(current.X + 1, current.Y),
                'W' => new Point(current.X - 1, current.Y),
                _ => throw new NotImplementedException()
            };

            // check if wall
            if (!_allWalls.Contains(next))
            {
                // initial position doesn't have a coin
                if (next != _initialPosition)
                    coins.Add(next);

                current = next;
            }
        }

        return (current, coins.Count);
    }

    private static HashSet<Point> CalculateAllWalls(int columns, int rows, HashSet<Point> walls)
    {
        // find edges
        var left = Enumerable.Range(0, rows).Select(s => new Point(-1, s));
        var right = Enumerable.Range(0, rows).Select(s => new Point(columns, s));
        var top = Enumerable.Range(0, columns).Select(s => new Point(s, rows));
        var bottom = Enumerable.Range(0, columns).Select(s => new Point(s, -1));

        // get all walls
        return walls.Union(left).Union(right).Union(top).Union(bottom).ToHashSet();
    }
}
