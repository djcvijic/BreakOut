using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LevelLoader
{
    public Level Load(int levelIndex)
    {
        var levelsJson = Resources.Load<TextAsset>("Data/levels").text;
        var levelsDto = JsonConvert.DeserializeObject<List<LevelDto>>(levelsJson);
        var levelDto = levelsDto[levelIndex];
        return new Level(
            new LevelGrid(levelDto.board.ConvertAll(x => new LevelRow(x.ConvertAll(BrickScriptable.Load)))),
            levelDto.allowedPowerUps.ConvertAll(PowerUpScriptable.Load));
    }
}