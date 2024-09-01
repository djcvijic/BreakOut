using System.Collections.Generic;

public class Level
{
    public readonly LevelGrid Grid;
    public readonly List<PowerUpScriptable> AllowedPowerUps;

    public Level(LevelGrid grid, List<PowerUpScriptable> allowedPowerUps)
    {
        Grid = grid;
        AllowedPowerUps = allowedPowerUps;
    }
}

public class LevelGrid
{
    public readonly List<LevelRow> Rows;

    public LevelGrid(List<LevelRow> rows)
    {
        Rows = rows;
    }
}

public class LevelRow
{
    public readonly List<BrickScriptable> Brick;

    public LevelRow(List<BrickScriptable> bricks)
    {
        Brick = bricks;
    }
}