using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace UI
{
  public class ConclusionMenu : BaseMenu
  {
    [SerializeField] private TextMeshProUGUI _conclusionText;
    [SerializeField] private Button _conclusionButton;
    [SerializeField] private Button _conclusionMenuButton;

    private bool _isWin;
    
    private GameManager _gameManager;
    private UIManager _uiManager;
    private SoundManager _soundManager;

    [Inject]
    public void Construct(GameManager gameManager, UIManager uiManager, SoundManager soundManager)
    {
      _gameManager = gameManager;
      _uiManager = uiManager;
      _soundManager = soundManager;
    }

    public override void Awake()
    {
      _conclusionButton.onClick.AddListener(OnConclusionButtonClicked);
      _conclusionMenuButton.onClick.AddListener(OnMenuButtonClicked);
      base.Awake();
    }

    private void OnDestroy()
    {
      _conclusionButton.onClick.RemoveListener(OnConclusionButtonClicked);
      _conclusionMenuButton.onClick.RemoveListener(OnMenuButtonClicked);
    }

    private void OnConclusionButtonClicked()
    {
      gameObject.SetActive(false);
      if (_isWin)
      {
        _gameManager.StartNextLevel();
      } else
      {
        _gameManager.ResetLevel();
      }
    }

    private void OnMenuButtonClicked()
    {
      _uiManager.ShowMainMenu();
    }

    public void ShowConclusionPanel(bool isWin)
    {
      _isWin = isWin;
      _conclusionText.text = isWin ? "You Win!" : "Time's up! You lost.";
      _conclusionButton.GetComponentInChildren<TextMeshProUGUI>().text = isWin ? "Next Level" : "Restart Level";
      _soundManager.PlayConclusionSound(isWin);
      Show();
    }
  }
}