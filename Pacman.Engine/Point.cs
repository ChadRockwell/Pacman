namespace Pacman.Engine;

public record Point(int X, int Y)
{
    public static Point InvalidPoint => new(-1, -1);
};
