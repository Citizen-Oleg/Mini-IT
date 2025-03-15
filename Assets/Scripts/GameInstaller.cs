using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    private MergeObjectProvider.Settings _settingsProvider;
    [SerializeField]
    private ParticleSpawner.Settings _settingsParticle;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_settingsProvider);
        Container.BindInstance(_settingsParticle);
        
        Container.Bind<MergeObjectProvider>().AsSingle().WithArguments(transform).NonLazy();
        Container.Bind<ParticleSpawner>().AsSingle().WithArguments(transform).NonLazy();
    }
}    
