using System;
using Tools.SimpleEventBus;
using UnityEngine;
using Zenject;

public class Spawner : ITickable
{
    public float CurrentTime => _currentTime;
    
    private readonly float _timeSpawn;
    private readonly int _spawnID;
    private readonly MergeObjectProvider _mergeObjectProvider;
    private readonly SlotManager _slotManager;
    
    private float _currentTime;

    public Spawner(Settings settings, MergeObjectProvider mergeObjectProvider, SlotManager slotManager)
    {
        _spawnID = settings.SpawnID;
        _slotManager = slotManager;
        _timeSpawn = settings.TimeSpawn;
        _mergeObjectProvider = mergeObjectProvider;

        _currentTime = _timeSpawn;
    }
    
    public void Tick()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0)
        {
            _currentTime = _timeSpawn;

            if (_slotManager.CanFreeSlot())
            {
                var slot = _slotManager.GetRandomFreeSlot();
                var mergeObject = _mergeObjectProvider.GetGameObjectByLevel(_spawnID);
                slot.SetMergeObject(mergeObject);
                EventStreams.UserInterface.Publish(new EventSpawn(slot));
            }
        }
    }

    [Serializable]
    public class Settings
    {
        public float TimeSpawn;
        public int SpawnID;
    }
}
