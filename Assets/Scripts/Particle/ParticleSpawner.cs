using System;
using Base.SimpleEventBus_and_MonoPool;
using UnityEngine;

public class ParticleSpawner
{
    private DefaultMonoBehaviourPool<ParticleObject> _defaultMonoBehaviourPool;
    
    public ParticleSpawner(Settings settings, Transform container)
    {
        _defaultMonoBehaviourPool = 
            new DefaultMonoBehaviourPool<ParticleObject>(settings.ParticleObject, container);
    }

    public void Release(ParticleObject particle)
    {
        _defaultMonoBehaviourPool.Release(particle);
    }

    public void Show(Vector3 position)
    {
        var fx = _defaultMonoBehaviourPool.Take();
        fx.Initialize(this);
        fx.transform.position = position;
        fx.Play();
    }

    [Serializable]
    public class Settings
    {
        public ParticleObject ParticleObject;
    }
}
