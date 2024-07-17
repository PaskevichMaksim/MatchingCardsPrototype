using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Save
{
  public class JsonSaveSystem : ISaveSystem
  {
    private readonly string _filePath;

    public JsonSaveSystem(string fileName)
    {
      _filePath = Path.Combine(Application.persistentDataPath, fileName);
    }

    public void SaveData(GameData data)
    {
      File.WriteAllText(_filePath, JsonUtility.ToJson(data));
    }

    public GameData LoadData()
    {
      if (!File.Exists(_filePath)) return new GameData { Level = 1, SoundOn = true };

      var jsonData = File.ReadAllText(_filePath);
      return JsonUtility.FromJson<GameData>(jsonData);
    }

    public void SaveAllData(Dictionary<SaveType, GameData> allData)
    {
      SaveData(allData[SaveType.Json]);
    }

    public Dictionary<SaveType, GameData> LoadAllData()
    {
      return new Dictionary<SaveType, GameData> { { SaveType.Json, LoadData() } };
    }
  }
}