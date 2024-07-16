using UnityEngine;
using Zenject;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private CardController _firstCard;
    private CardController _secondCard;
    private bool _canFlip = true;
    private int _currentLevel;
    private GridManager _gridManager;

    [Inject]
    private void Construct(GridManager gridManager)
    {
        _gridManager = gridManager;
    }

    private void Start()
    {
        _currentLevel = PlayerPrefs.GetInt(GameConstants.PLAYER_PREFS_LEVEL_KEY, 4);
        StartLevel(_currentLevel);
    }

    private void StartLevel(int level)
    {
        _gridManager.SetupGrid(level, OnCardClicked);
        StartCoroutine(HideCardsAfterDelay(level));
    }

    private IEnumerator HideCardsAfterDelay(int level)
    {
        float hideDelay = GameConstants.BASE_HIDE_DELAY + (level - 1) * 0.5f;
        yield return new WaitForSeconds(hideDelay); // Время ожидания зависит от уровня

        _gridManager.HideAllCards();
        _canFlip = true; // Разрешаем переворот карт после переворота на обратную сторону
    }

    private void OnCardClicked(CardController card)
    {
        if (!_canFlip || (_firstCard != null && _secondCard != null)) return;

        card.FlipCard();

        if (_firstCard == null)
        {
            _firstCard = card;
        }
        else if (_secondCard == null)
        {
            _secondCard = card;
            _canFlip = false; // Блокируем переворот других карт, пока идет проверка совпадения
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(GameConstants.CHECK_MATCH_DELAY);

        if (_firstCard.GetFrontSprite() == _secondCard.GetFrontSprite())
        {
            _firstCard.Disappear();
            _secondCard.Disappear();
        }
        else
        {
            _firstCard.FlipCard();
            _secondCard.FlipCard();
        }

        _firstCard = null;
        _secondCard = null;
        _canFlip = true; // Разрешаем переворот других карт
    }

    public void ResetLevel()
    {
        _gridManager.ResetGrid();
    }

    private void LevelUp()
    {
        _currentLevel++;
        PlayerPrefs.SetInt(GameConstants.PLAYER_PREFS_LEVEL_KEY, _currentLevel);
        PlayerPrefs.Save();
        ResetLevel();
        StartLevel(_currentLevel);
    }
}
