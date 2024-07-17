using System.Collections.Generic;
using UnityEngine;
namespace Save
{
  public class Base64SaveSystem : ISaveSystem
  {
    public void SaveData(GameData data)
    {
      string jsonData = JsonUtility.ToJson(data);
      string base64Data = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonData));
      PlayerPrefs.SetString(GameConstants.BASE_64_SAVE_KEY, base64Data);
      PlayerPrefs.Save();
    }

    public GameData LoadData()
    {
      string base64Data = PlayerPrefs.GetString(GameConstants.BASE_64_SAVE_KEY, string.Empty);

      if (string.IsNullOrEmpty(base64Data))
      {
        return new GameData { Level = 1, SoundOn = true };
      }

      string jsonData = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(base64Data));
      return JsonUtility.FromJson<GameData>(jsonData);
    }

    public void SaveAllData(Dictionary<SaveType, GameData> allData)
    {
      SaveData(allData[SaveType.Base64]);
    }

    public Dictionary<SaveType, GameData> LoadAllData()
    {
      return new Dictionary<SaveType, GameData> { { SaveType.Base64, LoadData() } };
    }
  }
}