using UnityEngine;
using Zenject;

public class HintSystem : MonoBehaviour
{
  private GameManager _gameManager;

  [Inject]
  public void Construct(GameManager gameManager)
  {
    _gameManager = gameManager;
  }

  public void ShowHint()
  {
    // Логика подсказки
  }
}