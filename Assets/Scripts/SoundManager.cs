using UnityEngine;

public class SoundManager : MonoBehaviour
{
  [SerializeField] private AudioClip _flipSound;
  [SerializeField] private AudioClip _matchSound;
  [SerializeField] private AudioClip _winSound;
  [SerializeField] private AudioClip _loseSound;
  
  private AudioSource _audioSource;

  private void Awake()
  {
    _audioSource = GetComponent<AudioSource>();
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
}