using UnityEngine;
using Save;
using Zenject;

public class SoundManager : MonoBehaviour
{
  [SerializeField] private AudioClip _flipSound;
  [SerializeField] private AudioClip _matchSound;
  [SerializeField] private AudioClip _winSound;
  [SerializeField] private AudioClip _loseSound;
  [SerializeField] private AudioSource _audioSource;
  public bool IsSoundOn => !_audioSource.mute;
  
  private SaveManager _saveManager;

  [Inject]
  public void Construct(SaveManager saveManager)
  {
    _saveManager = saveManager;
  }

  private void Awake()
  {
    LoadSoundState();
  }

  public void PlayFlipSound()
  {
    _audioSource.PlayOneShot(_flipSound);
  }

  public void PlayMatchSound()
  {
    _audioSource.PlayOneShot(_matchSound);
  }

  public void PlayConclusionSound(bool isWin)
  {
    _audioSource.PlayOneShot(isWin ? _winSound : _loseSound);
  }

  public void SetSound(bool isOn)
  {
    _audioSource.mute = !isOn;
    SaveSoundState(isOn);
  }

  private void SaveSoundState(bool isOn)
  {
    var data = _saveManager.LoadData(SaveType.PlayerPrefs);
    data.SoundOn = isOn;
    _saveManager.SaveData(data, SaveType.PlayerPrefs);
    _saveManager.SaveData(data, SaveType.Json);
    _saveManager.SaveData(data, SaveType.Base64);
  }

  private void LoadSoundState()
  {
    var data = _saveManager.LoadData(SaveType.PlayerPrefs);
    bool isSoundOn = data.SoundOn;
    _audioSource.mute = !isSoundOn;
  }
}