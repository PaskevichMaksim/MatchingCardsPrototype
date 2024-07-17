using UnityEngine;
using Zenject;
using System.Collections;
using UI;

public class HintSystem : MonoBehaviour
{
  private GridManager _gridManager;
  private UIManager _uiManager;

  [Inject]
  private void Construct(GridManager gridManager, UIManager uiManager)
  {
    _gridManager = gridManager;
    _uiManager = uiManager;
  }

  public void ShowHint()
  {
    StartCoroutine(ShuffleAndShowHint());
  }

  private IEnumerator ShuffleAndShowHint()
  {
    _uiManager.SetHintButtonInteractable(false);

    yield return _gridManager.AnimateCardsShuffle();
    _gridManager.ShuffleCardSprites();

    _gridManager.ShowAllCards();
    yield return new WaitForSeconds(GameConstants.HIDE_DELAY_AFTER_SHUFFLE);

    _gridManager.HideAllCards();

    _uiManager.SetHintButtonInteractable(true);
  }
}