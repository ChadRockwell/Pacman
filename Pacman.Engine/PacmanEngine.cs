namespace Pacman.Engine;

using System;
using System.Collections.Generic;

public sealed class PacmanEngine
{
    private readonly int _columns;
    private readonly string _commands;
    private readonly Point _initialPosition;
    private readonly bool _invalidInitialPosition;
    private readonly int _rows;
    private readonly HashSet<Point> _walls;

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
        _columns = columns;
        _rows = rows;
        _walls = walls;
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
            if (!IsWall(next))
            {
                // initial position doesn't have a coin
                if (next != _initialPosition)
                    coins.Add(next);

                current = next;
            }
        }

        return (current, coins.Count);
    }

    private bool IsWall(Point current)
    {
        int x = current.X;
        int y = current.Y;

        return x < 0 || y < 0 || x >= _columns || y >= _rows || _walls.Contains(current);
    }
}
