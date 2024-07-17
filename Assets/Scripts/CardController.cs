using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using System;

public class CardController : MonoBehaviour
{
    public event Action<CardController> OnCardClicked;
    [SerializeField] private Sprite _backSprite;
    [SerializeField] private Button _cardButton;
    private Image _cardImage;
    private Sprite _frontSprite;
    private bool _isFlipped;
    private bool _isMatched;
    private bool _turning;
    private Sequence _sequence;

    private SoundManager _soundManager;

    [Inject]
    public void Construct(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }

    private void Awake()
    {
        _cardImage = GetComponent<Image>();
        _cardButton = GetComponent<Button>();
        _cardButton.onClick.AddListener(HandleClick);
        ShowBack();
    }

    private void OnDestroy()
    {
        _cardButton.onClick.RemoveListener(HandleClick);
    }

    private void HandleClick()
    {
        if (_turning || _isMatched || _isFlipped)
        {
            return;
        }

        OnCardClicked?.Invoke(this);
    }

    public void FlipCard()
    {
        if (_sequence != null && _sequence.IsPlaying())
        {
            return;
        }

        _soundManager.PlayFlipSound();
        _isFlipped = !_isFlipped;
        _turning = true;

        if (_isFlipped)
        {
            DoFlip(_backSprite, _frontSprite);
        } else
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

    private void ShowBack()
    {
        _cardImage.sprite = _backSprite;
        _isFlipped = false;
    }

    public void ShowFront()
    {
        _cardImage.sprite = _frontSprite;
        _isFlipped = true;
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

    public bool IsMatched()
    {
        return _isMatched;
    }

    public class Factory : PlaceholderFactory<CardController> { }
}
