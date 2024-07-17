using System.Collections.Generic;
using UnityEngine;
namespace Save
{
  public class PlayerPrefsSaveSystem : ISaveSystem
  {
    public void SaveData(GameData data)
    {
      PlayerPrefs.SetInt(GameConstants.PLAYER_PREFS_LEVEL_KEY, data.Level);
      PlayerPrefs.SetInt(GameConstants.SOUND_PREF_KEY, data.SoundOn ? 1 : 0);
      PlayerPrefs.Save();
    }

    public GameData LoadData()
    {
      var data = new GameData
      {
        Level = PlayerPrefs.GetInt(GameConstants.PLAYER_PREFS_LEVEL_KEY, 1),
        SoundOn = PlayerPrefs.GetInt(GameConstants.SOUND_PREF_KEY, 1) == 1
      };

      return data;
    }

    public void SaveAllData(Dictionary<SaveType, GameData> allData)
    {
      SaveData(allData[SaveType.PlayerPrefs]);
    }

    public Dictionary<SaveType, GameData> LoadAllData()
    {
      return new Dictionary<SaveType, GameData> { { SaveType.PlayerPrefs, LoadData() } };
    }
  }
}