using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public class CardController : MonoBehaviour
{
    [SerializeField] private Sprite _backSprite;
    [SerializeField] private Button _cardButton;
    
    private Image _cardImage;
    private Sprite _frontSprite;
    private bool _isFlipped;
    private bool _isMatched;
    private bool _turning;
    private GameManager _gameManager;
    private Sequence _sequence;

    [Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Awake()
    {
        _cardImage = GetComponent<Image>();
        _cardButton = GetComponent<Button>();
        _cardButton.onClick.AddListener(OnClick);
        ShowBack();
    }

    private void OnDestroy()
    {
        _cardButton.onClick.RemoveListener(OnClick);
    }

    public void OnClick()
    {
        if (_turning || _isMatched || _isFlipped)
        {
            return;
        }

        _gameManager.OnCardClicked(this);
    }

    public void FlipCard()
    {
        if (_sequence != null && _sequence.IsPlaying())
        {
            return;
        }

        _isFlipped = !_isFlipped;
        _turning = true;

        if (_isFlipped)
        {
            DoFlip(_backSprite, _frontSprite);
        }
        else
        {
            DoFlip(_frontSprite, _backSprite);
        }
    }

    private void DoFlip(Sprite hideSprite, Sprite showSprite)
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(_cardImage.transform.DOScaleX(0, GameConstants.FLIP_DURATION))
            .AppendCallback(() =>
            {
                _cardImage.sprite = showSprite;
            })
            .Append(_cardImage.transform.DOScaleX(1, GameConstants.FLIP_DURATION))
            .AppendCallback(() =>
            {
                _turning = false;
            });
    }

    public void ShowBack()
    {
        _cardImage.sprite = _backSprite;
        _isFlipped = false;
    }

    public void ShowFront()
    {
        _cardImage.sprite = _frontSprite;
        _isFlipped = true;
    }

    public bool IsFlipped()
    {
        return _isFlipped;
    }

    public void SetFlipped(bool flipped)
    {
        _isFlipped = flipped;
        if (flipped)
        {
            ShowFront();
        }
        else
        {
            ShowBack();
        }
    }

    public void SetFrontSprite(Sprite sprite)
    {
        _frontSprite = sprite;
    }

    public Sprite GetFrontSprite()
    {
        return _frontSprite;
    }

    public void Disappear()
    {
        _isMatched = true;
        _sequence = DOTween.Sequence();
        _sequence.Append(_cardImage.DOFade(0, GameConstants.FLIP_DURATION));
    }

    public void ResetCard()
    {
        _isMatched = false;
        _cardImage.color = new Color(_cardImage.color.r, _cardImage.color.g, _cardImage.color.b, 1);
        ShowBack();
    }

    public class Factory : PlaceholderFactory<CardController> { }
}
