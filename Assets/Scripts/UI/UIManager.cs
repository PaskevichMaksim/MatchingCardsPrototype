using UnityEngine;
using Zenject;
namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private ConclusionMenu _conclusionMenu;
        [SerializeField] private PauseMenu _pauseMenu;
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private GameHUD _gameHUD;

        private GameManager _gameManager;

        [Inject]
        public void Construct(GameManager gameManager,HintSystem hintSystem)
        {
            _gameManager = gameManager;
            _gameManager.OnGameEnd += ShowConclusionPanel;
        }

        private void Start()
        {
            ShowMainMenu();
        }

        private void OnDestroy()
        {
            _gameManager.OnGameEnd -= ShowConclusionPanel;
        }
    

        public void SetHintButtonInteractable(bool interactable)
        {
            _gameHUD.HintButton.interactable = interactable;
        }

        private void ShowConclusionPanel(bool isWin)
        {
            _gameHUD.StopTimer();
            _conclusionMenu.Show();
            _conclusionMenu.ShowConclusionPanel(isWin);
        }
    
        public void TogglePauseMenu()
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                _pauseMenu.Hide();
            } else
            {
                Time.timeScale = 0;
                _pauseMenu.Show();
            }
        }

        public void ShowMainMenu()
        {
            _mainMenu.Show();
            _gameHUD.Hide();
            _pauseMenu.Hide();
            _conclusionMenu.Hide();
        }

        public void StartGame()
        {
            _mainMenu.Hide();
            _gameHUD.Show();
            _gameHUD.UpdateLevelText(_gameManager.CurrentLevel);
            _gameManager.StartGame();
        }
    }
}
