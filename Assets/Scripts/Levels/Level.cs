using System.Collections.Generic;

public class Level
{
    private readonly LevelGrid _grid;
    private readonly List<PowerUpScriptable> _allowedPowerUps;

    public Level(LevelGrid grid, List<PowerUpScriptable> allowedPowerUps)
    {
        _grid = grid;
        _allowedPowerUps = allowedPowerUps;
    }
}

public class LevelGrid
{
    private readonly List<LevelRow> _rows;

    public LevelGrid(List<LevelRow> rows)
    {
        _rows = rows;
    }
}

public class LevelRow
{
    private readonly List<BrickScriptable> _bricks;

    public LevelRow(List<BrickScriptable> bricks)
    {
        _bricks = bricks;
    }
}