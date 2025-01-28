using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("Prefabs")]
    public SyzozSoundManager soundManagerPrefab;
    public SyzozManager ManagerPrefab;
    public SyzozMain MainPrefabe;
    public SyzozUI UiPrefabe;

    public SyzozCanvas CanvasSo;
    public SyzozWeapons WeaponsSo;
    public SyzozActors ActorsSo;
    public SyzozAddresable AddresableSo;

    public override void InstallBindings()
    {
        // Bind ScriptableObjects as singletons
        Container.Bind<SyzozCanvas>().FromInstance(CanvasSo).AsSingle();
        Container.Bind<SyzozWeapons>().FromInstance(WeaponsSo).AsSingle();
        Container.Bind<SyzozActors>().FromInstance(ActorsSo).AsSingle();
        Container.Bind<SyzozAddresable>().FromInstance(AddresableSo).AsSingle();

        // Bind core systems (lazy instantiation)
        Container.Bind<SyzozManager>().FromComponentInNewPrefab(ManagerPrefab).AsSingle(); // Removed .NonLazy().
        Container.Bind<SyzozSoundManager>().FromComponentInNewPrefab(soundManagerPrefab).AsSingle(); // Removed .NonLazy().
        Container.Bind<SyzozMain>().FromComponentInNewPrefab(MainPrefabe).AsSingle(); // Removed .NonLazy().
        Container.Bind<SyzozUI>().FromComponentInNewPrefab(UiPrefabe).AsSingle(); // Removed .NonLazy().
    }

}