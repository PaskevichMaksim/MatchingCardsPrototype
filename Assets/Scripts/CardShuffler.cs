using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CardShuffler
{
  private readonly List<CardController> _cards;

  public CardShuffler(List<CardController> cards)
  {
    _cards = cards;
  }

  public void ShuffleCardSprites()
  {
    var visibleCards = _cards.Where(card => card.GetComponent<Image>().color.a == 1).ToList();
    var shuffledSprites = visibleCards.Select(card => card.GetFrontSprite()).OrderBy(s => Random.value).ToList();
        
    for (int i = 0; i < visibleCards.Count; i++)
    {
      visibleCards[i].SetFrontSprite(shuffledSprites[i]);
    }
  }

  public IEnumerator AnimateCardsShuffle()
  {
    Vector2[] initialPositions = _cards.Select(card => card.GetComponent<RectTransform>().anchoredPosition).ToArray();
    Vector2[] randomPositions = initialPositions.Select(pos => pos + new Vector2(Random.Range(-200, 200), Random.Range(-200, 200))).ToArray();

    for (int i = 0; i < GameConstants.SHUFFLE_CYCLES; i++)
    {
      foreach (var card in _cards)
      {
        card.GetComponent<RectTransform>().DOAnchorPos(randomPositions[Random.Range(0, randomPositions.Length)], 0.2f)
          .SetEase(Ease.InOutQuad);
      }
      yield return new WaitForSeconds(0.1f);
    }

    for (int i = 0; i < _cards.Count; i++)
    {
      _cards[i].GetComponent<RectTransform>().DOAnchorPos(initialPositions[i], 0.5f)
        .SetEase(Ease.InOutQuad);
    }
    yield return new WaitForSeconds(0.5f);
  }
}