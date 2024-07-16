using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _cardField;
    [SerializeField] private Sprite[] _cardSprites;

    private int _totalCards;
    private CardController _firstCard;
    private CardController _secondCard;
    private CardController.Factory _cardFactory;
    private HintSystem _hintSystem;
    private bool _canFlip = true;
    private int _currentLevel;

    [Inject]
    private void Construct(CardController.Factory cardFactory, HintSystem hintSystem)
    {
        _cardFactory = cardFactory;
        _hintSystem = hintSystem;
    }

    private void Start()
    {
        _currentLevel = PlayerPrefs.GetInt(GameConstants.PLAYER_PREFS_LEVEL_KEY, 1);
        StartLevel(_currentLevel);
    }

    private void StartLevel(int level)
    {
        _totalCards = level * GameConstants.LEVEL_MULTIPLIER;
        SetupCards();
    }

    private void SetupCards()
    {
        foreach (Transform child in _cardField)
        {
            Destroy(child.gameObject);
        }

        CalculateGridSize(_totalCards, out int rows, out int cols);
        SetupGridLayout(rows, cols);

        var sprites = GetRandomSprites(_totalCards / 2);
        sprites = sprites.Concat(sprites).OrderBy(s => Random.value).ToList(); // Перемешиваем спрайты и дублируем их для пар

        for (int i = 0; i < _totalCards; i++)
        {
            var card = _cardFactory.Create();
            card.transform.SetParent(_cardField, false);
            card.SetFrontSprite(sprites[i]);
            card.ShowFront(); // Карточки спавнятся лицевой стороной
        }

        StartCoroutine(HideCardsAfterDelay());
    }

    private IEnumerator HideCardsAfterDelay()
    {
        float hideDelay = GameConstants.BASE_HIDE_DELAY + (_currentLevel - 1) * 0.5f;
        yield return new WaitForSeconds(hideDelay); // Время ожидания зависит от уровня

        foreach (Transform child in _cardField)
        {
            var card = child.GetComponent<CardController>();
            if (card != null)
            {
                card.FlipCard(); // Переворачиваем карточки на обратную сторону
            }
        }

        _canFlip = true; // Разрешаем переворот карт после переворота на обратную сторону
    }

    private void CalculateGridSize(int totalCards, out int rows, out int cols)
    {
        cols = Mathf.CeilToInt(Mathf.Sqrt(totalCards));
        while (totalCards % cols != 0)
        {
            cols++;
        }
        rows = totalCards / cols;
    }

    private void SetupGridLayout(int rows, int cols)
    {
        GridLayoutGroup gridLayoutGroup = _cardField.GetComponent<GridLayoutGroup>();

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = cols;

        float parentWidth = _cardField.GetComponent<RectTransform>().rect.width;
        float parentHeight = _cardField.GetComponent<RectTransform>().rect.height;

        float cellWidth = (parentWidth - (cols - 1) * gridLayoutGroup.spacing.x) / cols;
        float cellHeight = (parentHeight - (rows - 1) * gridLayoutGroup.spacing.y) / rows;

        float cellSize = Mathf.Min(cellWidth, cellHeight);

        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
    }

    private List<Sprite> GetRandomSprites(int count)
    {
        return _cardSprites.OrderBy(s => Random.value).Take(count).ToList();
    }

    public void OnCardClicked(CardController card)
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
        foreach (Transform child in _cardField)
        {
            var card = child.GetComponent<CardController>();
            if (card != null)
            {
                card.ResetCard();
            }
        }
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
