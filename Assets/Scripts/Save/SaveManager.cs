using System;
using System.Collections.Generic;
using Zenject;

namespace Save
{
  public class SaveManager
  {
    private readonly Dictionary<SaveType, ISaveSystem> _saveSystems = new Dictionary<SaveType, ISaveSystem>();

    [Inject]
    public void Construct(JsonSaveSystem jsonSaveSystem, PlayerPrefsSaveSystem playerPrefsSaveSystem, Base64SaveSystem base64SaveSystem)
    {
      _saveSystems.Add(SaveType.Json, jsonSaveSystem);
      _saveSystems.Add(SaveType.PlayerPrefs, playerPrefsSaveSystem);
      _saveSystems.Add(SaveType.Base64, base64SaveSystem);
    }

    public void SaveData(GameData data, SaveType saveType)
    {
      if (_saveSystems.TryGetValue(saveType, out ISaveSystem system))
      {
        system.SaveData(data);
      } else
      {
        throw new ArgumentException($"Save type {saveType} is not supported.");
      }
    }

    public GameData LoadData(SaveType saveType)
    {
      if (_saveSystems.TryGetValue(saveType, out ISaveSystem system))
      {
        return system.LoadData();
      }

      throw new ArgumentException($"Save type {saveType} is not supported.");
    }

    public void SaveAllData(GameData data)
    {
      foreach (var saveSystem in _saveSystems.Values)
      {
        saveSystem.SaveData(data);
      }
    }

    public Dictionary<SaveType, GameData> LoadAllData()
    {
      var allData = new Dictionary<SaveType, GameData>();

      foreach (var saveType in _saveSystems.Keys)
      {
        allData[saveType] = _saveSystems[saveType].LoadData();
      }

      return allData;
    }
  }

  public enum SaveType
  {
    Json,
    PlayerPrefs,
    Base64
  }
}