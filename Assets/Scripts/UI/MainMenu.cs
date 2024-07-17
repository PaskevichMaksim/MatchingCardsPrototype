using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace UI
{
  public class MainMenu : BaseMenu
  {
    [SerializeField] private Button _startButton;
  
    private UIManager _uiManager;

    [Inject]
    private void Construct(UIManager uiManager)
    {
      _uiManager = uiManager;
    }

    public override void Awake()
    {
      _startButton.onClick.AddListener(OnStartButtonClicked);
      base.Awake();
    }

    private void OnDestroy()
    {
      _startButton.onClick.RemoveListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
      _uiManager.StartGame();
      gameObject.SetActive(false);
    }
  }
}