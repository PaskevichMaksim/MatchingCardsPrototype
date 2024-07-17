using Save;
using UI;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject _cardPrefab;

    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private GridManager _gridManager;
    [SerializeField]
    private HintSystem _hintSystem;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private SoundManager _soundManager;
    
    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle();
        Container.Bind<GridManager>().FromInstance(_gridManager).AsSingle();
        Container.Bind<HintSystem>().FromInstance(_hintSystem).AsSingle();
        Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle();
        Container.Bind<SoundManager>().FromInstance(_soundManager).AsSingle();

        Container.BindFactory<CardController, CardController.Factory>().FromComponentInNewPrefab(_cardPrefab);
        
        Save();
    }

    private void Save()
    {
        Container.BindInterfacesAndSelfTo<JsonSaveSystem>().AsSingle().WithArguments("game_data.json");
        Container.BindInterfacesAndSelfTo<PlayerPrefsSaveSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<Base64SaveSystem>().AsSingle();
        Container.Bind<SaveManager>().AsSingle();
    }
}