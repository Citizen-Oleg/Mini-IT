using Tools;
using UnityEngine;
using Zenject;

public class MergeInstaller : MonoInstaller
{
    [SerializeField]
    private SlotManager.Settings _settingsSlotManager;
    [SerializeField]
    private Spawner.Settings _settingsSpawner;
    [SerializeField]
    private SpawnTimerView.Settings _settingsSpawnTimer;

    public override void InstallBindings()
    {
        Container.BindInstance(_settingsSlotManager);
        Container.BindInstance(_settingsSpawner);
        Container.BindInstance(_settingsSpawnTimer);

        Container.Bind<SlotManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Spawner>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SpawnTimerView>().AsSingle().NonLazy();
    }
}
