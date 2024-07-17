using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
  public class SoundButton : MonoBehaviour
  {
    [SerializeField] private Image _soundImage;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;

    private Button _soundButton;
    private SoundManager _soundManager;

    [Inject]
    public void Construct(SoundManager soundManager)
    {
      _soundManager = soundManager;
    }

    private void Awake()
    {
      _soundButton = GetComponent<Button>();
      _soundButton.onClick.AddListener(ToggleSound);
      UpdateButtonSprite();
    }

    private void OnEnable()
    {
      UpdateButtonSprite();
    }

    private void OnDestroy()
    {
      _soundButton.onClick.RemoveListener(ToggleSound);
    }

    private void ToggleSound()
    {
      bool isSoundOn = _soundManager.IsSoundOn;
      _soundManager.SetSound(!isSoundOn);
      UpdateButtonSprite();
    }

    private void UpdateButtonSprite()
    {
      bool isSoundOn = _soundManager.IsSoundOn;
      _soundImage.sprite = isSoundOn ? _soundOnSprite : _soundOffSprite;
    }
  }
}