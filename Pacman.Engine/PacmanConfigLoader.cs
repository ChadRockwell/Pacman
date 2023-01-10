namespace Pacman.Engine;

using System.Collections.Generic;

public static class PacmanConfigLoader
{
    public async static Task<PacmanConfiguration> Load(Stream stream)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));

        StreamReader reader = new(stream);

        string? line;
        int lineNumber = 0;

        int columns = 0;
        int rows = 0;
        Point initialPosition = new(0, 0);
        string? commands = string.Empty;
        HashSet<Point> walls = new();

        while ((line = await reader.ReadLineAsync()) != null)
        {
            switch (lineNumber)
            {
                case 0:
                    ParseColumnsAndRows(line, out columns, out rows);
                    break;
                case 1:
                    ParsePoint(line, out initialPosition);
                    break;
                case 2:
                    commands = line;
                    break;
                default:
                    ParsePoint(line, out Point wallPosition);
                    walls.Add(wallPosition);
                    break;
            }

            lineNumber++;
        }

        return new PacmanConfiguration(columns, rows, initialPosition, commands, walls);
    }

    private static void ParseColumnsAndRows(string? line, out int columns, out int rows)
    {
        var array = line!.Split('\u0020');

        if (array.Length != 2)
        {
            throw new ArgumentException($"Invalid columns and rows specified: {line}");
        }

        columns = TryParseInt(array[0]);
        rows = TryParseInt(array[1]);
    }

    private static void ParsePoint(string? line, out Point initialPosition)
    {
        var array = line!.Split('\u0020');

        if (array.Length != 2)
        {
            throw new ArgumentException($"Invalid point specified:{line}");
        }

        var x = TryParseInt(array[0]);
        var y = TryParseInt(array[1]);

        initialPosition = new(x, y);
    }

    private static int TryParseInt(string s)
    {
        if (!int.TryParse(s, out int returnValue))
        {
            throw new ArgumentException($"Invalid integer specified:{s}");
        }

        return returnValue;
    }
}
