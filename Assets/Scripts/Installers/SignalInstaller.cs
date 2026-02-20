using UnityEngine;
using Zenject;

public class SignalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        //Block Place Events
        Container.DeclareSignal<GameSignal.OnShapePlaced>();
        Container.DeclareSignal<GameSignal.OnAllShapePlaced>();

        //Game Settings Signals
        Container.DeclareSignal<GameSignal.OnGameLevelPassed>();
        Container.DeclareSignal<GameSignal.OnGameStateChanged>();
        Container.DeclareSignal<GameSignal.OnGameOver>();
        Container.DeclareSignal<GameSignal.OnBoardGenerated>();

        Container.DeclareSignal<GameSignal.OnSlotCleared>();
        Container.DeclareSignal<GameSignal.OnMatchesFound>();
        Container.DeclareSignal<GameSignal.OnSpawnedNewBlocks>();

        Container.BindSignal<GameSignal.OnSlotCleared>()
            .ToMethod<SpawnManager>((x) => x.CheckAndSpawnNewShapes)
            .FromResolve();


    }
}