using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace UI
{
    public class GameHUD : BaseMenu
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Button _hintButton;
        [SerializeField] private Button _pauseButton;

        private float _timeRemaining;
        private bool _timerIsRunning;
        private Coroutine _timerCoroutine;

        private GameManager _gameManager;
        private HintSystem _hintSystem;
        private UIManager _uiManager;

        public Button HintButton => _hintButton;

        [Inject]
        public void Construct(GameManager gameManager, HintSystem hintSystem, UIManager uiManager)
        {
            _gameManager = gameManager;
            _hintSystem = hintSystem;
            _uiManager = uiManager;
        
        }

        public override void Awake()
        {
            _hintButton.onClick.AddListener(OnHintButtonClicked);
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
            _gameManager.OnLevelStarted += UpdateLevelText;
            base.Awake();
        }

        private void OnEnable()
        {
            _gameManager.OnStartTimer += StartTimer;
        }

        private void OnDisable()
        {
            _gameManager.OnStartTimer -= StartTimer;
        }

        private void OnDestroy()
        {
            _hintButton.onClick.RemoveListener(OnHintButtonClicked);
            _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
        }

        private void StartTimer(float duration)
        {
            StopTimer();
            _timeRemaining = duration;
            UpdateTimerText();
            _timerIsRunning = true;
            _timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        public void StopTimer()
        {
            _timerIsRunning = false;
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
            }
        }

        private IEnumerator TimerCoroutine()
        {
            while (_timerIsRunning)
            {
                if (_timeRemaining > 0)
                {
                    _timeRemaining -= Time.deltaTime;
                    UpdateTimerText();
                } else
                {
                    _timeRemaining = 0;
                    UpdateTimerText();
                    _timerIsRunning = false;
                    _gameManager.OnTimeOut();
                }
                yield return null;
            }
        }

        private void UpdateTimerText()
        {
            int minutes = Mathf.FloorToInt(_timeRemaining / 60);
            int seconds = Mathf.FloorToInt(_timeRemaining % 60);
            _timerText.text = $"{minutes:00}:{seconds:00}";
        }

        private void OnHintButtonClicked()
        {
            _hintSystem.ShowHint();
        }

        private void OnPauseButtonClicked()
        {
            _uiManager.TogglePauseMenu();
        }

        public void UpdateLevelText(int level)
        {
            _levelText.text = $"Level: {level}";
        }
    }
}
