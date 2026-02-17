using UnityEngine;
using Zenject;

public class SignalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<GameSignal.OnShapePlaced>();
        Container.DeclareSignal<GameSignal.OnSlotCleared>();
        Container.DeclareSignal<GameSignal.OnMatchesFound>();

        Container.BindSignal<GameSignal.OnSlotCleared>()
            .ToMethod<SpawnManager>((x) => x.CheckAndSpawnNewShapes)
            .FromResolve();
    }
}