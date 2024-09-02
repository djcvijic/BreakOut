using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public static class LevelLoader
{
    public static Level Load(int levelIndex)
    {
        var levelsDto = LoadAllLevelsDto();
        var levelDto = levelsDto[levelIndex];
        return LevelFromDto(levelDto);
    }

    public static List<Level> LoadAll()
    {
        var levelsDto = LoadAllLevelsDto();
        return levelsDto.ConvertAll(LevelFromDto);
    }

    public static int CountAllLevels()
    {
        var levelsDto = LoadAllLevelsDto();
        return levelsDto.Count;
    }

    private static List<LevelDto> LoadAllLevelsDto()
    {
        var levelsJson = Resources.Load<TextAsset>("Data/levels").text;
        return JsonConvert.DeserializeObject<List<LevelDto>>(levelsJson);
    }

    private static Level LevelFromDto(LevelDto levelDto)
    {
        return new Level(
            new LevelGrid(levelDto.board.ConvertAll(x => new LevelRow(x.ConvertAll(BrickScriptable.Load)))),
            levelDto.allowedPowerUps.ConvertAll(PowerUpScriptable.Load));
    }
}

public class LevelDto
{
    public List<List<int>> board;
    public List<string> allowedPowerUps;
}