namespace Pacman.Engine;

using System.Collections.Generic;

public record struct PacmanConfiguration(int Columns, int Rows, Point InitialPosition, string Commands, HashSet<Point> Walls);