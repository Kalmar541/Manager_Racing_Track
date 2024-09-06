using UnityEngine;
using Zenject;

public class TestInstaller : MonoInstaller
{
    [SerializeField] private ParkingManager parkingManager_scr; // парковка где расположены машины
    [SerializeField] private MergeManager mergeManager_scr; // список машин для марджа
    [SerializeField] private PlayerStats playerStats_scr; // данные мгрока
    [SerializeField] private Kassa kassa_scr; // данные мгрока
    [SerializeField] private GameOverlayUI gameOverlayUI_scr; // Скрипт UI в режиме игры
    [SerializeField] private QuestSystem questSystem_scr;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private PrestigeCounter _prestigeCounter; // счетчик престижа
    [SerializeField] private StartZone _startZone;
    [SerializeField] private ControllerMoveCamera _controllerMoveCamera;

    public override void InstallBindings()
    {
        Container.Bind<ParkingManager>().FromInstance(parkingManager_scr).AsSingle().NonLazy();
        Container.Bind<MergeManager>().FromInstance(mergeManager_scr).AsSingle().NonLazy();
        Container.Bind<PlayerStats>().FromInstance(playerStats_scr).AsSingle().NonLazy();
        Container.Bind<Kassa>().FromInstance(kassa_scr).AsSingle().NonLazy();
        Container.Bind<GameOverlayUI>().FromInstance(gameOverlayUI_scr).AsSingle().NonLazy();
        Container.Bind<QuestSystem>().FromInstance(questSystem_scr).AsSingle().NonLazy();
        Container.Bind<SoundManager>().FromInstance(_soundManager).AsSingle().NonLazy();
        Container.Bind<PrestigeCounter>().FromInstance(_prestigeCounter).AsSingle().NonLazy();
        Container.Bind<StartZone>().FromInstance(_startZone).AsSingle().NonLazy();
        Container.Bind<ControllerMoveCamera>().FromInstance(_controllerMoveCamera).AsSingle().NonLazy();
    }
}

