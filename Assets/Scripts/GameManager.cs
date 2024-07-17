using UnityEngine;
using Zenject;
using System;
using System.Collections;
using UI;

public class GameManager : MonoBehaviour
{
    public event Action<int> OnLevelStarted;
    public event Action<float> OnStartTimer;
    public event Action<bool> OnGameEnd;

    private CardController _firstCard;
    private CardController _secondCard;
    private bool _canFlip = true;
    private int _currentLevel;
    private GridManager _gridManager;
    private Coroutine _hideCardsCoroutine;

    public int CurrentLevel => _currentLevel;

    [Inject]
    private void Construct(GridManager gridManager, UIManager uiManager)
    {
        _gridManager = gridManager;
    }

    private void Start()
    {
        _currentLevel = PlayerPrefs.GetInt(GameConstants.PLAYER_PREFS_LEVEL_KEY, 1);
        StartLevel(_currentLevel);
    }

    public void StartGame()
    {
        StartLevel(_currentLevel);
    }
    
    private void StartLevel(int level)
    {
        OnLevelStarted?.Invoke(level);
        if (_hideCardsCoroutine != null)
        {
            StopCoroutine(_hideCardsCoroutine);
        }
        _gridManager.SetupGrid(level, OnCardClicked);
        _hideCardsCoroutine = StartCoroutine(HideCardsAfterDelay(level));
    }

    private IEnumerator HideCardsAfterDelay(int level)
    {
        float hideDelay = GameConstants.BASE_HIDE_DELAY + (level - 1) * 0.5f;
        yield return new WaitForSeconds(hideDelay);

        _gridManager.HideAllCards();
        _canFlip = true;

        float levelDuration = GameConstants.BASE_LEVEL_DURATION + (level - 1) * GameConstants.LEVEL_DURATION_INCREMENT;
        OnStartTimer?.Invoke(levelDuration);
    }

    private void OnCardClicked(CardController card)
    {
        if (!_canFlip || (_firstCard != null && _secondCard != null)) return;

        card.FlipCard();

        if (_firstCard == null)
        {
            _firstCard = card;
        } else if (_secondCard == null)
        {
            _secondCard = card;
            _canFlip = false;
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
            CheckWinCondition();
        } else
        {
            _firstCard.FlipCard();
            _secondCard.FlipCard();
        }

        _firstCard = null;
        _secondCard = null;
        _canFlip = true;
    }

    private void CheckWinCondition()
    {
        if (_gridManager.AllCardsMatched())
        {
            OnGameEnd?.Invoke(true);
        }
    }

    public void ResetLevel()
    {
        _gridManager.ResetGrid();
        StartLevel(_currentLevel);
    }

    public void StartNextLevel()
    {
        _currentLevel++;
        PlayerPrefs.SetInt(GameConstants.PLAYER_PREFS_LEVEL_KEY, _currentLevel);
        PlayerPrefs.Save();
        ResetLevel();
        StartLevel(_currentLevel);
    }

    public void OnTimeOut()
    {
        OnGameEnd?.Invoke(false);
    }
}
