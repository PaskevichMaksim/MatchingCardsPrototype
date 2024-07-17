using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace UI
{
  public class BackgroundManager : MonoBehaviour
  {
    [SerializeField] private Image _image;
    [SerializeField] private Color[] _colors;

    private GameManager _gameManager;

    [Inject]
    private void Construct(GameManager gameManager)
    {
      _gameManager = gameManager;
    }

    private void OnEnable()
    {
      _gameManager.OnLevelStarted += SetRandomBackgroundColor;
    }

    private void OnDisable()
    {
      _gameManager.OnLevelStarted -= SetRandomBackgroundColor;
    }

    private void SetRandomBackgroundColor(int level)
    {
      if (_colors.Length == 0) return;

      Color randomColor = _colors[Random.Range(0, _colors.Length)];
      _image.color = randomColor;
    }
  }
}