using System;
using System.Collections.Generic;
using Base.SimpleEventBus_and_MonoPool;
using UnityEngine;

public class MergeObjectProvider
{
    public int MaxLevel => _maxLevel;
        
    private Dictionary<int, DefaultMonoBehaviourPool<MergeObject>> _gameObjectPools = new Dictionary<int, DefaultMonoBehaviourPool<MergeObject>>();
    private int _maxLevel;

    public MergeObjectProvider(Settings settings, Transform parent)
    {
        _maxLevel = settings.MergeObjects.Count;
        foreach (var settingsMergeObject in settings.MergeObjects)
        {
            var pool = new DefaultMonoBehaviourPool<MergeObject>(settingsMergeObject, parent);
            _gameObjectPools.Add(settingsMergeObject.ID, pool);
        }
    }

    public bool HasNextLevel(int level)
    {
        return _gameObjectPools.ContainsKey(level + 1);
    }

    public MergeObject GetGameObjectByLevel(int level)
    {
        var mergeObject = _gameObjectPools[level].Take();
        mergeObject.Initialize(this);
        return mergeObject;
    }

    public void ReleaseModel(MergeObject mergeObject)
    {
        _gameObjectPools[mergeObject.ID].Release(mergeObject);
    }

    public void ReleaseAll()
    {
        foreach (var gameObjectPool in _gameObjectPools)
        {
            gameObjectPool.Value.ReleaseAll();
        }
    }
        
    [Serializable]
    public class Settings
    {
        public List<MergeObject> MergeObjects = new List<MergeObject>();
    }
}