using UnityEngine;
using Zenject;

public class SignalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        //Block Place Events
        Container.DeclareSignal<GameSignal.OnShapePlaced>();
        //Game Settings Signals
        Container.DeclareSignal<GameSignal.OnGameStateChanged>();
        //Match Singals
        Container.DeclareSignal<GameSignal.OnMatchesFound>();
        
        //UI Button Signals
        Container.DeclareSignal<GameSignal.OnClickPlayButton>();
        Container.DeclareSignal<GameSignal.OnClickBackButton>();
        Container.DeclareSignal<GameSignal.OnClickLevelButton>();
        Container.DeclareSignal<GameSignal.OnClickExitButton>();
        //TODO Exit button için application quit yazýlacak.


        Container.BindSignal<GameSignal.OnClickPlayButton>()
            .ToMethod<UIManager>(x => x.HandleClickedPlayButton)
            .FromResolve();

        Container.BindSignal<GameSignal.OnClickBackButton>()
            .ToMethod<UIManager>(x => x.HandleClickedBackButton)
            .FromResolve();

        Container.BindSignal<GameSignal.OnClickExitButton>()
            .ToMethod<UIManager>(x => x.HandleClickedExitButton)
            .FromResolve();

        Container.BindSignal<GameSignal.OnClickLevelButton>()
            .ToMethod<LevelManager>((x) => x.OnLevelChanged)
            .FromResolve();


    }
}