using System.Collections.Generic;

namespace Save
{
  public interface ISaveSystem
  {
    GameData LoadData();
    Dictionary<SaveType, GameData> LoadAllData();
  
    void SaveData(GameData data);
    void SaveAllData(Dictionary<SaveType, GameData> allData);
  }
}