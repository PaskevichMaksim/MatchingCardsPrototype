using System;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
  public class SoundButton : MonoBehaviour
  {
    [SerializeField] private Image _soundImage;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;

    private Button _soundButton; 
    private bool _isSoundOn;

    private void Awake()
    {
      _soundButton = GetComponent<Button>();
    
      _soundButton.onClick.AddListener(ToggleSound);
      LoadSoundState();
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
      _isSoundOn = !_isSoundOn;
      AudioListener.volume = _isSoundOn ? 1 : 0;
      UpdateButtonSprite();
      SaveSoundState();
    }

    private void SaveSoundState()
    {
      PlayerPrefs.SetInt(GameConstants.SOUND_PREF_KEY, _isSoundOn ? 1 : 0);
      PlayerPrefs.Save();
    }

    private void LoadSoundState()
    {
      _isSoundOn = PlayerPrefs.GetInt(GameConstants.SOUND_PREF_KEY, 1) == 1;
      AudioListener.volume = _isSoundOn ? 1 : 0;
    }

    private void UpdateButtonSprite()
    {
      _soundImage.sprite = _isSoundOn ? _soundOnSprite : _soundOffSprite;
    }
  }
}