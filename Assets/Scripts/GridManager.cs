using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Transform _cardField;
    [SerializeField] private Sprite[] _cardSprites;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        
    private CardController.Factory _cardFactory;
    private List<CardController> _cards = new List<CardController>();
    private Vector3[] _originalCardPositions;
    private CardShuffler _cardShuffler;

    [Inject]
    private void Construct(CardController.Factory cardFactory)
    {
        _cardFactory = cardFactory;
    }

    public void SetupGrid(int level, System.Action<CardController> onCardClicked)
    {
        ResetGrid();
        int totalCards = level * GameConstants.LEVEL_MULTIPLIER;

        CalculateGridSize(totalCards, out int rows, out int cols);
        SetupGridLayout(rows, cols);

        var sprites = GetRandomSprites(totalCards / 2);
        sprites = sprites.Concat(sprites).OrderBy(s => Random.value).ToList();

        _originalCardPositions = new Vector3[totalCards];

        for (int i = 0; i < totalCards; i++)
        {
            var card = _cardFactory.Create();
            card.transform.SetParent(_cardField, false);
            card.SetFrontSprite(sprites[i]);
            card.ShowFront(); 
            card.OnCardClicked += onCardClicked;
            _cards.Add(card);
            _originalCardPositions[i] = card.GetComponent<RectTransform>().anchoredPosition;
        }

        _cardShuffler = new CardShuffler(_cards);
    }

    public void ShowAllCards()
    {
        foreach (var card in _cards)
        {
            card.ShowFront();
        }
    }

    public void HideAllCards()
    {
        foreach (var card in _cards)
        {
            card.FlipCard();
        }
    }

    public void ResetGrid()
    {
        foreach (var card in _cards)
        {
            Destroy(card.gameObject);
        }
        _cards.Clear();
    }

    public void ShuffleCardSprites()
    {
        _cardShuffler.ShuffleCardSprites();
    }

    public IEnumerator AnimateCardsShuffle()
    {
        yield return _cardShuffler.AnimateCardsShuffle();
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
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = cols;

        float parentWidth = _cardField.GetComponent<RectTransform>().rect.width;
        float parentHeight = _cardField.GetComponent<RectTransform>().rect.height;

        float cellWidth = (parentWidth - (cols - 1) * _gridLayoutGroup.spacing.x) / cols;
        float cellHeight = (parentHeight - (rows - 1) * _gridLayoutGroup.spacing.y) / rows;

        float cellSize = Mathf.Min(cellWidth, cellHeight);

        _gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
    }

    private List<Sprite> GetRandomSprites(int count)
    {
        return _cardSprites.OrderBy(s => Random.value).Take(count).ToList();
    }

    public bool AllCardsMatched()
    {
        return _cards.All(card => card.IsMatched());
    }
}
