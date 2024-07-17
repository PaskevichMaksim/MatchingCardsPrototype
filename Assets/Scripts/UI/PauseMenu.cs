using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace UI
{
  public class PauseMenu : BaseMenu
  {
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _menuButton;

    private UIManager _uiManager;

    [Inject]
    private void Construct (UIManager uiManager)
    {
      _uiManager = uiManager;
    }

    public override void Awake()
    {
      _continueButton.onClick.AddListener(OnContinueButtonClicked);
      _menuButton.onClick.AddListener(OnMenuButtonClicked);
      base.Awake();
    }

    private void OnDestroy()
    {
      _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
      _menuButton.onClick.RemoveListener(OnMenuButtonClicked);
    }

    private void OnContinueButtonClicked()
    {
      _uiManager.TogglePauseMenu();
    }

    private void OnMenuButtonClicked()
    {
      Time.timeScale = 1;  
      _uiManager.ShowMainMenu();
    }
  }
}